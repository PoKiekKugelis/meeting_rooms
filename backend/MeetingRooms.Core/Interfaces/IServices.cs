using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Core.Interfaces;

public interface IRoomService
{
  Task<List<RoomDto>> GetAllAsync(RoomListQuery query, CancellationToken ct);
  Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<RoomDto>> CreateAsync(CreateRoomDto request, CancellationToken ct);
  Task<bool> DeleteAsync(int id, CancellationToken ct);
}
public interface IReservationService
{
  Task<List<ReservationDto>> GetAllAsync(ReservationListQuery query, CancellationToken ct);
  Task<Result<ReservationDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<ReservationDto>> CreateAsync(ReservationRequest request, CancellationToken ct);
  Task<Result<ReservationDto>> UpdateAsync(int id, ReservationRequest request, CancellationToken ct);
  Task<Result<bool>> CancelAsync(int id, CancellationToken ct);
}
public interface IUserService
{
  Task<List<UserDto>> GetAllAsync(CancellationToken ct);
  Task<List<UserViewDto>> GetAllViewAsync(CancellationToken ct);
  Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct);
  Task<Result<UserDto>> CreateAsync(CreateUserDto request, CancellationToken ct);
  Task<Result<bool>> DeleteAsync(int id, CancellationToken ct);
}
