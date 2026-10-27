namespace MeetingRooms.Core.Configuration;

public sealed class ReservationCompletionOptions
{
  public const string SectionName = "ReservationCompletion";

  public int SweepIntervalSeconds { get; set; } = 60;
}
