using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Queries;

namespace MeetingRooms.Tests;

public sealed class UserQueriesTests
{
  [Fact]
  public async Task GetUsersAsync_ActiveReservationCountRequested()
  {
    await using var db = TestDbContext.Create();
    var now = new DateTime(2026, 10, 28, 12, 0, 0, DateTimeKind.Utc);
    var user = new User("Jonas", "Jonaitis");
    var room = new Room("Room A", 8, "Floor 1");

    var ongoingReservation = new Reservation(
        "Ongoing", now.AddHours(-1), now.AddHours(1), 2, room, user);
    var futureReservation = new Reservation(
        "Future", now.AddDays(1), now.AddDays(1).AddHours(1), 2, room, user);
    var endedReservation = new Reservation(
        "Ended", now.AddHours(-3), now.AddHours(-2), 2, room, user);
    var cancelledReservation = new Reservation(
        "Cancelled", now.AddDays(2), now.AddDays(2).AddHours(1), 2, room, user);
    cancelledReservation.Cancel(now);

    db.Users.Add(user);
    db.Rooms.Add(room);
    db.Reservations.AddRange(
        ongoingReservation,
        futureReservation,
        endedReservation,
        cancelledReservation);
    await db.SaveChangesAsync();

    var queries = new UserQueries(db, new FixedTimeProvider(now));

    var result = await queries.GetUsersAsync(
        new UserListQuery(IncludeReservationCount: true),
        CancellationToken.None);

    var summary = Assert.Single(result);
    Assert.Equal(user.Id, summary.Id);
    Assert.Equal(2, summary.ActiveReservationsCount);
  }

  [Fact]
  public async Task GetUsersAsync_WhenReservationCountNotRequested_OmitsCountFromJson()
  {
    await using var db = TestDbContext.Create();
    db.Users.Add(new User("Jonas", "Jonaitis"));
    await db.SaveChangesAsync();

    var queries = new UserQueries(
        db,
        new FixedTimeProvider(new DateTime(2026, 10, 28, 12, 0, 0, DateTimeKind.Utc)));

    var result = await queries.GetUsersAsync(new UserListQuery(), CancellationToken.None);

    var summary = Assert.Single(result);
    Assert.Null(summary.ActiveReservationsCount);
  }
}
