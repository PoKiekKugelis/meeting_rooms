using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingRooms.Infrastructure.Queries;

public sealed class UserQueries(AppDbContext db, TimeProvider clock) : IUserQueries
{
  public async Task<List<UserSummaryDto>> GetUsersAsync(UserListQuery query, CancellationToken ct)
  {
    if (!query.IncludeReservationCount)
    {
      return await db.Users
          .AsNoTracking()
          .OrderBy(user => user.Id)
          .Select(user => new UserSummaryDto(
              user.Id,
              user.FirstName,
              user.LastName,
              ActiveReservationsCount: null))
          .ToListAsync(ct);
    }

    var now = clock.GetUtcNow().UtcDateTime;

    return await db.Users
        .AsNoTracking()
        .OrderBy(user => user.Id)
        .Select(user => new UserSummaryDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Reservations.Count(reservation =>
                reservation.Status == ReservationStatus.Booked && reservation.EndDate > now)))
        .ToListAsync(ct);
  }
}
