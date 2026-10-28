import { useQuery } from '@tanstack/react-query';
import { roomListQuery } from '../api/queries';
import type { RoomFilters } from '../utils/validation';

export function useRooms(filters: RoomFilters) {
  return useQuery(roomListQuery(filters));
}
