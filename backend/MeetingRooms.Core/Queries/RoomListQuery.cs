namespace MeetingRooms.Core.Queries;

/// <summary>
/// Represents a query for filtering rooms based on various criteria
/// </summary>
/// <param name="MinimumCapacity">The minimum capacity to filter by</param>
/// <param name="Floor">The floor to filter by</param>
/// <param name="Equipment">The equipment to filter by</param>
/// <param name="IncludeInactive">Whether to include inactive rooms</param>
public sealed record RoomListQuery(
    int? MinimumCapacity,
    string? Floor,
    IReadOnlyCollection<string>? Equipment,
    bool IncludeInactive = true);
