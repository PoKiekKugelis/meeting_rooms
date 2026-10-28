import { RESERVATION_STATUSES } from '../constants/status';
import type { ReservationFilters as ReservationFiltersValue } from '../utils/validation';
import { Field, inputClass } from './Field';

interface FilterOption {
  id: number;
  label: string;
}
interface ReservationFiltersProps {
  value: ReservationFiltersValue;
  roomOptions: FilterOption[];
  userOptions: FilterOption[];
  error: string;
  onChange: (value: ReservationFiltersValue) => void;
  onApply: () => void;
}

export function ReservationFilters({
  value,
  roomOptions,
  userOptions,
  error,
  onChange,
  onApply,
}: ReservationFiltersProps) {
  const update = (patch: Partial<ReservationFiltersValue>) => onChange({ ...value, ...patch });
  return (
    <div>
      <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-5">
        <Field label="Room">
          <select
            className={inputClass}
            value={value.roomId}
            onChange={(event) => update({ roomId: event.target.value })}
          >
            <option value="">All rooms</option>
            {roomOptions.map((room) => (
              <option key={room.id} value={room.id}>
                {room.label}
              </option>
            ))}
          </select>
        </Field>
        <Field label="Organizer">
          <select
            className={inputClass}
            value={value.userId}
            onChange={(event) => update({ userId: event.target.value })}
          >
            <option value="">All organizers</option>
            {userOptions.map((user) => (
              <option key={user.id} value={user.id}>
                {user.label}
              </option>
            ))}
          </select>
        </Field>
        <Field label="Status">
          <select
            className={inputClass}
            value={value.status}
            onChange={(event) =>
              update({ status: event.target.value as ReservationFiltersValue['status'] })
            }
          >
            <option value="">Any status</option>
            {RESERVATION_STATUSES.map((status) => (
              <option key={status} value={status}>
                {status}
              </option>
            ))}
          </select>
        </Field>
        <Field label="From">
          <input
            className={inputClass}
            type="datetime-local"
            value={value.from}
            onChange={(event) => update({ from: event.target.value })}
          />
        </Field>
        <Field label="To">
          <input
            className={inputClass}
            type="datetime-local"
            value={value.to}
            onChange={(event) => update({ to: event.target.value })}
          />
        </Field>
      </div>
      {error ? (
        <p role="alert" className="mt-3 text-sm text-rose-700 dark:text-rose-300">
          {error}
        </p>
      ) : null}
      <button
        type="button"
        disabled={Boolean(error)}
        onClick={onApply}
        className="mt-4 rounded-lg bg-emerald-600 px-4 py-2 text-sm font-semibold text-white hover:bg-emerald-700 disabled:cursor-not-allowed disabled:opacity-50"
      >
        Apply filters
      </button>
    </div>
  );
}
