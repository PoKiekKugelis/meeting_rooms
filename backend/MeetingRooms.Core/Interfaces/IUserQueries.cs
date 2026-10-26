using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

public interface IUserQueries
{
  Task<IReadOnlyList<UserListItem>> GetUsersAsync(CancellationToken ct);
  Task<UserListItem?> GetUserAsync(int id, CancellationToken ct);
}
