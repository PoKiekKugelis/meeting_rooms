namespace MeetingRooms.Core.DTOs;

public sealed record RoomDto(
    int Id,
    string Name,
    int Capacity,
    string Location,
    string Status,
    IReadOnlyCollection<RoomEquipmentDto> Equipment);

public sealed record CreateRoomDto(
    string Name,
    int Capacity,
    string Location);
