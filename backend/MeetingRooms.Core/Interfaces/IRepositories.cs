using MeetingRooms.Domain.Entities;
using MeetingRooms.Core.DTOs;

namespace MeetingRooms.Core.Interfaces;

public interface IRoomRepository
{
  Task<List<Room>> GetAllAsync(CancellationToken ct);
  Task<List<Room>> GetFilteredAsync(RoomFilterDto filter, CancellationToken ct);
  Task<Room?> GetByIdAsync(int id, CancellationToken ct);
  Task<Room> AddAsync(Room room, CancellationToken ct);
  Task<Room> UpdateAsync(Room room, CancellationToken ct);
  Task DeleteAsync(Room room, CancellationToken ct);
}

public interface IReservationRepository
{
  Task<List<Reservation>> GetAllAsync(CancellationToken ct);
  Task<List<Reservation>> GetFilteredAsync(ReservationFilterDto filter, CancellationToken ct);
  Task<Reservation?> GetByIdAsync(int id, CancellationToken ct);
  Task<Reservation> AddAsync(Reservation reservation, CancellationToken ct);
  Task<Reservation> UpdateAsync(Reservation reservation, CancellationToken ct);
  Task DeleteAsync(Reservation reservation, CancellationToken ct);
}

public interface IUserRepository
{
  Task<List<User>> GetAllAsync(CancellationToken ct);
  Task<User?> GetByIdAsync(int id, CancellationToken ct);
  Task<User> AddAsync(User user, CancellationToken ct);
  Task<User> UpdateAsync(User user, CancellationToken ct);
  Task DeleteAsync(User user, CancellationToken ct);
}
