using FluentValidation;
using MeetingRooms.Core.Common;
using MeetingRooms.Core.Configuration;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Domain.Entities;
using Microsoft.Extensions.Options;

namespace MeetingRooms.Core.Validators;

public sealed class ReservationRequestValidator : AbstractValidator<ReservationRequest>
{
  public ReservationRequestValidator(IOptions<ReservationBookingOptions> options, TimeProvider clock)
  {
    var limits = options.Value;
    var maxDuration = TimeSpan.FromDays(limits.MaximumDurationDays);
    var now = clock.GetUtcNow().UtcDateTime;

    RuleFor(request => request.RoomId)
        .GreaterThan(0)
        .WithErrorCode(BookingErrorCodes.RoomIdInvalid)
        .WithMessage("Select a valid meeting room.");

    RuleFor(request => request.UserId)
        .GreaterThan(0)
        .WithErrorCode(BookingErrorCodes.OrganizerIdInvalid)
        .WithMessage("Select a valid organizer.");

    RuleFor(request => request.Title)
        .Must(title => !string.IsNullOrWhiteSpace(title))
        .WithErrorCode(BookingErrorCodes.TitleRequired)
        .WithMessage("Meeting title is required.")
        .MaximumLength(Reservation.MaxTitleLength)
        .WithErrorCode(BookingErrorCodes.TitleTooLong)
        .WithMessage($"Meeting title cannot exceed {Reservation.MaxTitleLength} characters.");

    RuleFor(request => request.AttendeeCount)
        .GreaterThan(0)
        .WithErrorCode(BookingErrorCodes.AttendeeCountInvalid)
        .WithMessage("Attendee count must be at least one.");

    // R4 (ordering half): end must be after start.
    RuleFor(request => request.EndDate)
        .GreaterThan(request => request.StartDate)
        .WithErrorCode(BookingErrorCodes.InvalidTimeRange)
        .WithMessage("Meeting end time must be after its start time.");

    // R1: exactly MaximumDurationDays is allowed.
    RuleFor(request => request.EndDate)
        .Must((request, endDate) => endDate - request.StartDate <= maxDuration)
        .When(request => request.EndDate > request.StartDate)
        .WithErrorCode(BookingErrorCodes.MaxDuration)
        .WithMessage($"A reservation cannot last longer than {limits.MaximumDurationDays} days.");

    // R4: the start must be strictly in the future.
    RuleFor(request => request.StartDate)
        .Must(startDate => startDate > now)
        .WithErrorCode(BookingErrorCodes.StartNotFuture)
        .WithMessage("Reservation must start in the future.");

    // R6: the start cannot be beyond the booking horizon.
    RuleFor(request => request.StartDate)
        .Must(startDate => startDate <= now.AddDays(limits.MaximumBookingHorizonDays))
        .WithErrorCode(BookingErrorCodes.BookingHorizon)
        .WithMessage($"Reservation cannot start more than {limits.MaximumBookingHorizonDays} days in the future.");
  }
}
