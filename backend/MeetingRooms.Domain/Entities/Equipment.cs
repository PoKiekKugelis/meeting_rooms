using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

public class Equipment
{
  public const int MaxNameLength = 100;

  private Equipment()
  {
  }

  public Equipment(string name)
  {
    Name = DomainGuard.NormalizeRequiredText(name, MaxNameLength, nameof(name));
  }

  public int Id { get; private set; }
  public string Name { get; private set; } = string.Empty;
  public ICollection<RoomEquipment> Rooms { get; private set; } = [];
}
