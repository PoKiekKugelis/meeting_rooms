using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

/// <summary>
/// Represents a meeting room that can be reserved
/// </summary>
public class Room
{
  /// <summary>
  /// The maximum length for the room name
  /// </summary>
  public const int MaxNameLength = 100;

  /// <summary>
  /// The maximum length for the room location
  /// </summary>
  public const int MaxLocationLength = 150;

  /// <summary>
  /// Initializes a new instance of the <see cref="Room"/> class
  /// </summary>
  private Room()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Room"/> class with the specified name, capacity, and location
  /// </summary>
  /// <param name="name">The name of the room</param>
  /// <param name="capacity">The maximum number of attendees the room can accommodate</param>
  /// <param name="location">The location of the room</param>
  public Room(string name, int capacity, string location)
  {
    Name = DomainGuard.NormalizeRequiredText(name, MaxNameLength, nameof(name));
    Capacity = DomainGuard.EnsureInRange(capacity, 1, int.MaxValue, nameof(capacity));
    Location = DomainGuard.NormalizeRequiredText(location, MaxLocationLength, nameof(location));
  }

  /// <summary>
  /// The ID of the room
  /// </summary>
  public int Id { get; private set; }
  /// <summary>
  /// The name of the room
  /// </summary>
  public string Name { get; private set; } = string.Empty;
  /// <summary>
  /// The maximum number of attendees the room can accommodate
  /// </summary>
  public int Capacity { get; private set; }
  /// <summary>
  /// The location of the room
  /// </summary>
  public string Location { get; private set; } = string.Empty;
  /// <summary>
  /// The status of the room, indicating whether it is active or inactive
  /// </summary>
  public RoomStatus Status { get; private set; } = RoomStatus.Active;
  /// <summary>
  /// The collection of equipment associated with the room
  /// </summary>
  public ICollection<RoomEquipment> Equipment { get; private set; } = [];
  /// <summary>
  /// The collection of reservations made for the room
  /// </summary>
  public ICollection<Reservation> Reservations { get; private set; } = [];

  /// <summary>
  /// Deactivates the room, marking it as inactive and preventing new reservations from being made
  /// </summary>
  public void Deactivate() => Status = RoomStatus.Inactive;

  /// <summary>
  /// Activates the room, marking it as active and allowing new reservations to be made
  /// </summary>
  public void Activate() => Status = RoomStatus.Active;
}
