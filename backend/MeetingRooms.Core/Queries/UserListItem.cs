namespace MeetingRooms.Core.Queries;

public sealed record UserListItem(
    int Id,
    string FirstName,
    string LastName,
    int ActiveReservationsCount);
