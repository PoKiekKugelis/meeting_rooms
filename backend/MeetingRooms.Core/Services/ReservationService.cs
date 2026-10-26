using MapsterMapper;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Services;

public sealed class ReservationService(
    IMapper mapper,
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IReservationService
{
  public async Task<List<ReservationDto>> GetAllAsync(ReservationListQuery query, CancellationToken ct)
  {
    return mapper.Map<List<ReservationDto>>(await reservationRepository.GetFilteredAsync(query, ct));
  }

  public async Task<Result<ReservationDto>> GetByIdAsync(int id, CancellationToken ct)
  {
    var reservation = await reservationRepository.GetByIdAsync(id, ct);
    return reservation is null
        ? Result<ReservationDto>.Failure($"Reservation {id} was not found.", "NOT_FOUND")
        : Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
  }

  public async Task<Result<ReservationDto>> CreateAsync(CreateReservationDto request, CancellationToken ct)
  {
    var room = await roomRepository.GetByIdAsync(request.RoomId, ct);
    if (room is null)
    {
      return Result<ReservationDto>.Failure($"Room {request.RoomId} was not found.", "NOT_FOUND");
    }

    var organizer = await userRepository.GetByIdAsync(request.UserId, ct);
    if (organizer is null)
    {
      return Result<ReservationDto>.Failure($"User {request.UserId} was not found.", "NOT_FOUND");
    }

    Reservation reservation;
    try
    {
      reservation = new Reservation(
          request.Title,
          request.StartDate,
          request.EndDate,
          request.AttendeeCount,
          room,
          organizer);
    }
    catch (ArgumentException exception)
    {
      return Result<ReservationDto>.Failure(exception.Message, "VALIDATION_ERROR");
    }

    reservationRepository.Add(reservation);
    await unitOfWork.SaveChangesAsync(ct);
    return Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
  }

  public async Task<Result<bool>> CancelAsync(int id, CancellationToken ct)
  {
    var reservation = await reservationRepository.GetByIdAsync(id, ct);
    if (reservation is null)
    {
      return Result<bool>.Failure($"Reservation {id} was not found.", "NOT_FOUND");
    }

    try
    {
      reservation.Cancel();
    }
    catch (InvalidOperationException exception)
    {
      return Result<bool>.Failure(exception.Message, "BUSINESS_RULE_VIOLATION");
    }

    reservationRepository.Update(reservation);
    await unitOfWork.SaveChangesAsync(ct);
    return Result<bool>.Success(true);
  }
}
