namespace MeetingRooms.Domain.Common;

public static class DateTimeUtc
{
  public static DateTime Normalize(DateTime value) => value.Kind switch
  {
    DateTimeKind.Utc => value,
    DateTimeKind.Local => value.ToUniversalTime(),
    _ => DateTime.SpecifyKind(value, DateTimeKind.Utc),
  };
}
