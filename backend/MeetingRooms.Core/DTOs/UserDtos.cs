namespace MeetingRooms.Core.DTOs;

public sealed record UserDto(
    int Id,
    string FirstName,
    string LastName);

public sealed record UserViewDto(
    int Id,
    string FirstName,
    string LastName,
    int ActiveReservationsCount);

public sealed record CreateUserDto(string FirstName, string LastName);
