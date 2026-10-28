import { useMemo } from 'react';
import { RESERVATION_STATUS } from '../constants/status';
import { PAGE_SIZE } from '../constants/pagination';
import type { Reservation } from '../types/models';
import { FeedbackState } from './FeedbackState';
import { Pagination } from './Pagination';
import { ReservationCard } from './ReservationCard';

type ReservationAction = 'cancel' | 'complete';

interface ReservationContentProps {
  data: Reservation[];
  hasData: boolean;
  isLoading: boolean;
  isRefetching: boolean;
  error: Error | null;
  now: number;
  isActionPending: boolean;
  actionErrors: Record<number, string>;
  page: number;
  onPageChange: (page: number) => void;
  onRetry: () => void;
  onAction: (id: number, action: ReservationAction) => void;
}

export function ReservationContent({
  data,
  hasData,
  isLoading,
  isRefetching,
  error,
  now,
  isActionPending,
  actionErrors,
  page,
  onPageChange,
  onRetry,
  onAction,
}: ReservationContentProps) {
  const sortedReservations = useMemo(
    () => [...data].sort((first, second) => first.startDate.localeCompare(second.startDate)),
    [data],
  );
  const pageCount = Math.max(1, Math.ceil(sortedReservations.length / PAGE_SIZE));
  const currentPage = Math.min(page, pageCount);
  const pageReservations = sortedReservations.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE,
  );

  if (isLoading) return <FeedbackState message="Loading reservations..." />;
  if (error && !hasData) {
    return <FeedbackState message={error.message} actionLabel="Try again" onAction={onRetry} />;
  }

  return (
    <>
      {error ? (
        <div
          role="alert"
          className="mb-4 flex items-center justify-between gap-4 rounded-xl border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-800 dark:border-rose-900 dark:bg-rose-950/50 dark:text-rose-200"
        >
          <span>{error.message} Showing the last successfully loaded results.</span>
          <button
            type="button"
            onClick={onRetry}
            className="shrink-0 font-semibold underline underline-offset-2"
          >
            Retry
          </button>
        </div>
      ) : null}
      {isRefetching ? (
        <p role="status" className="mb-3 text-sm text-slate-500 dark:text-slate-400">
          Updating reservations...
        </p>
      ) : null}
      {sortedReservations.length === 0 ? (
        <FeedbackState message="No reservations match the applied filters." />
      ) : (
        <>
          <div className="grid gap-4 lg:grid-cols-2">
            {pageReservations.map((reservation) => {
              const isUpcoming =
                reservation.status === RESERVATION_STATUS.BOOKED &&
                now < new Date(reservation.startDate).getTime();
              const isRunning =
                reservation.status === RESERVATION_STATUS.BOOKED &&
                now >= new Date(reservation.startDate).getTime() &&
                now < new Date(reservation.endDate).getTime();

              return (
                <ReservationCard
                  key={reservation.id}
                  reservation={reservation}
                  now={now}
                  isLoading={isActionPending}
                  actionError={actionErrors[reservation.id]}
                  onCancel={() => onAction(reservation.id, 'cancel')}
                  canCancel={isUpcoming}
                  canComplete={isRunning}
                  onComplete={() => onAction(reservation.id, 'complete')}
                />
              );
            })}
          </div>
          <Pagination
            currentPage={currentPage}
            pageSize={PAGE_SIZE}
            totalItems={sortedReservations.length}
            onPageChange={onPageChange}
          />
        </>
      )}
    </>
  );
}
