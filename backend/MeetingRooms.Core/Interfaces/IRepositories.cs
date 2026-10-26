using MeetingRooms.Domain.Entities;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

public interface IRoomRepository
{
  Task<List<Room>> GetAllAsync(CancellationToken ct);
  Task<List<Room>> GetFilteredAsync(RoomListQuery query, CancellationToken ct);
  Task<Room?> GetByIdAsync(int id, CancellationToken ct);
  void Add(Room room);
  void Delete(Room room);
}

public interface IReservationRepository
{
  Task<List<Reservation>> GetAllAsync(CancellationToken ct);
  Task<List<Reservation>> GetFilteredAsync(ReservationListQuery query, CancellationToken ct);
  Task<Reservation?> GetByIdAsync(int id, CancellationToken ct);
  void Add(Reservation reservation);
  void Update(Reservation reservation);
  void Delete(Reservation reservation);
}

public interface IUserRepository
{
  Task<List<User>> GetAllAsync(CancellationToken ct);
  Task<User?> GetByIdAsync(int id, CancellationToken ct);
  void Add(User user);
  void Delete(User user);
}
