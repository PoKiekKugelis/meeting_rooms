using MapsterMapper;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Services;

public sealed class UserService(
    IMapper mapper,
    IUserQueries userQueries,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IUserService
{
  public async Task<List<UserDto>> GetAllAsync(CancellationToken ct)
  {
    return mapper.Map<List<UserDto>>(await userRepository.GetAllAsync(ct));
  }

  public async Task<List<UserViewDto>> GetAllViewAsync(CancellationToken ct)
  {
    return mapper.Map<List<UserViewDto>>(await userQueries.GetUsersAsync(ct));
  }

  public async Task<Result<UserDto>> GetByIdAsync(int id, CancellationToken ct)
  {
    var user = await userQueries.GetUserAsync(id, ct);
    return user is null ? Result<UserDto>.Failure($"User {id} was not found.", "NOT_FOUND")
      : Result<UserDto>.Success(mapper.Map<UserDto>(user));
  }

  public async Task<Result<UserDto>> CreateAsync(UserRequest request, CancellationToken ct)
  {
    User user;
    try
    {
      user = new User(request.FirstName, request.LastName);
    }
    catch (ArgumentException exception)
    {
      return Result<UserDto>.Failure(exception.Message, "VALIDATION_ERROR");
    }

    userRepository.Add(user);
    await unitOfWork.SaveChangesAsync(ct);

    return Result<UserDto>.Success(mapper.Map<UserDto>(user));
  }

  public async Task<Result<bool>> DeleteAsync(int id, CancellationToken ct)
  {
    var user = await userRepository.GetByIdAsync(id, ct);
    if (user is null)
    {
      return Result<bool>.Failure($"User {id} was not found.", "NOT_FOUND");
    }

    userRepository.Delete(user);
    await unitOfWork.SaveChangesAsync(ct);
    return Result<bool>.Success(true);
  }
}
