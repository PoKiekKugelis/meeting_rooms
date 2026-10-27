import type { ReservationRequest } from '../types/models';
import type { ReservationStatus } from '../constants/status';

export interface RoomFilters {
  minimumCapacity: string;
  floor: string;
  equipment: string[];
  includeInactive: boolean;
}

export interface ReservationFilters {
  roomId: string;
  userId: string;
  status: ReservationStatus | '';
  from: string;
  to: string;
}

export const EMPTY_RESERVATION_FILTERS: ReservationFilters = {
  roomId: '',
  userId: '',
  status: '',
  from: '',
  to: '',
};

export const EMPTY_ROOM_FILTERS: RoomFilters = {
  minimumCapacity: '',
  floor: '',
  equipment: [],
  includeInactive: false,
};

export function validateReservation(request: ReservationRequest): string {
  if (!request.roomId || !request.userId) return 'Select a room and organizer.';
  if (!request.title.trim()) return 'Enter a meeting title.';
  if (request.title.trim().length > 200) return 'The title cannot exceed 200 characters.';
  if (!request.startDate || !request.endDate) return 'Select both start and end times.';

  const start = new Date(request.startDate);
  const end = new Date(request.endDate);
  if (Number.isNaN(start.getTime()) || Number.isNaN(end.getTime())) return 'Enter valid dates.';
  if (start <= new Date()) return 'The meeting must start in the future.';
  if (end <= start) return 'The end time must be after the start time.';
  if (request.attendeeCount < 1) return 'Attendee count must be at least one.';
  return '';
}

export function validateFilterRange(from: string, to: string): string {
  if (from && to && new Date(from) > new Date(to))
    return 'The start filter must be before the end filter.';
  return '';
}
