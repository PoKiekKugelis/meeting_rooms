import { EMPTY_RESERVATION_FILTERS } from './validation';
import type { ReservationFilters } from './validation';

export type ScheduleView = 'day' | 'week';

export function toDateInputValue(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

export function fromDateInputValue(value: string): Date {
  const [year, month, day] = value.split('-').map(Number);
  return new Date(year, month - 1, day);
}

export function addDays(date: Date, days: number): Date {
  const result = new Date(date);
  result.setDate(result.getDate() + days);
  return result;
}

function startOfWeek(date: Date): Date {
  const result = new Date(date);
  result.setHours(0, 0, 0, 0);
  result.setDate(result.getDate() - ((result.getDay() + 6) % 7));
  return result;
}

export function getScheduleDays(selectedDate: string, view: ScheduleView): Date[] {
  const date = fromDateInputValue(selectedDate);
  const start = view === 'week' ? startOfWeek(date) : date;
  const numberOfDays = view === 'week' ? 7 : 1;

  return Array.from({ length: numberOfDays }, (_, index) => addDays(start, index));
}

function toLocalDateTime(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  const seconds = String(date.getSeconds()).padStart(2, '0');
  return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
}

export function getScheduleFilters(days: Date[]): ReservationFilters {
  const start = days[0];
  const end = addDays(start, days.length);

  return {
    ...EMPTY_RESERVATION_FILTERS,
    from: toLocalDateTime(start),
    to: toLocalDateTime(end),
  };
}

export function formatSchedulePeriod(days: Date[], view: ScheduleView): string {
  const formatDate = new Intl.DateTimeFormat(undefined, {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });

  return view === 'day'
    ? formatDate.format(days[0])
    : `${formatDate.format(days[0])} – ${formatDate.format(days[6])}`;
}
