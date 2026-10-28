using System.Text.Json.Serialization;

namespace MeetingRooms.Core.DTOs;

/// <summary>
/// Represents a data transfer object for user details
/// </summary>
/// <param name="Id">The unique identifier for the user</param>
/// <param name="FirstName">The first name of the user</param>
/// <param name="LastName">The last name of the user</param>
public sealed record UserDto(
    int Id,
    string FirstName,
    string LastName);

/// <summary>
/// Represents a data transfer object for user summary details
/// </summary>
/// <param name="Id">The unique identifier for the user</param>
/// <param name="FirstName">The first name of the user</param>
/// <param name="LastName">The last name of the user</param>
/// <param name="ActiveReservationsCount">The count of active reservations for the user</param>
public sealed record UserSummaryDto(
    int Id,
    string FirstName,
    string LastName,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? ActiveReservationsCount);

/// <summary>
/// Represents a request to create a new user
/// </summary>
/// <param name="FirstName">The first name of the user</param>
/// <param name="LastName">The last name of the user</param>
public sealed record UserRequest(
    string FirstName,
    string LastName);
