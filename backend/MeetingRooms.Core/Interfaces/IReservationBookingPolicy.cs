using MeetingRooms.Core.DTOs;
using MeetingRooms.Domain.Entities;

namespace MeetingRooms.Core.Interfaces;

/// <summary>
/// Defines the contract for a policy that checks reservation booking rules
/// </summary>
public interface IReservationBookingPolicy
{
  /// <summary>
  /// Checks the reservation booking policy
  /// </summary>
  /// <param name="input">The input for the reservation policy check</param>
  /// <param name="room">The room for which to check the policy</param>
  /// <param name="user">The user for which to check the policy</param>
  /// <param name="excludedReservationId">The ID of the reservation to exclude from the check</param>
  /// <param name="ct">The cancellation token</param>
  /// <returns>The result of the policy check</returns>
  Task<(string Code, string Message)?> CheckAsync(
      ReservationPolicyInput input,
      Room? room,
      User? user,
      int? excludedReservationId,
      CancellationToken ct);
}

