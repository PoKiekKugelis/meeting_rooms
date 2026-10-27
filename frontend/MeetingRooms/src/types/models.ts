import type { ReservationStatus, RoomStatus } from '../constants/status';

export interface Room {
  id: number;
  name: string;
  capacity: number;
  location: string;
  status: RoomStatus;
  isCurrentlyReserved: boolean;
  equipment: string[];
}

export interface User {
  id: number;
  firstName: string;
  lastName: string;
}

export interface UserSummary extends User {
  activeReservationsCount: number;
}

export interface Reservation {
  id: number;
  roomId: number;
  roomName: string;
  userId: number;
  userName: string;
  title: string;
  startDate: string;
  endDate: string;
  attendeeCount: number;
  status: ReservationStatus;
  completedAt: string | null;
}

export interface ReservationRequest {
  roomId: number;
  userId: number;
  title: string;
  startDate: string;
  endDate: string;
  attendeeCount: number;
}
