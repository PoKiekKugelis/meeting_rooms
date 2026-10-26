using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;
using MeetingRooms.Core.DTOs;

namespace MeetingRooms.Infrastructure.Repositories;


public sealed class RoomRepository(AppDbContext db) : IRoomRepository
{
  public async Task<List<Room>> GetAllAsync(CancellationToken ct)
  {
    return await db.Rooms.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
  }

  public async Task<List<Room>> GetFilteredAsync(RoomFilterDto? filter, CancellationToken ct)
  {
    IQueryable<Room> query = db.Rooms
        .AsNoTracking()
        .Include(x => x.Equipment)
        .Where(x => x.Status == RoomStatus.Active);

    if (filter != null)
    {
      if (filter.MinCapacity.HasValue)
      {
        query = query.Where(x => x.Capacity >= filter.MinCapacity.Value);
      }

      if (!string.IsNullOrWhiteSpace(filter.Floor))
      {
        query = query.Where(x => x.Location == filter.Floor);
      }

      if (filter.Equipment is not null && filter.Equipment.Count > 0)
      {
        foreach (var equipmentName in filter.Equipment)
        {
          query = query.Where(x => x.Equipment.Any(x => x.Equipment.Name == equipmentName));
        }
      }
    }

    return await query
        .OrderBy(x => x.Name)
        .ToListAsync(ct);
  }

  public async Task<Room?> GetByIdAsync(int id, CancellationToken ct)
  {
    return await db.Rooms.FirstOrDefaultAsync(x => x.Id == id, ct);
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
