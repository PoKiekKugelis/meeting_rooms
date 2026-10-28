namespace MeetingRooms.Core.DTOs;

/// <summary>
/// Represents a data transfer object for room equipment details
/// </summary>
/// <param name="Name">The name of the equipment</param>
/// <param name="Quantity">The quantity of the equipment</param>
public sealed record RoomEquipmentDto(
    string Name,
    int Quantity);
