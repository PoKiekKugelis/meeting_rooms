namespace MeetingRooms.Domain.Common;

/// <summary>
/// Provides utility methods for working with DateTime values in UTC
/// </summary>
public static class DateTimeUtc
{
  /// <summary>
  /// Normalizes a DateTime value to UTC, converting local times to UTC
  /// </summary>
  /// <param name="value">The value to normalize</param>
  /// <returns>The normalized value in UTC</returns>
  public static DateTime Normalize(DateTime value) => value.Kind switch
  {
    DateTimeKind.Utc => value,
    DateTimeKind.Local => value.ToUniversalTime(),
    _ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
  };
}
