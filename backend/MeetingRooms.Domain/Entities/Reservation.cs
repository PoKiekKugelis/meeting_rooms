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
    Status = ReservationStatus.Booked;
    SetDetails(title, startDate, endDate, attendeeCount, room, user);
  }

  public int Id { get; private set; }
  public string ReservationTitle { get; private set; } = string.Empty;
  public int AttendeeCount { get; private set; }
  public DateTime EndDate { get; private set; }
  public DateTime StartDate { get; private set; }
  public DateTime? CompletedAt { get; private set; }
  public ReservationStatus Status { get; private set; } = ReservationStatus.Booked;
  public int RoomId { get; private set; }
  public int UserId { get; private set; }
  public User User { get; private set; } = null!;
  public Room Room { get; private set; } = null!;

  public void UpdateDetails(
      string title,
      DateTime startDate,
      DateTime endDate,
      int attendeeCount,
      Room room,
      User user)
  {
    if (Status != ReservationStatus.Booked)
    {
      throw new InvalidOperationException("Only booked reservations can be edited.");
    }

    SetDetails(title, startDate, endDate, attendeeCount, room, user);
  }

  public void Cancel(DateTime now)
  {
    if (Status != ReservationStatus.Booked)
    {
      throw new InvalidOperationException("Only booked reservations can be cancelled.");
    }

    now = DateTimeUtc.Normalize(now);
    if (now >= EndDate)
    {
      throw new InvalidOperationException("Reservations that have ended cannot be cancelled.");
    }

    Status = ReservationStatus.Cancelled;
  }

  public void Complete(DateTime completedAt)
  {
    if (Status != ReservationStatus.Booked)
    {
      throw new InvalidOperationException("Only booked reservations can be completed.");
    }

    completedAt = DateTimeUtc.Normalize(completedAt);
    if (completedAt < StartDate)
    {
      throw new InvalidOperationException("A reservation cannot be completed before it starts.");
    }

    Status = ReservationStatus.Completed;
    CompletedAt = completedAt > EndDate ? EndDate : completedAt;
  }

  private void SetDetails(
      string title,
      DateTime startDate,
      DateTime endDate,
      int attendeeCount,
      Room room,
      User user)
  {
    ArgumentNullException.ThrowIfNull(room);
    ArgumentNullException.ThrowIfNull(user);

    startDate = DateTimeUtc.Normalize(startDate);
    endDate = DateTimeUtc.Normalize(endDate);

    var normalizedTitle = DomainGuard.NormalizeRequiredText(title, MaxTitleLength, nameof(title));
    if (startDate >= endDate)
    {
      throw new ArgumentException("Reservation end must be after its start.", nameof(endDate));
    }

    var normalizedAttendeeCount = DomainGuard.EnsureInRange(attendeeCount, 1, room.Capacity, nameof(attendeeCount));
    if (room.Status != RoomStatus.Active)
    {
      throw new ArgumentException("Inactive rooms cannot be reserved.", nameof(room));
    }

    ReservationTitle = normalizedTitle;
    StartDate = startDate;
    EndDate = endDate;
    AttendeeCount = normalizedAttendeeCount;
    Room = room;
    RoomId = room.Id;
    User = user;
    UserId = user.Id;
  }
}
