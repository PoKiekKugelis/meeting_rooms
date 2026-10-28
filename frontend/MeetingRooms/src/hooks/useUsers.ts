import { useQuery } from '@tanstack/react-query';
import { userListQuery } from '../api/queries';
import type { UserFilters } from '../utils/validation';

export function useUsers(filters: UserFilters) {
  return useQuery(userListQuery(filters));
}
