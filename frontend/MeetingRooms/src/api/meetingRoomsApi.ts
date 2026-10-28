import { API_ENDPOINTS } from '../constants/endpoints';
import type { Reservation, ReservationRequest, Room, UserSummary } from '../types/models';
import type { ReservationFilters, RoomFilters, UserFilters } from '../utils/validation';
import { request } from './client';

export function getRooms(filters: RoomFilters, signal?: AbortSignal) {
  return request<Room[]>(API_ENDPOINTS.rooms, {
    query: {
      minimumCapacity: filters.minimumCapacity,
      floor: filters.floor,
      equipment: Object.keys(filters.equipment),
      includeInactive: filters.includeInactive,
    },
    signal,
  });
}

export function getUserSummaries(filters: UserFilters, signal?: AbortSignal) {
  return request<UserSummary[]>(API_ENDPOINTS.users, {
    query: { includeReservationCount: filters.includeReservationCount },
    signal,
  });
}

export function getReservations(filters: ReservationFilters, signal?: AbortSignal) {
  return request<Reservation[]>(API_ENDPOINTS.reservations, {
    query: {
      roomId: filters.roomId,
      userId: filters.userId,
      status: filters.status,
      from: filters.from ? new Date(filters.from).toISOString() : undefined,
      to: filters.to ? new Date(filters.to).toISOString() : undefined,
    },
    signal,
  });
}

export function createReservation(body: ReservationRequest) {
  return request<Reservation>(API_ENDPOINTS.reservations, {
    method: 'POST',
    body: JSON.stringify(body),
  });
}

export function cancelReservation(id: number) {
  return request<void>(API_ENDPOINTS.reservationById(id), { method: 'DELETE' });
}

export function completeReservation(id: number) {
  return request<Reservation>(API_ENDPOINTS.completeReservation(id), { method: 'POST' });
}
