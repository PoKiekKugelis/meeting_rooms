using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController(IReservationService service) : ControllerBase
{
  [HttpGet]
  public Task<List<ReservationDto>> GetAll(
      [FromQuery] int? roomId,
      [FromQuery] int? userId,
      [FromQuery] ReservationStatus? status,
      [FromQuery] DateTime? from,
      [FromQuery] DateTime? to,
      CancellationToken ct)
  {
    return service.GetAllAsync(new ReservationListQuery(roomId, userId, status, from, to), ct);
  }

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  [HttpPost]
  public async Task<IActionResult> Create(ReservationRequest request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, ReservationRequest request, CancellationToken ct)
  {
    var result = await service.UpdateAsync(id, request, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Cancel(int id, CancellationToken ct)
  {
    var result = await service.CancelAsync(id, ct);
    return result.IsSuccess ? NoContent() : result.ToProblem();
  }

  [HttpPost("{id:int}/complete")]
  public async Task<IActionResult> Complete(int id, CancellationToken ct)
  {
    var result = await service.CompleteAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }
}
