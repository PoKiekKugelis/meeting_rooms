namespace MeetingRooms.Core.DTOs;

/// <summary>
/// Represents a data transfer object for reservation details
/// </summary>
/// <param name="Id">The unique identifier for the reservation</param>
/// <param name="RoomId">The identifier for the room reserved</param>
/// <param name="RoomName">The name of the room reserved</param>
/// <param name="UserId">The identifier for the user who made the reservation</param>
/// <param name="UserName">The name of the user who made the reservation</param>
/// <param name="Title">The title of the reservation</param>
/// <param name="StartDate">The start date and time of the reservation</param>
/// <param name="EndDate">The end date and time of the reservation</param>
/// <param name="AttendeeCount">The number of attendees for the reservation</param>
/// <param name="Status">The status of the reservation</param>
/// <param name="CompletedAt">The date and time when the reservation was completed</param>
public sealed record ReservationDto(
    int Id,
    int RoomId,
    string RoomName,
    int UserId,
    string UserName,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    int AttendeeCount,
    string Status,
    DateTime? CompletedAt);

/// <summary>
/// Represents a request to create a new reservation
/// </summary>
/// <param name="RoomId">The identifier for the room to be reserved</param>
/// <param name="UserId">The identifier for the user making the reservation</param>
/// <param name="Title">The title of the reservation</param>
/// <param name="StartDate">The start date and time of the reservation</param>
/// <param name="EndDate">The end date and time of the reservation</param>
/// <param name="AttendeeCount">The number of attendees for the reservation</param>
public sealed record ReservationRequest(
    int RoomId,
    int UserId,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    int AttendeeCount);

/// <summary>
/// Represents the input for reservation policy validation
/// </summary>
/// <param name="RoomId">The identifier for the room to be reserved</param>
/// <param name="UserId">The identifier for the user making the reservation</param>
/// <param name="AttendeeCount">The number of attendees for the reservation</param>
/// <param name="StartDate">The start date and time of the reservation</param>
/// <param name="EndDate">The end date and time of the reservation</param>
public sealed record ReservationPolicyInput(
    int RoomId,
    int UserId,
    int AttendeeCount,
    DateTime StartDate,
    DateTime EndDate);

