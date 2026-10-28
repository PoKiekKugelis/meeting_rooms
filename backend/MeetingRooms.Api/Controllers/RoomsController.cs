using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Api.Controllers;

/// <summary>
/// Controller for managing rooms in the Meeting Rooms application
/// </summary>
/// <param name="service">The room service instance</param>
[ApiController]
[Route("api/[controller]")]
public sealed class RoomsController(IRoomService service) : ControllerBase
{
  /// <summary>
  /// Retrieves a list of all rooms with optional filtering parameters
  /// </summary>
  /// <param name="minimumCapacity">The minimum capacity of the rooms to retrieve</param>
  /// <param name="floor">The floor of the rooms to retrieve</param>
  /// <param name="equipment">The equipment required in the rooms to retrieve</param>
  /// <param name="includeInactive">Whether to include inactive rooms</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of rooms</returns>
  [HttpGet]
  public Task<List<RoomDto>> GetAll(
      [FromQuery] int? minimumCapacity,
      [FromQuery] string? floor,
      [FromQuery] string[]? equipment,
      [FromQuery] bool includeInactive = false,
      CancellationToken ct = default)
  {
    var query = new RoomListQuery(minimumCapacity, floor, equipment, includeInactive);
    return service.GetAllAsync(query, ct);
  }

  /// <summary>
  /// Retrieves a room by its ID
  /// </summary>
  /// <param name="id">The room ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The room if found, otherwise an error message</returns>
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  /// <summary>
  /// Creates a new room
  /// </summary>
  /// <param name="request">The room creation request</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created room if successful, otherwise an error message</returns>
  [HttpPost]
  public async Task<IActionResult> Create(RoomRequest request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  /// <summary>
  /// Deletes a room by its ID
  /// </summary>
  /// <param name="id">The room ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>No content if successful, otherwise an error message</returns>
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    return await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
  }
}
