namespace MeetingRooms.Core.DTOs;

/// <summary>
/// Represents a data transfer object for room details
/// </summary>
/// <param name="Id">The unique identifier for the room</param>
/// <param name="Name">The name of the room</param>
/// <param name="Capacity">The capacity of the room</param>
/// <param name="Location">The location (floor) of the room</param>
/// <param name="Status">The status of the room</param>
/// <param name="Equipment">The equipment available in the room</param>
public sealed record RoomDto(
    int Id,
    string Name,
    int Capacity,
    string Location,
    string Status,
    IReadOnlyCollection<RoomEquipmentDto> Equipment);

/// <summary>
/// Represents a request to create a new room
/// </summary>
/// <param name="Name">The name of the room</param>
/// <param name="Capacity">The capacity of the room</param>
/// <param name="Location">The location (floor) of the room</param>
public sealed record RoomRequest(
    string Name,
    int Capacity,
    string Location);
