import { RESERVATION_STATUS } from '../constants/status';
import { ConfirmationDialog } from './ConfirmationDialog';
import type { Reservation } from '../types/models';
import { formatDateTime } from '../utils/format';

interface ReservationCardProps {
  reservation: Reservation;
  now: number;
  isLoading: boolean;
  actionError?: string;
  canCancel: boolean;
  canComplete: boolean;
  onCancel: () => void;
  onComplete: () => void;
}

export function ReservationCard({
  reservation,
  now,
  isLoading,
  actionError,
  canCancel,
  canComplete,
  onCancel,
  onComplete,
}: ReservationCardProps) {
  const end = new Date(reservation.endDate).getTime();
  const statusClass =
    reservation.status === RESERVATION_STATUS.BOOKED
      ? 'bg-amber-100 text-amber-800 dark:bg-amber-950 dark:text-amber-300'
      : reservation.status === RESERVATION_STATUS.COMPLETED
        ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300'
        : 'bg-slate-200 text-slate-700 dark:bg-slate-800 dark:text-slate-300';

  return (
    <article className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md dark:border-slate-800 dark:bg-slate-900">
      <div className="flex flex-wrap items-start justify-between gap-3">
        <div>
          <p className="text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">
            Room #{reservation.roomId} · {reservation.roomName}
          </p>
          <h2 className="mt-1 text-lg font-bold text-slate-950 dark:text-white">
            {reservation.title}
          </h2>
        </div>
        <span className={`rounded-full px-3 py-1 text-xs font-bold ${statusClass}`}>
          {reservation.status}
        </span>
      </div>
      <dl className="mt-5 grid gap-3 text-sm sm:grid-cols-2">
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Organizer</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">
            {reservation.userName} (#{reservation.userId})
          </dd>
        </div>
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Attendees</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">
            {reservation.attendeeCount}
          </dd>
        </div>
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Starts</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">
            {formatDateTime(reservation.startDate)}
          </dd>
        </div>
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Ends</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">
            {formatDateTime(reservation.endDate)}
          </dd>
        </div>
      </dl>
      {reservation.completedAt ? (
        <p className="mt-4 text-xs text-slate-500 dark:text-slate-400">
          Completed at {formatDateTime(reservation.completedAt)}
        </p>
      ) : null}
      {reservation.status === RESERVATION_STATUS.BOOKED && now >= end ? (
        <p role="status" className="mt-4 text-sm text-slate-500 dark:text-slate-400">
          Meeting ended; status is being updated.
        </p>
      ) : null}
      {canCancel || canComplete ? (
        <div className="mt-5 flex flex-wrap items-center gap-2">
          {canComplete ? (
            <ConfirmationDialog
              title="End this meeting early?"
              description={`This will mark “${reservation.title}” as completed now and release the room.`}
              confirmLabel="End meeting"
              onConfirm={onComplete}
              trigger={
                <button
                  type="button"
                  disabled={isLoading}
                  className="rounded-lg bg-emerald-600 px-3 py-2 text-sm font-semibold text-white hover:bg-emerald-700 disabled:cursor-not-allowed disabled:opacity-50"
                >
                  End meeting
                </button>
              }
            />
          ) : null}
          {canCancel ? (
            <ConfirmationDialog
              title="Cancel this reservation?"
              description={`“${reservation.title}” will be cancelled and the room will become available.`}
              confirmLabel="Cancel reservation"
              destructive
              onConfirm={onCancel}
              trigger={
                <button
                  type="button"
                  disabled={isLoading}
                  className="rounded-lg bg-rose-600 text-white px-3 py-2 text-sm font-semibold text-rose-700 hover:bg-rose-700 disabled:cursor-not-allowed disabled:opacity-50"
                >
                  Cancel reservation
                </button>
              }
            />
          ) : null}
        </div>
      ) : null}
      {actionError ? (
        <p
          role="alert"
          className="mt-4 rounded-lg bg-rose-50 p-3 text-sm text-rose-800 dark:bg-rose-950/50 dark:text-rose-200"
        >
          {actionError}
        </p>
      ) : null}
    </article>
  );
}
