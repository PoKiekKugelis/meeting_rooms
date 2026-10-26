namespace MeetingRooms.Domain.Entities;

public class Equipment
{
  public int Id { set; get; }
  public string Name { get; set; } = string.Empty;
  public ICollection<RoomEquipment> Rooms { get; set; } = [];
}
