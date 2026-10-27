using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;

namespace MeetingRooms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class RoomsController(IRoomService service) : ControllerBase
{
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

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreateRoomDto request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    return await service.DeleteAsync(id, ct) ? NoContent() : NotFound();
  }
}
