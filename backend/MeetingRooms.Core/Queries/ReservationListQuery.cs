using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Queries;

public sealed record ReservationListQuery(
    int? RoomId,
    int? UserId,
    ReservationStatus? Status,
    DateTime? From,
    DateTime? To);
