using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

/// <summary>
/// Represents a piece of equipment that can be associated with a room
/// </summary>
public class Equipment
{
  /// <summary>
  /// The maximum length for the equipment name
  /// </summary>
  public const int MaxNameLength = 100;

  /// <summary>
  /// Initializes a new instance of the <see cref="Equipment"/> class
  /// </summary>
  private Equipment()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Equipment"/> class with the specified name
  /// </summary>
  /// <param name="name">The name of the equipment</param>
  public Equipment(string name)
  {
    Name = DomainGuard.NormalizeRequiredText(name, MaxNameLength, nameof(name));
  }

  /// <summary>
  /// The ID of the equipment
  /// </summary>
  public int Id { get; private set; }
  /// <summary>
  /// The name of the equipment
  /// </summary>
  public string Name { get; private set; } = string.Empty;
  /// <summary>
  /// The collection of rooms that have this equipment
  /// </summary>
  public ICollection<RoomEquipment> Rooms { get; private set; } = [];
}
