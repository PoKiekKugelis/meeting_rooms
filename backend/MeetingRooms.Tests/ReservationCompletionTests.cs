using MapsterMapper;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Mapping;
using MeetingRooms.Core.Policies;
using MeetingRooms.Core.Services;
using MeetingRooms.Core.Validators;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Infrastructure.Repositories;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Tests;

public sealed class ReservationCompletionTests
{
  private static readonly DateTime Now = new(2026, 10, 26, 9, 0, 0, DateTimeKind.Utc);

  [Fact]
  public void Complete_ClampsCompletedAtToEndDate_WhenCalledAfterTheMeeting()
  {
    var reservation = CreateReservation(Now.AddHours(-2), Now.AddHours(-1));

    reservation.Complete(Now);

    Assert.Equal(ReservationStatus.Completed, reservation.Status);
    Assert.Equal(reservation.EndDate, reservation.CompletedAt);
  }

  [Fact]
  public void Complete_RecordsActualTime_WhenEndedEarly()
  {
    var reservation = CreateReservation(Now.AddHours(-1), Now.AddHours(1));

    reservation.Complete(Now);

    Assert.Equal(ReservationStatus.Completed, reservation.Status);
    Assert.Equal(Now, reservation.CompletedAt!.Value);
  }

  [Fact]
  public void Complete_Throws_WhenMeetingHasNotStarted()
  {
    var reservation = CreateReservation(Now.AddHours(1), Now.AddHours(2));

    Assert.Throws<InvalidOperationException>(() => reservation.Complete(Now));
  }

  [Fact]
  public void Cancel_Throws_WhenMeetingHasEnded()
  {
    var reservation = CreateReservation(Now.AddHours(-2), Now.AddHours(-1));

    Assert.Throws<InvalidOperationException>(() => reservation.Cancel(Now));
  }

  [Fact]
  public async Task CompleteAsync_CompletesAnOngoingReservation()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var reservation = AddReservation(world, "Ongoing", Now.AddHours(-1), Now.AddHours(1));
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    var result = await service.CompleteAsync(reservation.Id, CancellationToken.None);

    Assert.True(result.IsSuccess);
    Assert.Equal("Completed", result.Value!.Status);
    Assert.Equal(Now, result.Value.CompletedAt!.Value);
    Assert.Equal(ReservationStatus.Completed, reservation.Status);
  }

  [Fact]
  public async Task CompleteAsync_ReturnsNotCompletable_WhenAlreadyCompleted()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var reservation = AddReservation(world, "Ongoing", Now.AddHours(-1), Now.AddHours(1));
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    await service.CompleteAsync(reservation.Id, CancellationToken.None);
    var second = await service.CompleteAsync(reservation.Id, CancellationToken.None);

    Assert.False(second.IsSuccess);
    Assert.Equal(BookingErrorCodes.ReservationNotCompletable, second.ErrorCode);
  }

  [Fact]
  public async Task CompleteAsync_ReturnsNotCompletable_WhenMeetingHasNotStarted()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var reservation = AddReservation(world, "Upcoming", Now.AddHours(1), Now.AddHours(2));
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    var result = await service.CompleteAsync(reservation.Id, CancellationToken.None);

    Assert.False(result.IsSuccess);
    Assert.Equal(BookingErrorCodes.ReservationNotCompletable, result.ErrorCode);
  }

  [Fact]
  public async Task CancelAsync_ReturnsNotEditable_WhenMeetingHasEnded()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var reservation = AddReservation(world, "Ended", Now.AddHours(-2), Now.AddHours(-1));
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    var result = await service.CancelAsync(reservation.Id, CancellationToken.None);

    Assert.False(result.IsSuccess);
    Assert.Equal(BookingErrorCodes.ReservationNotEditable, result.ErrorCode);
    Assert.Equal(ReservationStatus.Booked, reservation.Status);
  }

  [Fact]
  public async Task UpdateAsync_ReturnsNotEditable_WhenMeetingHasEnded()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var reservation = AddReservation(world, "Ended", Now.AddHours(-2), Now.AddHours(-1));
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    var request = new ReservationRequest(
        world.Room.Id,
        world.User.Id,
        "Moved",
        Now.AddDays(1),
        Now.AddDays(1).AddHours(1),
        2);

    var result = await service.UpdateAsync(reservation.Id, request, CancellationToken.None);

    Assert.False(result.IsSuccess);
    Assert.Equal(BookingErrorCodes.ReservationNotEditable, result.ErrorCode);
    Assert.Equal("Ended", reservation.ReservationTitle);
  }

  [Fact]
  public async Task CompleteExpiredAsync_CompletesOnlyExpiredBookedReservations()
  {
    var world = await CreateTestContext();
    using var _ = world.Db;

    var expired = AddReservation(world, "Expired", Now.AddHours(-2), Now.AddHours(-1));
    var future = AddReservation(world, "Future", Now.AddHours(1), Now.AddHours(2));
    var cancelled = AddReservation(world, "Cancelled", Now.AddHours(-2), Now.AddHours(-1));
    cancelled.Cancel(Now.AddHours(-3));
    var completed = AddReservation(world, "Completed", Now.AddHours(-2), Now.AddHours(-1));
    completed.Complete(completed.EndDate);
    await world.Db.SaveChangesAsync();

    var service = CreateService(world, new FixedTimeProvider(Now));
    var count = await service.CompleteExpiredAsync(Now, CancellationToken.None);

    Assert.Equal(1, count);
    Assert.Equal(ReservationStatus.Completed, expired.Status);
    Assert.Equal(expired.EndDate, expired.CompletedAt);
    Assert.Equal(ReservationStatus.Booked, future.Status);
    Assert.Equal(ReservationStatus.Cancelled, cancelled.Status);
    Assert.Equal(ReservationStatus.Completed, completed.Status);
  }

  private static Reservation CreateReservation(DateTime start, DateTime end)
  {
    var room = new Room("A", 10, "Floor 1");
    var user = new User("Jonas", "Jonaitis");
    return new Reservation("Meeting", start, end, 2, room, user);
  }

  private static async Task<TestContext> CreateTestContext()
  {
    var db = TestDbContext.Create();
    var room = new Room("A", 10, "Floor 1");
    var user = new User("Jonas", "Jonaitis");

    db.Rooms.Add(room);
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return new TestContext(db, room, user);
  }

  private static Reservation AddReservation(
      TestContext world,
      string title,
      DateTime start,
      DateTime end)
  {
    var reservation = new Reservation(title, start, end, 2, world.Room, world.User);
    world.Db.Reservations.Add(reservation);
    return reservation;
  }

  private static ReservationService CreateService(TestContext world, TimeProvider clock)
  {
    var reservations = new ReservationRepository(world.Db);
    var options = Options.Create(new ReservationBookingOptions
    {
      MaximumDurationDays = 7,
      MaximumBookingHorizonDays = 90,
    });

    return new ReservationService(
        new Mapper(MapsterConfiguration.Create()),
        new ReservationRequestValidator(options, clock),
        new ReservationBookingPolicy(reservations),
        reservations,
        new RoomRepository(world.Db),
        new UserRepository(world.Db),
        world.Db,
        clock);
  }

  private sealed record TestContext(
    AppDbContext Db,
    Room Room,
    User User);
}
