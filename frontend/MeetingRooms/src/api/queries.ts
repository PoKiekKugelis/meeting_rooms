import { queryOptions } from '@tanstack/react-query';
import { getReservations, getRooms, getUserSummaries } from './meetingRoomsApi';
import type { ReservationFilters, RoomFilters } from '../utils/validation';

export const queryKeys = {
  rooms: {
    all: ['rooms'] as const,
    list: (filters: RoomFilters) => ['rooms', 'list', filters] as const,
  },
  users: {
    all: ['users'] as const,
  },
  reservations: {
    all: ['reservations'] as const,
    list: (filters: ReservationFilters) => ['reservations', 'list', filters] as const,
  },
};

export const roomListQuery = (filters: RoomFilters) =>
  queryOptions({
    queryKey: queryKeys.rooms.list(filters),
    queryFn: ({ signal }) => getRooms(filters, signal),
  });

export const usersQuery = () =>
  queryOptions({
    queryKey: queryKeys.users.all,
    queryFn: ({ signal }) => getUserSummaries(signal),
  });

export const reservationListQuery = (filters: ReservationFilters) =>
  queryOptions({
    queryKey: queryKeys.reservations.list(filters),
    queryFn: ({ signal }) => getReservations(filters, signal),
  });
