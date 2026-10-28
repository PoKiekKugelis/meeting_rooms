using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;

namespace MeetingRooms.Api.Controllers;

/// <summary>
/// Controller for managing users in the Meeting Rooms application
/// </summary>
/// <param name="service">The user service instance</param>
[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService service) : ControllerBase
{
  /// <summary>
  /// Retrieves a list of all users
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of users</returns>
  [HttpGet]
  public Task<List<UserDto>> GetAll(CancellationToken ct) => service.GetAllAsync(ct);

  /// <summary>
  /// Retrieves a list of all users with their active reservations count
  /// </summary>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of users with views</returns>
  [HttpGet("views")]
  public Task<List<UserViewDto>> GetAllViews(CancellationToken ct) => service.GetAllViewAsync(ct);

  /// <summary>
  /// Retrieves a user by their ID
  /// </summary>
  /// <param name="id">The user ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The user if found, otherwise an error message</returns>
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  /// <summary>
  /// Creates a new user
  /// </summary>
  /// <param name="request">The user creation request</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created user if successful, otherwise an error message</returns>
  [HttpPost]
  public async Task<IActionResult> Create(UserRequest request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  /// <summary>
  /// Deletes a user by their ID
  /// </summary>
  /// <param name="id">The user ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>No content if successful, otherwise an error message</returns>
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await service.DeleteAsync(id, ct);
    return result.IsSuccess ? NoContent() : result.ToProblem();
  }
}
