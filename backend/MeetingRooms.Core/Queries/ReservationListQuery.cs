using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Queries;

/// <summary>
/// Represents a query for filtering reservations based on various criteria
/// </summary>
/// <param name="RoomId">The ID of the room to filter by</param>
/// <param name="UserId">The ID of the user to filter by</param>
/// <param name="Status">The status to filter by</param>
/// <param name="From">The start date to filter by</param>
/// <param name="To">The end date to filter by</param>
public sealed record ReservationListQuery(
    int? RoomId,
    int? UserId,
    ReservationStatus? Status,
    DateTime? From,
    DateTime? To);
