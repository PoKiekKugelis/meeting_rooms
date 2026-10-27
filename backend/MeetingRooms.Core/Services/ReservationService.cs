using FluentValidation;
using MapsterMapper;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Core.Queries;
using MeetingRooms.Domain.Common;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Services;

public sealed class ReservationService(
    IMapper mapper,
    IValidator<ReservationRequest> validator,
    IReservationBookingPolicy bookingPolicy,
    IReservationRepository reservationRepository,
    IRoomRepository roomRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    TimeProvider clock) : IReservationService
{
  public async Task<List<ReservationDto>> GetAllAsync(ReservationListQuery query, CancellationToken ct)
  {
    query = query with
    {
      From = query.From.HasValue ? DateTimeUtc.Normalize(query.From.Value) : null,
      To = query.To.HasValue ? DateTimeUtc.Normalize(query.To.Value) : null,
    };

    return mapper.Map<List<ReservationDto>>(await reservationRepository.GetFilteredAsync(query, ct));
  }

  public async Task<Result<ReservationDto>> GetByIdAsync(int id, CancellationToken ct)
  {
    var reservation = await reservationRepository.GetByIdAsync(id, ct);
    return reservation is null
        ? Result<ReservationDto>.Failure($"Reservation {id} was not found.", BookingErrorCodes.ReservationNotFound)
        : Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
  }

  public async Task<Result<ReservationDto>> CreateAsync(ReservationRequest request, CancellationToken ct)
  {
    request = request with
    {
      StartDate = DateTimeUtc.Normalize(request.StartDate),
      EndDate = DateTimeUtc.Normalize(request.EndDate),
    };

    var validation = await validator.ValidateAsync(request, ct);
    if (!validation.IsValid)
    {
      return Result<ReservationDto>.Failure(validation.Errors[0].ErrorMessage, validation.Errors[0].ErrorCode);
    }

    return await unitOfWork.ExecuteInTransactionAsync(async token =>
    {
      var room = await roomRepository.GetByIdAsync(request.RoomId, token);
      var user = await userRepository.GetByIdAsync(request.UserId, token);

      var draft = new ReservationPolicyInput(
          request.RoomId,
          request.UserId,
          request.AttendeeCount,
          request.StartDate,
          request.EndDate);

      var violation = await bookingPolicy.CheckAsync(draft, room, user, excludedReservationId: null, token);
      if (violation is not null)
      {
        return Result<ReservationDto>.Failure(violation.Value.Message, violation.Value.Code);
      }

      try
      {
        var reservation = new Reservation(
            request.Title,
            request.StartDate,
            request.EndDate,
            request.AttendeeCount,
            room!,
            user!);

        reservationRepository.Add(reservation);
        await unitOfWork.SaveChangesAsync(token);

        return Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
      }
      catch (ArgumentException exception)
      {
        return Result<ReservationDto>.Failure(
            exception.Message,
            BookingErrorCodes.ValidationError);
      }
    }, ct);
  }

  public async Task<Result<ReservationDto>> UpdateAsync(int id, ReservationRequest request, CancellationToken ct)
  {
    request = request with
    {
      StartDate = DateTimeUtc.Normalize(request.StartDate),
      EndDate = DateTimeUtc.Normalize(request.EndDate),
    };

    var validation = await validator.ValidateAsync(request, ct);
    if (!validation.IsValid)
    {
      return Result<ReservationDto>.Failure(validation.Errors[0].ErrorMessage, validation.Errors[0].ErrorCode);
    }

    return await unitOfWork.ExecuteInTransactionAsync(async token =>
    {
      var reservation = await reservationRepository.GetByIdAsync(id, token);
      if (reservation is null)
      {
        return Result<ReservationDto>.Failure(
            $"Reservation {id} was not found.",
            BookingErrorCodes.ReservationNotFound);
      }

      if (reservation.Status != ReservationStatus.Booked)
      {
        return Result<ReservationDto>.Failure(
            "Only booked reservations can be edited.",
            BookingErrorCodes.ReservationNotEditable);
      }

      if (clock.GetUtcNow().UtcDateTime >= reservation.EndDate)
      {
        return Result<ReservationDto>.Failure(
            "Reservations that have ended cannot be edited.",
            BookingErrorCodes.ReservationNotEditable);
      }

      var room = await roomRepository.GetByIdAsync(request.RoomId, token);
      var user = await userRepository.GetByIdAsync(request.UserId, token);

      var draft = new ReservationPolicyInput(
          request.RoomId,
          request.UserId,
          request.AttendeeCount,
          request.StartDate,
          request.EndDate);

      var violation = await bookingPolicy.CheckAsync(draft, room, user, excludedReservationId: id, token);
      if (violation is not null)
      {
        return Result<ReservationDto>.Failure(violation.Value.Message, violation.Value.Code);
      }

      try
      {
        reservation.UpdateDetails(
            request.Title,
            request.StartDate,
            request.EndDate,
            request.AttendeeCount,
            room!,
            user!);

        await unitOfWork.SaveChangesAsync(token);

        return Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
      }
      catch (ArgumentException exception)
      {
        return Result<ReservationDto>.Failure(
            exception.Message,
            BookingErrorCodes.ValidationError);
      }
      catch (InvalidOperationException exception)
      {
        return Result<ReservationDto>.Failure(
            exception.Message,
            BookingErrorCodes.BusinessRuleViolation);
      }
    }, ct);
  }

  public async Task<Result<bool>> CancelAsync(int id, CancellationToken ct)
  {
    return await unitOfWork.ExecuteInTransactionAsync(async ct =>
    {
      var reservation = await reservationRepository.GetByIdAsync(id, ct);
      if (reservation is null)
      {
        return Result<bool>.Failure(
            $"Reservation {id} was not found.",
            BookingErrorCodes.ReservationNotFound);
      }

      if (reservation.Status != ReservationStatus.Booked)
      {
        return Result<bool>.Failure(
            "Only booked reservations can be cancelled.",
            BookingErrorCodes.ReservationNotEditable);
      }

      try
      {
        reservation.Cancel(clock.GetUtcNow().UtcDateTime);
      }
      catch (InvalidOperationException exception)
      {
        return Result<bool>.Failure(exception.Message, BookingErrorCodes.ReservationNotEditable);
      }

      await unitOfWork.SaveChangesAsync(ct);

      return Result<bool>.Success(true);
    }, ct);
  }

  public async Task<Result<ReservationDto>> CompleteAsync(int id, CancellationToken ct)
  {
    return await unitOfWork.ExecuteInTransactionAsync(async token =>
    {
      var reservation = await reservationRepository.GetByIdAsync(id, token);
      if (reservation is null)
      {
        return Result<ReservationDto>.Failure(
            $"Reservation {id} was not found.",
            BookingErrorCodes.ReservationNotFound);
      }

      try
      {
        reservation.Complete(clock.GetUtcNow().UtcDateTime);
      }
      catch (InvalidOperationException exception)
      {
        return Result<ReservationDto>.Failure(
            exception.Message,
            BookingErrorCodes.ReservationNotCompletable);
      }

      await unitOfWork.SaveChangesAsync(token);
      return Result<ReservationDto>.Success(mapper.Map<ReservationDto>(reservation));
    }, ct);
  }

  public async Task<int> CompleteExpiredAsync(DateTime now, CancellationToken ct)
  {
    now = DateTimeUtc.Normalize(now);

    return await unitOfWork.ExecuteInTransactionAsync(async token =>
    {
      var expired = await reservationRepository.GetExpiredBookedAsync(now, token);
      if (expired.Count == 0)
      {
        return 0;
      }

      foreach (var reservation in expired)
      {
        reservation.Complete(reservation.EndDate);
      }

      await unitOfWork.SaveChangesAsync(token);
      return expired.Count;
    }, ct);
  }
}
