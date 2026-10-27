namespace MeetingRooms.Core.Configuration;

public sealed class ReservationBookingOptions
{
  public const string SectionName = "ReservationBooking";

  public int MaximumDurationDays { get; set; }

  public int MaximumBookingHorizonDays { get; set; }
}
