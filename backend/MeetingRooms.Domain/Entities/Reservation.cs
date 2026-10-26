using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

public class Reservation
{
  public const int MaxTitleLength = 200;

  private Reservation()
  {
  }

  public Reservation(
      string title,
      DateTime startDate,
      DateTime endDate,
      int attendeeCount,
      Room room,
      User user)
  {
    ArgumentNullException.ThrowIfNull(room);
    ArgumentNullException.ThrowIfNull(user);

    ReservationTitle = DomainGuard.NormalizeRequiredText(title, MaxTitleLength, nameof(title));
    if (startDate >= endDate)
    {
      throw new ArgumentException("Reservation end must be after its start.", nameof(endDate));
    }

    AttendeeCount = DomainGuard.EnsureInRange(attendeeCount, 1, room.Capacity, nameof(attendeeCount));

    if (room.Status != RoomStatus.Active)
    {
      throw new ArgumentException("Inactive rooms cannot be reserved.", nameof(room));
    }

    StartDate = startDate;
    EndDate = endDate;
    Room = room;
    RoomId = room.Id;
    User = user;
    UserId = user.Id;
    Status = ReservationStatus.Booked;
  }

  public int Id { get; private set; }
  public string ReservationTitle { get; private set; } = string.Empty;
  public int AttendeeCount { get; private set; }
  public DateTime EndDate { get; private set; }
  public DateTime StartDate { get; private set; }
  public ReservationStatus Status { get; private set; } = ReservationStatus.Booked;
  public int RoomId { get; private set; }
  public int UserId { get; private set; }
  public User User { get; private set; } = null!;
  public Room Room { get; private set; } = null!;

  /// <summary>Cancels a booked reservation.</summary>
  /// <exception cref="InvalidOperationException">The reservation is not booked.</exception>
  public void Cancel()
  {
    if (Status != ReservationStatus.Booked)
    {
      throw new InvalidOperationException("Only booked reservations can be cancelled.");
    }

    Status = ReservationStatus.Cancelled;
  }
}
