using MeetingRooms.Domain.Entities;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a repository that manages room entities
/// </summary>
public interface IRoomRepository
{
  /// <summary>
  /// Gets all rooms from the repository
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<List<Room>> GetAllAsync(CancellationToken ct);
  /// <summary>
  /// Gets a filtered list of rooms based on the provided query parameters
  /// </summary>
  /// <param name="query">The query parameters</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<List<Room>> GetFilteredAsync(RoomListQuery query, CancellationToken ct);
  /// <summary>
  /// Gets a room by its ID
  /// </summary>
  /// <param name="id">The room ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<Room?> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Adds a new room to the repository
  /// </summary>
  /// <param name="room">The room to add</param>
  void Add(Room room);
  /// <summary>
  /// Deletes a room from the repository
  /// </summary>
  /// <param name="room">The room to delete</param>
  void Delete(Room room);
}

/// <summary>
/// Defines the contract for a repository that manages reservation entities
/// </summary>
public interface IReservationRepository
{
  /// <summary>
  /// Gets all reservations from the repository
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<List<Reservation>> GetAllAsync(CancellationToken ct);
  /// <summary>
  /// Gets a filtered list of reservations based on the provided query parameters
  /// </summary>
  /// <param name="query">The query parameters</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<List<Reservation>> GetFilteredAsync(ReservationListQuery query, CancellationToken ct);
  /// <summary>
  /// Gets a reservation by its ID
  /// </summary>
  /// <param name="id">The reservation ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<Reservation?> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Adds a new reservation to the repository
  /// </summary>
  /// <param name="now">The current date and time</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<List<Reservation>> GetExpiredBookedAsync(DateTime now, CancellationToken ct);
  /// <summary>
  /// Adds a new reservation to the repository
  /// </summary>
  /// <param name="roomId">The ID of the room</param>
  /// <param name="startDate">The start date and time</param>
  /// <param name="endDate">The end date and time</param>
  /// <param name="excludedReservationId">The ID of the reservation to exclude</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<bool> HasRoomOverlapAsync(int roomId, DateTime startDate, DateTime endDate, int? excludedReservationId, CancellationToken ct);
  /// <summary>
  /// Checks if a user has overlapping reservations
  /// </summary>
  /// <param name="userId">The ID of the user</param>
  /// <param name="startDate">The start date and time</param>
  /// <param name="endDate">The end date and time</param>
  /// <param name="excludedReservationId">The ID of the reservation to exclude</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<bool> HasOrganizerOverlapAsync(int userId, DateTime startDate, DateTime endDate, int? excludedReservationId, CancellationToken ct);
  /// <summary>
  /// Adds a new reservation to the repository
  /// </summary>
  /// <param name="reservation">The reservation to add</param>
  void Add(Reservation reservation);
  /// <summary>
  /// Updates a reservation in the repository
  /// </summary>
  /// <param name="reservation">The reservation to update</param>
  void Update(Reservation reservation);
  /// <summary>
  /// Deletes a reservation from the repository
  /// </summary>
  /// <param name="reservation">The reservation to delete</param>
  void Delete(Reservation reservation);
}

/// <summary>
/// Defines the contract for a repository that manages user entities
/// </summary>
public interface IUserRepository
{
  /// <summary>
  /// Gets a user by its ID
  /// </summary>
  /// <param name="id">The user ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>A task representing the asynchronous operation</returns>
  Task<User?> GetByIdAsync(int id, CancellationToken ct);
  /// <summary>
  /// Adds a new user to the repository
  /// </summary>
  /// <param name="user">The user to add</param>
  void Add(User user);
  /// <summary>
  /// Deletes a user from the repository
  /// </summary>
  /// <param name="user">The user to delete</param>
  void Delete(User user);
}
