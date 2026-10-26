namespace MeetingRooms.Domain.Common;

internal static class DomainGuard
{
  internal static int EnsureInRange(int value, int minimum, int maximum, string parameterName)
  {
    if (minimum > maximum)
    {
      throw new ArgumentOutOfRangeException(nameof(minimum), "The minimum cannot exceed the maximum.");
    }

    if (value < minimum || value > maximum)
    {
      throw new ArgumentOutOfRangeException(
          parameterName,
          value,
          $"Value must be between {minimum} and {maximum}.");
    }

    return value;
  }

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
