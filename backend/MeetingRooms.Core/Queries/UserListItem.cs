namespace MeetingRooms.Core.Queries;

/// <summary>
/// Represents a user with their active reservations count
/// </summary>
/// <param name="Id">The ID of the user</param>
/// <param name="FirstName">The first name of the user</param>
/// <param name="LastName">The last name of the user</param>
/// <param name="ActiveReservationsCount">The count of active reservations for the user</param>
public sealed record UserListItem(
    int Id,
    string FirstName,
    string LastName,
    int ActiveReservationsCount);
