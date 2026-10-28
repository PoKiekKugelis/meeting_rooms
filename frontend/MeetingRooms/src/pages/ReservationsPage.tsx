import { useEffect, useState } from 'react';
import { RESERVATION_STATUS } from '../constants/status';
import { PageHeader } from '../components/PageHeader';
import { ReservationManagementPanel } from '../components/ReservationManagementPanel';
import { ReservationContent } from '../components/ReservationContent';
import { useReservationActions } from '../hooks/useReservationActions';
import { useReservations } from '../hooks/useReservations';
import type { Reservation, ReservationRequest } from '../types/models';
import { EMPTY_RESERVATION_FILTERS, validateFilterRange } from '../utils/validation';

const CLOCK_REFRESH_INTERVAL_MS = 30_000;
const EXPIRED_RESERVATION_REFRESH_INTERVAL_MS = 15_000;
const NO_RESERVATIONS: Reservation[] = [];

export function ReservationsPage() {
  const [draftFilters, setDraftFilters] = useState(EMPTY_RESERVATION_FILTERS);
  const [appliedFilters, setAppliedFilters] = useState(EMPTY_RESERVATION_FILTERS);
  const [page, setPage] = useState(1);
  const [actionErrors, setActionErrors] = useState<Record<number, string>>({});
  const [now, setNow] = useState(() => Date.now());

  const reservations = useReservations(appliedFilters);
  const { create, cancel, complete, isLoading: isActionPending } = useReservationActions();
  const data = reservations.data ?? NO_RESERVATIONS;
  const refetchReservations = reservations.refetch;
  const filterError = validateFilterRange(draftFilters.from, draftFilters.to);
  const hasExpiredBookedReservation = data.some(
    (reservation) =>
      reservation.status === RESERVATION_STATUS.BOOKED &&
      now >= new Date(reservation.endDate).getTime(),
  );

  useEffect(() => {
    const timer = window.setInterval(() => setNow(Date.now()), CLOCK_REFRESH_INTERVAL_MS);
    return () => window.clearInterval(timer);
  }, []);

  useEffect(() => {
    if (!hasExpiredBookedReservation) return;

    const timer = window.setInterval(
      () => void refetchReservations(),
      EXPIRED_RESERVATION_REFRESH_INTERVAL_MS,
    );
    return () => window.clearInterval(timer);
  }, [hasExpiredBookedReservation, refetchReservations]);

  const createReservation = async (request: ReservationRequest) => {
    await create(request);
  };

  const handleReservationAction = async (id: number, action: 'cancel' | 'complete') => {
    setActionErrors((previous) => {
      const next = { ...previous };
      delete next[id];
      return next;
    });

    try {
      if (action === 'cancel') await cancel(id);
      else await complete(id);
    } catch (requestError) {
      const message =
        requestError instanceof Error
          ? requestError.message
          : 'The reservation action could not be completed.';
      setActionErrors((previous) => ({ ...previous, [id]: message }));
    }
  };

  const applyFilters = () => {
    if (filterError) return;
    setAppliedFilters(draftFilters);
    setPage(1);
  };

  return (
    <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <PageHeader
        title="Manage reservations"
        description="Create bookings, cancel upcoming meetings or end a meeting early"
      />
      <section
        aria-label="Reservation management"
        className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm dark:border-slate-800 dark:bg-slate-900"
      >
        <div className="p-5">
          <ReservationManagementPanel
            filters={draftFilters}
            filterError={filterError}
            isSubmitting={isActionPending}
            onFiltersChange={setDraftFilters}
            onApplyFilters={applyFilters}
            onCreate={createReservation}
          />
        </div>
        <div className="border-t border-slate-200 p-5 dark:border-slate-800">
          <ReservationContent
            data={data}
            hasData={reservations.data !== undefined}
            isLoading={reservations.isLoading}
            isRefetching={reservations.isRefetching}
            error={reservations.error}
            now={now}
            isActionPending={isActionPending}
            actionErrors={actionErrors}
            page={page}
            onPageChange={setPage}
            onRetry={() => void refetchReservations()}
            onAction={(id, action) => void handleReservationAction(id, action)}
          />
        </div>
      </section>
    </main>
  );
}
