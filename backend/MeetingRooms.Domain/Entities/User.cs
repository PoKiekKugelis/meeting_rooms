using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

public class User
{
  public const int MaxNameLength = 100;

  private User()
  {
  }

  public User(string firstName, string lastName)
  {
    FirstName = DomainGuard.NormalizeRequiredText(firstName, MaxNameLength, nameof(firstName));
    LastName = DomainGuard.NormalizeRequiredText(lastName, MaxNameLength, nameof(lastName));
  }

  public int Id { get; private set; }
  public string FirstName { get; private set; } = string.Empty;
  public string LastName { get; private set; } = string.Empty;
  public ICollection<Reservation> Reservations { get; private set; } = [];
}
