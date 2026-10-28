import { ROOM_STATUS } from '../constants/status';
import type { Reservation, Room } from '../types/models';
import type { ScheduleView } from '../utils/schedule';
import { RoomSchedule } from './RoomSchedule';

interface RoomCardProps {
  room: Room;
  reservations: Reservation[];
  scheduleDays: Date[];
  scheduleView: ScheduleView;
  scheduleLoading: boolean;
  scheduleError: string;
  onRetrySchedule: () => void;
}

export function RoomCard({
  room,
  reservations,
  scheduleDays,
  scheduleView,
  scheduleLoading,
  scheduleError,
  onRetrySchedule,
}: RoomCardProps) {
  const statusClass =
    room.status === ROOM_STATUS.ACTIVE
      ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300'
      : 'bg-slate-200 text-slate-700 dark:bg-slate-800 dark:text-slate-300';

  return (
    <article className="flex h-full flex-col rounded-xl border border-slate-200 bg-slate-50 p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md dark:border-slate-700 dark:bg-slate-950">
      <div className="flex items-start justify-between gap-4">
        <div>
          <p className="text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">
            Room #{room.id}
          </p>
          <h2 className="mt-1 text-xl font-bold text-slate-950 dark:text-white">{room.name}</h2>
        </div>
        <span className={`rounded-full px-3 py-1 text-xs font-bold ${statusClass}`}>
          {room.status}
        </span>
      </div>
      <dl className="mt-5 grid grid-cols-2 gap-3 text-sm">
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Capacity</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">{room.capacity} people</dd>
        </div>
        <div>
          <dt className="text-slate-500 dark:text-slate-400">Location</dt>
          <dd className="font-semibold text-slate-900 dark:text-white">{room.location}</dd>
        </div>
      </dl>
      <div className="mt-5">
        <p className="text-xs font-bold uppercase tracking-wider text-slate-500 dark:text-slate-400">
          Equipment
        </p>
        <div className="mt-2 flex flex-wrap gap-2">
          {room.equipment.length ? (
            room.equipment.map((item) => (
              <span
                key={item.name}
                title={`${item.name}, quantity ${item.quantity}`}
                className="rounded-md bg-slate-100 px-2 py-1 text-xs text-slate-700 dark:bg-slate-800 dark:text-slate-200"
              >
                {item.name} · {item.quantity}
              </span>
            ))
          ) : (
            <span className="text-sm text-slate-500 dark:text-slate-400">None listed</span>
          )}
        </div>
      </div>
      <RoomSchedule
        days={scheduleDays}
        reservations={reservations.filter((reservation) => reservation.roomId === room.id)}
        view={scheduleView}
        isLoading={scheduleLoading}
        error={scheduleError}
        onRetry={onRetrySchedule}
      />
    </article>
  );
}
