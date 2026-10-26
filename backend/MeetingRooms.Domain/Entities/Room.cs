namespace MeetingRooms.Domain.Entities;

public class Room
{
  public int Id { set; get; }
  public string Name { set; get; } = string.Empty;
  public int Capacity { set; get; }
  public string Location { set; get; } = string.Empty;
  public RoomStatus Status { get; set; } = RoomStatus.Active;
  public ICollection<RoomEquipment> Equipment { get; set; } = [];
  public ICollection<Reservation> Reservations { get; set; } = [];
}
