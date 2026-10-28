using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Core.Common;

namespace MeetingRooms.Api.Extensions;

/// <summary>
/// Extension methods for converting Result objects to IActionResult
/// </summary>
public static class ResultExtensions
{
  /// <summary>
  /// Converts a Result object to an ObjectResult with appropriate HTTP status code and error message
  /// </summary>
  /// <typeparam name="T">The type of the result value</typeparam>
  /// <param name="result">The result object to convert</param>
  /// <returns>The ObjectResult representing the problem</returns>
  public static ObjectResult ToProblem<T>(this Result<T> result)
  {
    var code = result.ErrorCode ?? "INTERNAL_ERROR";
    var status = code switch
    {
      BookingErrorCodes.RoomOverlap or BookingErrorCodes.OrganizerOverlap
          or BookingErrorCodes.ReservationNotEditable
          or BookingErrorCodes.ReservationNotCompletable
          => StatusCodes.Status409Conflict,
      BookingErrorCodes.RoomNotFound
          or BookingErrorCodes.UserNotFound
          or BookingErrorCodes.ReservationNotFound
          => StatusCodes.Status404NotFound,
      _ => StatusCodes.Status400BadRequest,
    };

    return new ObjectResult(new { code, message = result.Error ?? "The request could not be completed", })
    {
      StatusCode = status,
    };
  }
}
