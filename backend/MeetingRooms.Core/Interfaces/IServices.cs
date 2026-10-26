using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;

namespace MeetingRooms.Core.Interfaces;

public interface IRoomService
{
  Task<List<RoomDto>> GetAllAsync(RoomFilterDto? filter, CancellationToken ct);
  Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<RoomDto> CreateAsync(CreateRoomDto request, CancellationToken ct);
  Task<bool> DeleteAsync(int id, CancellationToken ct);
}
public interface IReservationService
{
  Task<List<ReservationDto>> GetAllAsync(ReservationFilterDto? filter, CancellationToken ct);
  Task<Result<ReservationDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<ReservationDto>> CreateAsync(CreateReservationDto request, CancellationToken ct);
  Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
}
public interface IUserService
{
  Task<List<UserDto>> GetAllAsync(ReservationFilterDto? filter, CancellationToken ct);
  Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<UserDto>> CreateAsync(CreateUserDto request, CancellationToken ct);
  Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
}
