using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Api.Extensions;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;

namespace MeetingRooms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController(IUserService service) : ControllerBase
{
  [HttpGet]
  public Task<List<UserDto>> GetAll(CancellationToken ct) => service.GetAllAsync(ct);

  [HttpGet("views")]
  public Task<List<UserViewDto>> GetAllViews(CancellationToken ct) => service.GetAllViewAsync(ct);

  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var result = await service.GetByIdAsync(id, ct);
    return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
  }

  [HttpPost]
  public async Task<IActionResult> Create(CreateUserDto request, CancellationToken ct)
  {
    var result = await service.CreateAsync(request, ct);
    return result.IsSuccess
        ? CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value)
        : result.ToProblem();
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id, CancellationToken ct)
  {
    var result = await service.DeleteAsync(id, ct);
    return result.IsSuccess ? NoContent() : result.ToProblem();
  }
}
