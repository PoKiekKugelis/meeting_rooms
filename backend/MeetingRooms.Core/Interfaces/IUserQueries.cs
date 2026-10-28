using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a service that provides user-related queries
/// </summary>
public interface IUserQueries
{
  /// <summary>
  /// Gets a list of users with their active reservations count
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of users with reservation counts</returns>
  Task<IReadOnlyList<UserListItem>> GetUsersAsync(CancellationToken ct);
  /// <summary>
  /// Gets a user by their ID
  /// </summary>
  /// <param name="id">The ID of the user to get</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The requested user</returns>
  Task<UserListItem?> GetUserAsync(int id, CancellationToken ct);
}
