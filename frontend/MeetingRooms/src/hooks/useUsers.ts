import { useQuery } from '@tanstack/react-query';
import { usersQuery } from '../api/queries';

export function useUsers() {
  return useQuery(usersQuery());
}
