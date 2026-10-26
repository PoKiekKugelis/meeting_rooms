using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Infrastructure.Repositories;


public sealed class ReservationRepository(AppDbContext db) : IReservationRepository
{
  public async Task<List<Reservation>> GetAllAsync(CancellationToken ct)
  {
    return await db.Reservations.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
  }

  public async Task<List<Reservation>> GetFilteredAsync(ReservationListQuery criteria, CancellationToken ct)
  {
    IQueryable<Reservation> reservations = db.Reservations
        .AsNoTracking()
        .Include(x => x.User)
        .Include(x => x.Room);

    if (criteria.RoomId.HasValue)
    {
      reservations = reservations.Where(x => x.RoomId == criteria.RoomId.Value);
    }

    if (criteria.UserId.HasValue)
    {
      reservations = reservations.Where(x => x.UserId == criteria.UserId.Value);
    }

    if (criteria.Status.HasValue)
    {
      reservations = reservations.Where(x => x.Status == criteria.Status.Value);
    }

    if (criteria.From.HasValue)
    {
      reservations = reservations.Where(x => x.EndDate > criteria.From.Value.UtcDateTime);
    }

    if (criteria.To.HasValue)
    {
      reservations = reservations.Where(x => x.StartDate < criteria.To.Value.UtcDateTime);
    }

    return await reservations
        .OrderBy(r => r.StartDate)
        .ToListAsync(ct);
  }

  public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct)
  {
    return await db.Reservations
        .Include(reservation => reservation.User)
        .Include(reservation => reservation.Room)
        .FirstOrDefaultAsync(reservation => reservation.Id == id, ct);
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
