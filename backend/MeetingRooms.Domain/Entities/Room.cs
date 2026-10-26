using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

public class Room
{
  public const int MaxNameLength = 100;

  public const int MaxLocationLength = 150;

  private Room()
  {
  }

  public Room(string name, int capacity, string location)
  {
    Name = DomainGuard.NormalizeRequiredText(name, MaxNameLength, nameof(name));
    Capacity = DomainGuard.EnsureInRange(capacity, 1, int.MaxValue, nameof(capacity));
    Location = DomainGuard.NormalizeRequiredText(location, MaxLocationLength, nameof(location));
  }

  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public int Capacity { get; private set; }
  public string Location { get; private set; } = string.Empty;
  public RoomStatus Status { get; private set; } = RoomStatus.Active;
  public ICollection<RoomEquipment> Equipment { get; private set; } = [];
  public ICollection<Reservation> Reservations { get; private set; } = [];
}
