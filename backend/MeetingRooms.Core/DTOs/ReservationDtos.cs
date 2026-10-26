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

public sealed record CreateReservationDto(
    int RoomId,
    string RoomName,
    int UserId,
    string UserName,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    int AttendeeCount);
