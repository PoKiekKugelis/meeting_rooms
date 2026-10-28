import { RESERVATION_STATUS } from '../constants/status';
import type { Reservation } from '../types/models';
import type { ScheduleView } from '../utils/schedule';

interface RoomScheduleProps {
  days: Date[];
  reservations: Reservation[];
  view: ScheduleView;
  isLoading: boolean;
  error: string;
  onRetry: () => void;
}

const dateFormatter = new Intl.DateTimeFormat(undefined, {
  weekday: 'short',
  month: 'short',
  day: 'numeric',
});
const timeFormatter = new Intl.DateTimeFormat(undefined, {
  hour: 'numeric',
  minute: '2-digit',
});

function getReservationsForDay(reservations: Reservation[], day: Date) {
  const start = new Date(day);
  start.setHours(0, 0, 0, 0);
  const end = new Date(start);
  end.setDate(end.getDate() + 1);

  return reservations
    .filter(
      (reservation) =>
        reservation.status !== RESERVATION_STATUS.CANCELLED &&
        new Date(reservation.startDate) < end &&
        new Date(reservation.endDate) > start,
    )
    .sort((a, b) => a.startDate.localeCompare(b.startDate));
}

function getReservationTime(reservation: Reservation, day: Date) {
  const dayStart = new Date(day);
  dayStart.setHours(0, 0, 0, 0);
  const dayEnd = new Date(dayStart);
  dayEnd.setDate(dayEnd.getDate() + 1);
  const start = new Date(Math.max(new Date(reservation.startDate).getTime(), dayStart.getTime()));
  const end = new Date(Math.min(new Date(reservation.endDate).getTime(), dayEnd.getTime()));
  return `${timeFormatter.format(start)} – ${timeFormatter.format(end)}`;
}

function getStatusClass(status: Reservation['status']) {
  return status === RESERVATION_STATUS.COMPLETED
    ? 'border-emerald-300 bg-emerald-50 text-emerald-900 dark:border-emerald-800 dark:bg-emerald-950/70 dark:text-emerald-200'
    : 'border-amber-300 bg-amber-50 text-amber-950 dark:border-amber-800 dark:bg-amber-950/70 dark:text-amber-200';
}

function DayTimeline({ day, reservations }: { day: Date; reservations: Reservation[] }) {
  const dayReservations = getReservationsForDay(reservations, day);
  const dayStart = new Date(day);
  dayStart.setHours(0, 0, 0, 0);
  const labels = Array.from({ length: 5 }, (_, index) => {
    const tick = new Date(dayStart);
    tick.setHours(index * 6);
    return timeFormatter.format(tick);
  });

  return (
    <div className="mt-3 overflow-x-auto pb-2">
      <div className="min-w-[680px]">
        <div className="grid grid-cols-5 text-xs text-slate-500 dark:text-slate-400">
          {labels.map((label, index) => (
            <span key={label + index} className={index === 4 ? 'text-right' : ''}>
              {label}
            </span>
          ))}
        </div>
        <div
          className="relative mt-2 border-y border-slate-200 bg-[linear-gradient(to_right,transparent_calc(25%-1px),rgb(148_163_184_/_0.25)_25%,transparent_calc(25%+1px),transparent_calc(50%-1px),rgb(148_163_184_/_0.25)_50%,transparent_calc(50%+1px),transparent_calc(75%-1px),rgb(148_163_184_/_0.25)_75%,transparent_calc(75%+1px))] dark:border-slate-700"
          style={{ minHeight: `${Math.max(48, dayReservations.length * 48)}px` }}
        >
          {dayReservations.map((reservation, index) => {
            const start = new Date(reservation.startDate);
            const end = new Date(reservation.endDate);
            const clippedStart = Math.max(start.getTime(), dayStart.getTime());
            const nextDay = new Date(dayStart);
            nextDay.setDate(nextDay.getDate() + 1);
            const clippedEnd = Math.min(end.getTime(), nextDay.getTime());
            const startMinutes = (clippedStart - dayStart.getTime()) / 60_000;
            const durationMinutes = (clippedEnd - clippedStart) / 60_000;
            const left = (startMinutes / 1440) * 100;
            const width = Math.min(100 - left, Math.max(2, (durationMinutes / 1440) * 100));

            return (
              <div
                key={reservation.id}
                title={`${reservation.title} · ${getReservationTime(reservation, day)} · ${reservation.userName}`}
                className={`absolute flex h-10 min-w-0 flex-col justify-center overflow-hidden rounded-md border px-2 text-xs ${getStatusClass(reservation.status)}`}
                style={{ left: `${left}%`, width: `${width}%`, top: `${index * 48 + 4}px` }}
              >
                <span className="truncate font-semibold">{reservation.title}</span>
                <span className="truncate">{getReservationTime(reservation, day)}</span>
              </div>
            );
          })}
          {dayReservations.length === 0 ? (
            <p className="px-3 py-4 text-sm text-slate-500 dark:text-slate-400">No bookings</p>
          ) : null}
        </div>
      </div>
    </div>
  );
}

export function RoomSchedule({
  days,
  reservations,
  view,
  isLoading,
  error,
  onRetry,
}: RoomScheduleProps) {
  if (isLoading) {
    return (
      <p role="status" className="mt-4 text-sm text-slate-500 dark:text-slate-400">
        Loading schedule...
      </p>
    );
  }

  if (error) {
    return (
      <div className="mt-4 flex flex-wrap items-center justify-between gap-2 text-sm text-rose-700 dark:text-rose-300">
        <p role="alert">Unable to load this schedule.</p>
        <button
          type="button"
          onClick={onRetry}
          className="font-semibold underline underline-offset-2"
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className="mt-5 border-t border-slate-200 pt-4 dark:border-slate-800">
      <p className="text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">
        Scheduled bookings
      </p>
      {view === 'day' ? (
        <DayTimeline day={days[0]} reservations={reservations} />
      ) : (
        <div className="mt-3 divide-y divide-slate-200 dark:divide-slate-800">
          {days.map((day) => {
            const dayReservations = getReservationsForDay(reservations, day);
            return (
              <section key={day.toISOString()} className="grid gap-2 py-3 sm:grid-cols-[7rem_1fr]">
                <h3 className="text-sm font-semibold text-slate-700 dark:text-slate-200">
                  {dateFormatter.format(day)}
                </h3>
                {dayReservations.length ? (
                  <ul className="grid gap-2">
                    {dayReservations.map((reservation) => (
                      <li
                        key={reservation.id}
                        title={`${reservation.userName} · ${reservation.status}`}
                        className={`rounded-md border px-3 py-2 text-sm ${getStatusClass(reservation.status)}`}
                      >
                        <span className="font-semibold">{reservation.title}</span>
                        <span className="ml-2 whitespace-nowrap text-xs">
                          {getReservationTime(reservation, day)}
                        </span>
                      </li>
                    ))}
                  </ul>
                ) : (
                  <p className="text-sm text-slate-500 dark:text-slate-400">No bookings</p>
                )}
              </section>
            );
          })}
        </div>
      )}
    </div>
  );
}
