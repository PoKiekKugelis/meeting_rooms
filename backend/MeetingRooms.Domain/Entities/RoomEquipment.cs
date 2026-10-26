using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

public class RoomEquipment
{
  private RoomEquipment()
  {
  }

  public RoomEquipment(Room room, Equipment equipment, int quantity = 1)
  {
    ArgumentNullException.ThrowIfNull(room);
    ArgumentNullException.ThrowIfNull(equipment);

    Room = room;
    RoomId = room.Id;
    Equipment = equipment;
    EquipmentId = equipment.Id;
    Quantity = DomainGuard.EnsureInRange(quantity, 1, int.MaxValue, nameof(quantity));
  }

  public int RoomId { get; private set; }
  public Room Room { get; private set; } = null!;
  public int EquipmentId { get; private set; }
  public Equipment Equipment { get; private set; } = null!;
  public int Quantity { get; private set; }
}
