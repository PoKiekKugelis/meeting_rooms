namespace MeetingRooms.Domain.Entities;

public class RoomEquipment
{
  public int RoomId { get; set; }

  public Room Room { get; set; } = null!;

  public int EquipmentId { get; set; }

  public Equipment Equipment { get; set; } = null!;

  public int Quantity { get; set; }
}
