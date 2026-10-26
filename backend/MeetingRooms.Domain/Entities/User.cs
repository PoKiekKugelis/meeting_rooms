namespace MeetingRooms.Domain.Entities;

public class User
{
  public int Id { set; get; }
  public string FirstName { set; get; } = string.Empty;
  public string Lastname { set; get; } = string.Empty;
  public ICollection<Reservation> Reservations { set; get; } = [];
}
