namespace MeetingRooms.Core.Queries;

/// <summary>
/// Represents a query for retrieving a list of users
/// </summary>
/// <param name="IncludeReservationCount">Whether to include each user's non-ended booked reservation count</param>
public sealed record UserListQuery(bool IncludeReservationCount = false);
