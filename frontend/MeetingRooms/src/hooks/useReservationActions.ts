import { useCallback } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { cancelReservation, completeReservation, createReservation } from '../api/meetingRoomsApi';
import { queryKeys } from '../api/queries';
import type { ReservationRequest } from '../types/models';

export function useReservationActions() {
  const queryClient = useQueryClient();

  const invalidateServerState = useCallback(
    () =>
      Promise.all([
        queryClient.invalidateQueries({ queryKey: queryKeys.reservations.all }),
        queryClient.invalidateQueries({ queryKey: queryKeys.rooms.all }),
        queryClient.invalidateQueries({ queryKey: queryKeys.users.all }),
      ]),
    [queryClient],
  );

  const createMutation = useMutation({
    mutationFn: (body: ReservationRequest) => createReservation(body),
    onSuccess: invalidateServerState,
  });
  const cancelMutation = useMutation({
    mutationFn: (id: number) => cancelReservation(id),
    onSuccess: invalidateServerState,
  });
  const completeMutation = useMutation({
    mutationFn: (id: number) => completeReservation(id),
    onSuccess: invalidateServerState,
  });

  return {
    isLoading: createMutation.isPending || cancelMutation.isPending || completeMutation.isPending,
    create: createMutation.mutateAsync,
    cancel: cancelMutation.mutateAsync,
    complete: completeMutation.mutateAsync,
  };
}
