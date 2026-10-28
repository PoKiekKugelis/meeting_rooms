using MeetingRooms.Core.Queries;
using MeetingRooms.Core.DTOs;

namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a service that provides user-related queries
/// </summary>
public interface IUserQueries
{
  /// <summary>
  /// Gets user summaries, optionally including the count of non-ended booked reservations
  /// </summary>
  /// <param name="query">The list query options</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The user summaries matching the requested representation</returns>
  Task<List<UserSummaryDto>> GetUsersAsync(UserListQuery query, CancellationToken ct);
}
