using MeetingRooms.Domain.Common;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Tests;

public sealed class DateTimeUtcTests
{
  [Fact]
  public void Normalize_TreatsUnspecifiedAsUtc()
  {
    var value = new DateTime(2026, 10, 27, 10, 0, 0, DateTimeKind.Unspecified);

    var result = DateTimeUtc.Normalize(value);

    Assert.Equal(DateTimeKind.Utc, result.Kind);
    Assert.Equal(value.Ticks, result.Ticks);
  }

  [Fact]
  public void Normalize_ConvertsLocalToUtc()
  {
    var value = new DateTime(2026, 10, 27, 10, 0, 0, DateTimeKind.Local);

    var result = DateTimeUtc.Normalize(value);

    Assert.Equal(DateTimeKind.Utc, result.Kind);
    Assert.Equal(value.ToUniversalTime(), result);
  }

  [Fact]
  public void Reservation_StoresDatesAsUtc()
  {
    var room = new Room("vienas", 10, "Floor 1");
    var user = new User("Antanas", "Antaninis");
    var start = new DateTime(2026, 10, 27, 10, 0, 0, DateTimeKind.Unspecified);

    var reservation = new Reservation("Planning", start, start.AddHours(1), 2, room, user);

    Assert.Equal(DateTimeKind.Utc, reservation.StartDate.Kind);
    Assert.Equal(DateTimeKind.Utc, reservation.EndDate.Kind);
  }
}
