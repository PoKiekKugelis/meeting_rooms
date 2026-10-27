export const RESERVATION_STATUS = {
  BOOKED: 'Booked',
  COMPLETED: 'Completed',
  CANCELLED: 'Cancelled',
} as const;

export type ReservationStatus = (typeof RESERVATION_STATUS)[keyof typeof RESERVATION_STATUS];
export const RESERVATION_STATUSES = Object.values(RESERVATION_STATUS);

export const ROOM_STATUS = {
  ACTIVE: 'Active',
  INACTIVE: 'Inactive',
} as const;

export type RoomStatus = (typeof ROOM_STATUS)[keyof typeof ROOM_STATUS];
