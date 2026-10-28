namespace MeetingRooms.Core.Common;

/// <summary>
/// Represents the result of an operation, encapsulating success status, value, error message, and error code
/// </summary>
/// <typeparam name="T">The type of the result value</typeparam>
/// <param name="IsSuccess">Indicates whether the operation was successful</param>
/// <param name="Value">The value returned by the operation, if it was successful</param>
/// <param name="Error">The error message, if the operation failed</param>
/// <param name="ErrorCode">The error code, if the operation failed</param>
public sealed record Result<T>(bool IsSuccess, T? Value, string? Error, string? ErrorCode)
{
  public static Result<T> Success(T value) => new(true, value, null, null);
  public static Result<T> Failure(string error, string code = "BUSINESS_ERROR") => new(false, default, error, code);
}
