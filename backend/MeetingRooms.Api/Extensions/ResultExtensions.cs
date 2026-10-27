using Microsoft.AspNetCore.Mvc;
using MeetingRooms.Core.Common;

namespace MeetingRooms.Api.Extensions;

public static class ResultExtensions
{
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
