namespace MeetingRooms.Core.Configuration;

/// <summary>
/// Represents configuration options for reservation completion, including scanning interval for checking expired reservations
/// </summary>
public sealed class ReservationCompletionOptions
{
  public const string SectionName = "ReservationCompletion";

  public int SweepIntervalSeconds { get; set; } = 60;
}
