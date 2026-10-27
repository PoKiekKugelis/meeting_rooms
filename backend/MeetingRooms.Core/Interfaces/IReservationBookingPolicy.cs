using MeetingRooms.Core.DTOs;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Interfaces;

public interface IReservationBookingPolicy
{
  Task<(string Code, string Message)?> CheckAsync(
      ReservationPolicyInput input,
      Room? room,
      User? user,
      int? excludedReservationId,
      CancellationToken ct);
}

