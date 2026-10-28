using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

/// <summary>
/// Represents a user who can make reservations for meeting rooms
/// </summary>
public class User
{
  /// <summary>
  /// The maximum length for the user's first and last names
  /// </summary>
  public const int MaxNameLength = 100;

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class
  /// </summary>
  private User()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="User"/> class with the specified first and last names
  /// </summary>
  /// <param name="firstName">The user's first name</param>
  /// <param name="lastName">The user's last name</param>
  public User(string firstName, string lastName)
  {
    FirstName = DomainGuard.NormalizeRequiredText(firstName, MaxNameLength, nameof(firstName));
    LastName = DomainGuard.NormalizeRequiredText(lastName, MaxNameLength, nameof(lastName));
  }

  /// <summary>
  /// The ID of the user
  /// </summary>
  public int Id { get; private set; }
  /// <summary>
  /// The user's first name
  /// </summary>
  public string FirstName { get; private set; } = string.Empty;
  /// <summary>
  /// The user's last name
  /// </summary>
  public string LastName { get; private set; } = string.Empty;
  /// <summary>
  /// The collection of reservations made by the user
  /// </summary>
  public ICollection<Reservation> Reservations { get; private set; } = [];
}
