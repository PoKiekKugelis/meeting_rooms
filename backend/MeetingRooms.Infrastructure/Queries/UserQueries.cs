using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Infrastructure.Queries;

public sealed class UserQueries(AppDbContext db) : IUserQueries
{
  public async Task<IReadOnlyList<UserListItem>> GetUsersAsync(CancellationToken ct)
  {
    var now = DateTime.UtcNow;

    return await BuildUsersQuery(now)
        .OrderBy(user => user.LastName)
        .ToListAsync(ct);
  }

  public async Task<UserListItem?> GetUserAsync(int id, CancellationToken ct)
  {
    var now = DateTime.UtcNow;
    return await BuildUsersQuery(now)
        .Where(user => user.Id == id)
        .FirstOrDefaultAsync(ct);
  }

  private IQueryable<UserListItem> BuildUsersQuery(DateTime now)
  {
    return db.Users
        .AsNoTracking()
        .Select(user => new UserListItem(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Reservations.Count(reservation =>
                reservation.Status == ReservationStatus.Booked && reservation.EndDate > now)));
  }
}
