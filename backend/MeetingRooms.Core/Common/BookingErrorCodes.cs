namespace MeetingRooms.Core.Common;

public static class BookingErrorCodes
{
  // Request shape
  public const string RoomIdInvalid = "ROOM_ID_INVALID";
  public const string OrganizerIdInvalid = "ORGANIZER_ID_INVALID";
  public const string TitleRequired = "TITLE_REQUIRED";
  public const string TitleTooLong = "TITLE_TOO_LONG";
  public const string AttendeeCountInvalid = "ATTENDEE_COUNT_INVALID";

  // Booking rules R1-R7
  public const string MaxDuration = "R1_MAX_DURATION";
  public const string RoomOverlap = "R2_ROOM_OVERLAP";
  public const string OrganizerOverlap = "R3_ORGANIZER_OVERLAP";
  public const string InvalidTimeRange = "R4_INVALID_TIME_RANGE";
  public const string StartNotFuture = "R4_START_NOT_FUTURE";
  public const string CapacityExceeded = "R5_CAPACITY_EXCEEDED";
  public const string BookingHorizon = "R6_BOOKING_HORIZON";
  public const string RoomInactive = "R7_ROOM_INACTIVE";

  // Existence and state
  public const string RoomNotFound = "ROOM_NOT_FOUND";
  public const string UserNotFound = "USER_NOT_FOUND";
  public const string ReservationNotFound = "RESERVATION_NOT_FOUND";
  public const string ReservationNotEditable = "RESERVATION_NOT_EDITABLE";
  public const string ReservationNotCompletable = "RESERVATION_NOT_COMPLETABLE";

  // Fallbacks for domain-guard and unexpected business failures
  public const string ValidationError = "VALIDATION_ERROR";
  public const string BusinessRuleViolation = "BUSINESS_RULE_VIOLATION";
}
