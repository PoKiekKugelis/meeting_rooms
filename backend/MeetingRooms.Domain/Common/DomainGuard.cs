namespace MeetingRooms.Domain.Common;

/// <summary>
/// Provides guard methods for validating domain entities and value objects
/// </summary>
internal static class DomainGuard
{
  /// <summary>
  /// Ensures that a value is within the specified range, throwing an exception if it is not
  /// </summary>
  /// <param name="value">The value to validate</param>
  /// <param name="minimum">The minimum allowed value</param>
  /// <param name="maximum">The maximum allowed value</param>
  /// <param name="parameterName">The name of the parameter being validated</param>
  /// <returns>The validated value</returns>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  internal static int EnsureInRange(int value, int minimum, int maximum, string parameterName)
  {
    if (minimum > maximum)
    {
      throw new ArgumentOutOfRangeException(nameof(minimum), "The minimum cannot exceed the maximum.");
    }

    if (value < minimum || value > maximum)
    {
      throw new ArgumentOutOfRangeException( parameterName, value, $"Value must be between {minimum} and {maximum}.");
    }

    return value;
  }

  /// <summary>
  /// Normalizes a required text value by ensuring it does not exceed the specified maximum length, throwing an exception if it does
  /// </summary>
  /// <param name="value">The value to normalize</param>
  /// <param name="maxLength">The maximum allowed length</param>
  /// <param name="parameterName">The name of the parameter being validated</param>
  /// <returns>The normalized value</returns>
  /// <exception cref="ArgumentException"></exception>
  internal static string NormalizeRequiredText(string value, int maxLength, string parameterName)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);

    var normalized = value.Trim();
    if (normalized.Length > maxLength)
    {
      throw new ArgumentException($"Text cannot exceed {maxLength} characters.", parameterName);
    }

    return normalized;
  }
}
