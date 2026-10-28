namespace MeetingRooms.Core.Configuration;

/// <summary>
/// Represents configuration options for reservation booking, including maximum duration and booking horizon
/// </summary>
public sealed class ReservationBookingOptions
{
  public const string SectionName = "ReservationBooking";

  public int MaximumDurationDays { get; set; } = 7;

  public int MaximumBookingHorizonDays { get; set; } = 90;
}
