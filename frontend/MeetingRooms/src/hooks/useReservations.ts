import { useQuery } from '@tanstack/react-query';
import { reservationListQuery } from '../api/queries';
import type { ReservationFilters } from '../utils/validation';

export function useReservations(filters: ReservationFilters) {
  return useQuery(reservationListQuery(filters));
}
