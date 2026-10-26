namespace MeetingRooms.Core.DTOs;

public sealed record RoomDto(
    int Id,
    string Name,
    int Capacity,
    string Location,
    string Status,
    bool IsCurrentlyReserved,
    IReadOnlyCollection<string> Equipment);

public sealed record CreateRoomDto(
    string Name,
    int Capacity,
    string Location,
    IReadOnlyCollection<string> Equipment);
