using MapsterMapper;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Services;

public sealed class RoomService(
    IMapper mapper,
    IRoomRepository roomRepository,
    IUnitOfWork unitOfWork) : IRoomService
{
  public async Task<List<RoomDto>> GetAllAsync(RoomListQuery query, CancellationToken ct)
  {
    return mapper.Map<List<RoomDto>>(await roomRepository.GetFilteredAsync(query, ct));
  }

  public async Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken ct)
  {
    var room = await roomRepository.GetByIdAsync(id, ct);
    return room is null
        ? Result<RoomDto>.Failure($"Room {id} was not found.", "NOT_FOUND")
        : Result<RoomDto>.Success(mapper.Map<RoomDto>(room));
  }

  public async Task<Result<RoomDto>> CreateAsync(RoomRequest request, CancellationToken ct)
  {
    Room room;
    try
    {
      room = new Room(request.Name, request.Capacity, request.Location);
    }
    catch (ArgumentException exception)
    {
      return Result<RoomDto>.Failure(exception.Message, "VALIDATION_ERROR");
    }

    roomRepository.Add(room);
    await unitOfWork.SaveChangesAsync(ct);
    return Result<RoomDto>.Success(mapper.Map<RoomDto>(room));
  }

  public async Task<bool> DeleteAsync(int id, CancellationToken ct)
  {
    var room = await roomRepository.GetByIdAsync(id, ct);
    if (room is null)
    {
      return false;
    }

    roomRepository.Delete(room);
    await unitOfWork.SaveChangesAsync(ct);
    return true;
  }
}
