using Microsoft.EntityFrameworkCore;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;
using MeetingRooms.Infrastructure.Data;

namespace MeetingRooms.Infrastructure.Repositories;


public sealed class UserRepository(AppDbContext db) : IUserRepository
{
  public async Task<List<User>> GetAllAsync(CancellationToken ct)
  {
    return await db.Users.AsNoTracking().OrderBy(x => x.Id).ToListAsync(ct);
  }

  public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
  {
    return await db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);
  }

  public void Add(User user)
  {
    db.Users.Add(user);
  }

  public void Delete(User user)
  {
    db.Users.Remove(user);
  }
}
