using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a service that manages meeting rooms
/// </summary>
public interface IRoomService
{
  /// <summary>
  /// Gets a list of all rooms based on the provided query parameters
  /// </summary>
  /// <param name="query">The query parameters</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of rooms</returns>
  Task<List<RoomDto>> GetAllAsync(RoomListQuery query, CancellationToken ct);
  /// <summary>
  /// Gets a room by its ID
  /// </summary>
  /// <param name="id">The ID of the room to get</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The room if found, otherwise null</returns>
  Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Creates a new room based on the provided request
  /// </summary>
  /// <param name="request">The request for creating a new room</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created room</returns>
  Task<Result<RoomDto>> CreateAsync(RoomRequest request, CancellationToken ct);
  /// <summary>
  /// Deletes a room by its ID
  /// </summary>
  /// <param name="id">The ID of the room to delete</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A boolean indicating whether the room was deleted</returns>
  Task<bool> DeleteAsync(int id, CancellationToken ct);
}

/// <summary>
/// Defines the contract for a service that manages reservations
/// </summary>
public interface IReservationService
{
  /// <summary>
  /// Gets a list of all reservations based on the provided query parameters
  /// </summary>
  /// <param name="query">The query parameters</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of reservations</returns>
  Task<List<ReservationDto>> GetAllAsync(ReservationListQuery query, CancellationToken ct);
  /// <summary>
  /// Gets a reservation by its ID
  /// </summary>
  /// <param name="id">The ID of the reservation to get</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The reservation if found, otherwise null</returns>
  Task<Result<ReservationDto>> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Creates a new reservation based on the provided request
  /// </summary>
  /// <param name="request">The request for creating the reservation</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created reservation</returns>
  Task<Result<ReservationDto>> CreateAsync(ReservationRequest request, CancellationToken ct);
  /// <summary>
  /// Updates an existing reservation based on the provided request
  /// </summary>
  /// <param name="id">The ID of the reservation to update</param>
  /// <param name="request">The request for updating the reservation</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The updated reservation</returns>
  Task<Result<ReservationDto>> UpdateAsync(int id, ReservationRequest request, CancellationToken ct);
  /// <summary>
  /// Cancels an existing reservation by its ID
  /// </summary>
  /// <param name="id">The ID of the reservation to cancel</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A boolean indicating whether the reservation was canceled</returns>
  Task<Result<bool>> CancelAsync(int id, CancellationToken ct);
  /// <summary>
  /// Marks an existing reservation as completed by its ID
  /// </summary>
  /// <param name="id">The ID of the reservation to complete</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The completed reservation</returns>
  Task<Result<ReservationDto>> CompleteAsync(int id, CancellationToken ct);
  /// <summary>
  /// Marks expired reservations as completed based on the current time
  /// </summary>
  /// <param name="now">The current time</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The number of reservations that were completed</returns>
  Task<int> CompleteExpiredAsync(DateTime now, CancellationToken ct);
}

/// <summary>
/// Defines the contract for a service that manages users
/// </summary>
public interface IUserService
{
  /// <summary>
  /// Gets a filtered list of user summaries
  /// </summary>
  /// <param name="query">The list query options</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The user summaries matching the requested representation</returns>
  Task<List<UserSummaryDto>> GetFilteredAsync(UserListQuery query, CancellationToken ct);
  /// <summary>
  /// Gets a user by their ID
  /// </summary>
  /// <param name="id">The ID of the user to get</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The requested user</returns>
  Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Creates a new user based on the provided request
  /// </summary>
  /// <param name="request">The request containing user information</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created user</returns>
  Task<Result<UserDto>> CreateAsync(UserRequest request, CancellationToken ct);
  /// <summary>
  /// Deletes a user by their ID
  /// </summary>
  /// <param name="id">The ID of the user to delete</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A boolean indicating whether the user was deleted</returns>
  Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
}
