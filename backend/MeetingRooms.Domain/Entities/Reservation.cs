namespace MeetingRooms.Domain.Entities;

public class Reservation
{
  public int Id { get; set; }
  public string ReservationTitle { get; set; } = string.Empty;
  public int AttendeeCount { get; set; }
  public DateTime EndDate { get; set; }
  public DateTime StartDate { get; set; }
  public ReservationStatus Status { get; set; }
  public int RoomId { get; set; }
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  public Room Room { get; set; } = null!;
}
