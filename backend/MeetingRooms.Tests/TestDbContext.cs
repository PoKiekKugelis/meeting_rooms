using MeetingRooms.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Tests;

internal sealed class FixedTimeProvider(DateTime now) : TimeProvider
{
  public override DateTimeOffset GetUtcNow() => new(now, TimeSpan.Zero);
}

internal static class TestDbContext
{
  public static AppDbContext Create()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase($"meeting-rooms-tests-{Guid.NewGuid()}")
        .Options;

    return new AppDbContext(options);
  }
}
