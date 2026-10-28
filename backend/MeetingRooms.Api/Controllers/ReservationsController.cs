using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Api.Controllers;

/// <summary>
/// Controller for managing reservations in the Meeting Rooms application
/// </summary>
/// <param name="service">The reservation service instance</param>
[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController(IReservationService service) : ControllerBase
{
  /// <summary>
  /// Retrieves a list of all reservations with optional filtering parameters
  /// </summary>
  /// <param name="roomId">The room ID to filter by</param>
  /// <param name="userId">The user ID to filter by</param>
  /// <param name="status">The status to filter by</param>
  /// <param name="from">The start date to filter by</param>
  /// <param name="to">The end date to filter by</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The list of reservations</returns>
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

  /// <summary>
  /// Retrieves a reservation by its ID
  /// </summary>
  /// <param name="id">The reservation ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The reservation if found, otherwise an error message</returns>
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  /// <summary>
  /// Creates a new reservation
  /// </summary>
  /// <param name="request">The reservation creation request</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The created reservation if successful, otherwise an error message</returns>
  [HttpPost]
  public async Task<IActionResult> Create(ReservationRequest request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  /// <summary>
  /// Updates an existing reservation
  /// </summary>
  /// <param name="id">The reservation ID</param>
  /// <param name="request">The reservation update request</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The updated reservation if successful, otherwise an error message</returns>
  [HttpPut("{id:int}")]
  public async Task<IActionResult> Update(int id, ReservationRequest request, CancellationToken ct)
  {
    var result = await service.UpdateAsync(id, request, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  /// <summary>
  /// Cancels an existing reservation
  /// </summary>
  /// <param name="id">The reservation ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>No content if successful, otherwise an error message</returns>
  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Cancel(int id, CancellationToken ct)
  {
    var result = await service.CancelAsync(id, ct);
    return result.IsSuccess ? NoContent() : result.ToProblem();
  }

  /// <summary>
  /// Marks a reservation as completed
  /// </summary>
  /// <param name="id">The reservation ID</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The completed reservation if successful, otherwise an error message</returns>
  [HttpPost("{id:int}/complete")]
  public async Task<IActionResult> Complete(int id, CancellationToken ct)
  {
    var result = await service.CompleteAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }
}
