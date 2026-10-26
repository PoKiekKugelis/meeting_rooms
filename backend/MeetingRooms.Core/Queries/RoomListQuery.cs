namespace MeetingRooms.Core.Queries;

public sealed record RoomListQuery(
    int? MinimumCapacity,
    string? Floor,
    IReadOnlyCollection<string>? Equipment,
    bool IncludeInactive = true);
