namespace MeetingRooms.Core.DTOs;

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
    string Status);

public sealed record ReservationRequest(
    int RoomId,
    int UserId,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    int AttendeeCount);

public sealed record ReservationPolicyInput(
    int RoomId,
    int UserId,
    int AttendeeCount,
    DateTime StartDate,
    DateTime EndDate);

