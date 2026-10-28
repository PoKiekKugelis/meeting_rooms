using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

/// <summary>
/// Represents the association between a room and a piece of equipment
/// </summary>
public class RoomEquipment
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RoomEquipment"/> class
  /// </summary>
  private RoomEquipment()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoomEquipment"/> class with the specified room, equipment, and quantity
  /// </summary>
  /// <param name="room">The room to associate with the equipment</param>
  /// <param name="equipment">The equipment to associate with the room</param>
  /// <param name="quantity">The quantity of the equipment</param>
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

  /// <summary>
  /// The ID of associated room
  /// </summary>
  public int RoomId { get; private set; }
  /// <summary>
  /// The associated room
  /// </summary>
  public Room Room { get; private set; } = null!;
  /// <summary>
  /// The ID of associated equipment
  /// </summary>
  public int EquipmentId { get; private set; }
  /// <summary>
  /// The associated equipment
  /// </summary>
  public Equipment Equipment { get; private set; } = null!;
  /// <summary>
  /// The quantity of the associated equipment in the room
  /// </summary>
  public int Quantity { get; private set; }
}
