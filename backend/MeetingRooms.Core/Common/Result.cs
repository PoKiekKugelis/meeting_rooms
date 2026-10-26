namespace MeetingRooms.Core.Common;

public sealed record Result<T>(bool IsSuccess, T? Value, string? Error, string? ErrorCode)
{
  public static Result<T> Success(T value) => new(true, value, null, null);
  public static Result<T> Failure(string error, string code = "BUSINESS_ERROR") => new(false, default, error, code);
}
