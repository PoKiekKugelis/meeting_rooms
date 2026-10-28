using MeetingRooms.Core.Policies;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Validators;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Tests;

public sealed class ReservationRulesTests
{
  private static readonly DateTime Now = new(2026, 10, 26, 9, 0, 0, DateTimeKind.Utc);

  [Fact]
  public async Task R1_AllowsExactlySevenDays()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = CreateValidator(clock);

    var start = Now.AddDays(1);
    var request = new ReservationRequest(1, 1, "Seven days", start, start.AddDays(7), 2);

    var result = await validator.ValidateAsync(request);

    Assert.True(result.IsValid);
  }

  [Fact]
  public async Task R1_RejectsMoreThanSevenDays()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = CreateValidator(clock);

    var start = Now.AddDays(1);
    var request = new ReservationRequest(1, 1, "Too long", start, start.AddDays(7).AddSeconds(1), 2);

    var result = await validator.ValidateAsync(request);

    Assert.Contains(result.Errors, error => error.ErrorCode == BookingErrorCodes.MaxDuration);
  }

  [Fact]
  public async Task R4_RejectsStartInThePast()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = CreateValidator(clock);

    var request = new ReservationRequest(1, 1, "Past", Now.AddMinutes(-1), Now.AddHours(1), 2);

    var result = await validator.ValidateAsync(request);

    Assert.Contains(result.Errors, error => error.ErrorCode == BookingErrorCodes.StartNotFuture);
  }

  [Fact]
  public async Task RejectsEndBeforeStart()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = CreateValidator(clock);

    var start = Now.AddDays(1);
    var request = new ReservationRequest(1, 1, "Backwards", start, start.AddHours(-1), 2);

    var result = await validator.ValidateAsync(request);

    Assert.Contains(result.Errors, error => error.ErrorCode == BookingErrorCodes.InvalidTimeRange);
  }

  [Fact]
  public async Task R6_RejectsStartBeyondTheHorizon()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = CreateValidator(clock);

    var start = Now.AddDays(91);
    var request = new ReservationRequest(1, 1, "Too far", start, start.AddHours(1), 2);

    var result = await validator.ValidateAsync(request);

    Assert.Contains(result.Errors, error => error.ErrorCode == BookingErrorCodes.BookingHorizon);
  }

  [Fact]
  public async Task R1_And_R6_UseTheConfiguredLimits()
  {
    var clock = new FixedTimeProvider(Now);
    var validator = new ReservationRequestValidator(
        Options.Create(new ReservationBookingOptions
        {
          MaximumDurationDays = 2,
          MaximumBookingHorizonDays = 10,
        }),
        clock);

    var start = Now.AddDays(1);
    var allowed = new ReservationRequest(1, 1, "Two days", start, start.AddDays(2), 2);
    var tooLong = allowed with { EndDate = start.AddDays(2).AddSeconds(1) };
    var tooFar = allowed with { StartDate = Now.AddDays(11), EndDate = Now.AddDays(11).AddHours(1) };

    Assert.True((await validator.ValidateAsync(allowed)).IsValid);
    Assert.Contains(
        (await validator.ValidateAsync(tooLong)).Errors,
        error => error.ErrorCode == BookingErrorCodes.MaxDuration);
    Assert.Contains(
        (await validator.ValidateAsync(tooFar)).Errors,
        error => error.ErrorCode == BookingErrorCodes.BookingHorizon);
  }

  [Fact]
  public async Task R2_RejectsAnOverlappingRoomBooking()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    var existingStart = Now.AddDays(1);
    AddReservation(Db, "Existing", existingStart, existingStart.AddHours(2), test.Room, test.SecondUser);
    await Db.SaveChangesAsync();

    var error = await CheckAsync(test, test.Room, test.User, existingStart.AddHours(1), existingStart.AddHours(3));

    Assert.Equal(BookingErrorCodes.RoomOverlap, error?.Code);
  }

  [Fact]
  public async Task R2_AllowsAnAdjacentBooking()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    var existingStart = Now.AddDays(1);
    AddReservation(Db, "Existing", existingStart, existingStart.AddHours(2), test.Room, test.SecondUser);
    await Db.SaveChangesAsync();

    var error = await CheckAsync(test, test.Room, test.User, existingStart.AddHours(2), existingStart.AddHours(3));

    Assert.Null(error);
  }

  [Fact]
  public async Task R2_CancelledReservationsDoNotBlock()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    var existingStart = Now.AddDays(1);
    var cancelled = AddReservation(Db, "Cancelled", existingStart, existingStart.AddHours(2), test.Room, test.SecondUser);
    cancelled.Cancel(Now);
    await Db.SaveChangesAsync();

    var error = await CheckAsync(test, test.Room, test.User, existingStart, existingStart.AddHours(1));

    Assert.Null(error);
  }

  [Fact]
  public async Task R3_RejectsTheOrganizerOverlappingInAnotherRoom()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    var existingStart = Now.AddDays(1);
    AddReservation(Db, "Other room", existingStart, existingStart.AddHours(2), test.OtherRoom, test.User);
    await Db.SaveChangesAsync();

    var error = await CheckAsync(test, test.Room, test.User, existingStart.AddHours(1), existingStart.AddHours(3));

    Assert.Equal(BookingErrorCodes.OrganizerOverlap, error?.Code);
  }

  [Fact]
  public async Task R5_RejectsMoreAttendeesThanCapacity()
  {
    var test = await CreateTestContext(capacity: 4);
    using var Db = test.Db;

    var error = await CheckAsync(test, test.Room, test.User, Now.AddDays(1), Now.AddDays(1).AddHours(1), attendeeCount: 5);

    Assert.Equal(BookingErrorCodes.CapacityExceeded, error?.Code);
  }

  [Fact]
  public async Task R7_RejectsInactiveRooms()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    test.Room.Deactivate();

    var error = await CheckAsync(test, test.Room, test.User, Now.AddDays(1), Now.AddDays(1).AddHours(1));

    Assert.Equal(BookingErrorCodes.RoomInactive, error?.Code);
  }

  [Fact]
  public async Task R2_R3_DoNotBlockWhenEditingTheSameReservation()
  {
    var test = await CreateTestContext();
    using var Db = test.Db;

    var start = Now.AddDays(1);
    var reservation = AddReservation(Db, "Editing", start, start.AddHours(2), test.Room, test.User);
    await Db.SaveChangesAsync();

    var error = await CheckAsync(test, test.Room, test.User, start, start.AddHours(2), excludedReservationId: reservation.Id);

    Assert.Null(error);
  }

  private static ReservationRequestValidator CreateValidator(TimeProvider clock)
  {
    return new(Options.Create(new ReservationBookingOptions
    {
      MaximumDurationDays = 7,
      MaximumBookingHorizonDays = 90,
    }), clock);
  }

  private static async Task<(string Code, string Message)?> CheckAsync(
      TestContext test,
      Room room,
      User user,
      DateTime start,
      DateTime end,
      int attendeeCount = 2,
      int? excludedReservationId = null)
  {
    var policy = new ReservationBookingPolicy(new ReservationRepository(test.Db));

    return await policy.CheckAsync(
        new ReservationPolicyInput(room.Id, user.Id, attendeeCount, start, end),
        room,
        user,
        excludedReservationId,
        CancellationToken.None);
  }

  private static Reservation AddReservation(
      AppDbContext db,
      string title,
      DateTime start,
      DateTime end,
      Room room,
      User user)
  {
    var reservation = new Reservation(title, start, end, 2, room, user);
    db.Reservations.Add(reservation);
    return reservation;
  }

  private static async Task<TestContext> CreateTestContext(int capacity = 10)
  {
    var db = TestDbContext.Create();

    var room = new Room("A", capacity, "Floor 1");
    var otherRoom = new Room("B", 10, "Floor 1");
    var user = new User("Jonas", "Jonaitis");
    var secondUser = new User("Antanas", "Antanaitis");

    db.Rooms.AddRange(room, otherRoom);
    db.Users.AddRange(user, secondUser);
    await db.SaveChangesAsync();

    return new TestContext(db, room, otherRoom, user, secondUser);
  }

  private sealed record TestContext(
      AppDbContext Db,
      Room Room,
      Room OtherRoom,
      User User,
      User SecondUser);
}
