using MeetingRooms.Domain.Common;

namespace MeetingRooms.Domain.Entities;

/// <summary>
/// Represents a reservation for a meeting room
/// </summary>
public class Reservation
{
  /// <summary>
  /// The maximum length for the reservation title
  /// </summary>
  public const int MaxTitleLength = 200;

  /// <summary>
  /// Initializes a new instance of the <see cref="Reservation"/> class
  /// </summary>
  private Reservation()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Reservation"/> class with the specified details
  /// </summary>
  /// <param name="title">The title of the reservation</param>
  /// <param name="startDate">The start date and time of the reservation</param>
  /// <param name="endDate">The end date and time of the reservation</param>
  /// <param name="attendeeCount">The number of attendees for the reservation</param>
  /// <param name="room">The room for which the reservation is made</param>
  /// <param name="user">The user who made the reservation</param>
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

  /// <summary>
  /// The ID of the reservation
  /// </summary>
  public int Id { get; private set; }
  /// <summary>
  /// The title of the reservation
  /// </summary>
  public string ReservationTitle { get; private set; } = string.Empty;
  /// <summary>
  /// The number of attendees for the reservation
  /// </summary>
  public int AttendeeCount { get; private set; }
  /// <summary>
  /// The end date and time of the reservation
  /// </summary>
  public DateTime EndDate { get; private set; }
  /// <summary>
  /// The start date and time of the reservation
  /// </summary>
  public DateTime StartDate { get; private set; }
  /// <summary>
  /// The date and time when the reservation was completed
  /// </summary>
  public DateTime? CompletedAt { get; private set; }
  /// <summary>
  /// The status of the reservation
  /// </summary>
  public ReservationStatus Status { get; private set; } = ReservationStatus.Booked;
  /// <summary>
  /// The ID of the room for which the reservation is made
  /// </summary>
  public int RoomId { get; private set; }
  /// <summary>
  /// The ID of the user who made the reservation
  /// </summary>
  public int UserId { get; private set; }
  /// <summary>
  /// The user who made the reservation
  /// </summary>
  public User User { get; private set; } = null!;
  /// <summary>
  /// The room for which the reservation is made
  /// </summary>
  public Room Room { get; private set; } = null!;

  /// <summary>
  /// Updates the details of the reservation if it is currently booked
  /// </summary>
  /// <param name="title">The title of the reservation</param>
  /// <param name="startDate">The start date and time of the reservation</param>
  /// <param name="endDate">The end date and time of the reservation</param>
  /// <param name="attendeeCount">The number of attendees for the reservation</param>
  /// <param name="room">The room for which the reservation is made</param>
  /// <param name="user">The user who made the reservation</param>
  /// <exception cref="InvalidOperationException"></exception>
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

  /// <summary>
  /// Cancels the reservation if it is currently booked
  /// </summary>
  /// <param name="now">The date and time when the cancellation is requested</param>
  /// <exception cref="InvalidOperationException"></exception>
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

  /// <summary>
  /// Completes the reservation if it is currently booked
  /// </summary>
  /// <param name="completedAt">The date and time when the reservation was completed</param>
  /// <exception cref="InvalidOperationException"></exception>
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

  /// <summary>
  /// Sets the details of the reservation, validating the input parameters
  /// </summary>
  /// <param name="title">The title of the reservation</param>
  /// <param name="startDate">The start date and time of the reservation</param>
  /// <param name="endDate">The end date and time of the reservation</param>
  /// <param name="attendeeCount">The number of attendees for the reservation</param>
  /// <param name="room">The room for which the reservation is made</param>
  /// <param name="user">The user who made the reservation</param>
  /// <exception cref="ArgumentException"></exception>
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
