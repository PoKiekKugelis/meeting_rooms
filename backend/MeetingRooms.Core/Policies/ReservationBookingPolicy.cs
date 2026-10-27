using MeetingRooms.Core.Common;
using MeetingRooms.Core.DTOs;
using MeetingRooms.Core.Interfaces;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Policies;

public sealed class ReservationBookingPolicy(IReservationRepository reservations) : IReservationBookingPolicy
{
  public async Task<(string Code, string Message)?> CheckAsync(
      ReservationPolicyInput input,
      Room? room,
      User? user,
      int? excludedReservationId,
      CancellationToken ct)
  {
    if (room is null)
    {
      return (BookingErrorCodes.RoomNotFound, $"Room {input.RoomId} was not found.");
    }

    // R7 - inactive rooms cannot be booked.
    if (room.Status != RoomStatus.Active)
    {
      return (BookingErrorCodes.RoomInactive, "Inactive rooms cannot be booked.");
    }

    // R5 - attendees must fit.
    if (input.AttendeeCount > room.Capacity)
    {
      return (
        BookingErrorCodes.CapacityExceeded,
        $"Attendee count cannot exceed the room capacity of {room.Capacity}.");
    }

    if (user is null)
    {
      return (BookingErrorCodes.UserNotFound, $"User {input.UserId} was not found.");
    }

    // R2 - the repository already ignores cancelled reservations.
    if (await reservations.HasRoomOverlapAsync(input.RoomId, input.StartDate, input.EndDate, excludedReservationId, ct))
    {
      return (
          BookingErrorCodes.RoomOverlap,
          "The room already has a booking during this time.");
    }

    // R3 - the organizer cannot be double-booked, even in another room.
    if (await reservations.HasOrganizerOverlapAsync(input.UserId, input.StartDate, input.EndDate, excludedReservationId, ct))
    {
      return (
          BookingErrorCodes.OrganizerOverlap,
          "The organizer already has a meeting during this time.");
    }

    return null;
  }
}
