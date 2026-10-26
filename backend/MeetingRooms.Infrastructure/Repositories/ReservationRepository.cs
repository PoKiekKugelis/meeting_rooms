using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Core.DTOs;

namespace MeetingRooms.Infrastructure.Repositories;


public sealed class ReservationRepository(AppDbContext db) : IReservationRepository
{
  public async Task<List<Reservation>> GetAllAsync(CancellationToken ct)
  {
    return await db.Reservations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
  }

  public async Task<List<Reservation>> GetFilteredAsync(ReservationFilterDto? filter, CancellationToken ct)
  {
    IQueryable<Reservation> query = db.Reservations
        .AsNoTracking()
        .Include(x => x.User)
        .Include(x => x.Room);

    if (filter != null)
    {
      if (filter.RoomId.HasValue)
      {
        query = query.Where(x => x.RoomId == filter.RoomId);
      }

      if (filter.UserId.HasValue)
      {
        query = query.Where(x => x.UserId == filter.UserId);
      }

      if (!string.IsNullOrWhiteSpace(filter.Status) && Enum.TryParse<ReservationStatus>(filter.Status, true, out var status))
      {
        query = query.Where(x => x.Status == status);
      }

      if (filter.From.HasValue)
      {
        query = query.Where(x => x.EndDate > filter.From.Value);
      }

      if (filter.To.HasValue)
      {
        query = query.Where(x => x.StartDate < filter.To.Value);
      }
    }

    return await query
        .OrderBy(r => r.StartDate)
        .ToListAsync(ct);
  }

  public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct)
  {
    return await db.Reservations.FirstOrDefaultAsync(x => x.Id == id, ct);
  }

  public void Add(Reservation reservation)
  {
    db.Reservations.Add(reservation);
  }

  public void Update(Reservation reservation)
  {
    db.Reservations.Update(reservation);
  }

  public void Delete(Reservation reservation)
  {
    db.Reservations.Remove(reservation);
  }
}
