using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Infrastructure.Repositories;


public sealed class RoomRepository(AppDbContext db) : IRoomRepository
{
  public async Task<List<Room>> GetAllAsync(CancellationToken ct)
  {
    return await db.Rooms.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
  }

  public async Task<List<Room>> GetFilteredAsync(RoomListQuery criteria, CancellationToken ct)
  {
    IQueryable<Room> rooms = db.Rooms
        .AsNoTracking()
        .Include(x => x.Equipment)
        .ThenInclude(link => link.Equipment);

    if (!criteria.IncludeInactive)
    {
      rooms = rooms.Where(x => x.Status == RoomStatus.Active);
    }

    if (criteria.MinimumCapacity.HasValue)
    {
      rooms = rooms.Where(x => x.Capacity >= criteria.MinimumCapacity.Value);
    }

    if (!string.IsNullOrWhiteSpace(criteria.Floor))
    {
      rooms = rooms.Where(x => x.Location == criteria.Floor);
    }

    if (criteria.Equipment is { Count: > 0 })
    {
      foreach (var equipmentName in criteria.Equipment)
      {
        rooms = rooms.Where(x => x.Equipment.Any(e => e.Equipment.Name == equipmentName));
      }
    }

    return await rooms
        .OrderBy(x => x.Name)
        .ToListAsync(ct);
  }

  public async Task<Room?> GetByIdAsync(int id, CancellationToken ct)
  {
    return await db.Rooms
        .Include(x => x.Equipment)
        .ThenInclude(link => link.Equipment)
        .FirstOrDefaultAsync(x => x.Id == id, ct);
  }

  public void Add(Room room)
  {
    db.Rooms.Add(room);
  }

  public void Delete(Room room)
  {
    db.Rooms.Remove(room);
  }
}
