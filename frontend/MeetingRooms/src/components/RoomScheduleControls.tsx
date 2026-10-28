import {
  addDays,
  formatSchedulePeriod,
  fromDateInputValue,
  getScheduleDays,
  toDateInputValue,
  type ScheduleView,
} from '../utils/schedule';

interface RoomScheduleControlsProps {
  selectedDate: string;
  view: ScheduleView;
  onDateChange: (date: string) => void;
  onViewChange: (view: ScheduleView) => void;
}

const buttonClass =
  'rounded-lg border border-slate-300 px-3 py-2 text-sm font-semibold text-slate-700 hover:bg-slate-50 dark:border-slate-700 dark:text-slate-200 dark:hover:bg-slate-800';

export function RoomScheduleControls({
  selectedDate,
  view,
  onDateChange,
  onViewChange,
}: RoomScheduleControlsProps) {
  const periodLabel = formatSchedulePeriod(getScheduleDays(selectedDate, view), view);

  const navigate = (direction: -1 | 1) => {
    const offset = view === 'week' ? 7 : 1;
    onDateChange(toDateInputValue(addDays(fromDateInputValue(selectedDate), direction * offset)));
  };

  return (
    <div className="flex flex-wrap items-center justify-between gap-4 pt-5">
      <div className="flex flex-wrap items-center gap-3">
        <label
          htmlFor="schedule-date"
          className="text-sm font-semibold text-slate-700 dark:text-slate-200"
        >
          Schedule date
        </label>
        <input
          id="schedule-date"
          type="date"
          value={selectedDate}
          onChange={(event) => onDateChange(event.target.value || toDateInputValue(new Date()))}
          className="rounded-lg border border-slate-300 bg-white px-3 py-2 text-sm text-slate-900 dark:border-slate-700 dark:bg-slate-950 dark:text-white"
        />
        <span className="text-sm text-slate-500 dark:text-slate-400">{periodLabel}</span>
      </div>

      <div className="flex flex-wrap items-center gap-2">
        <button type="button" onClick={() => navigate(-1)} className={buttonClass}>
          Previous
        </button>
        <button
          type="button"
          onClick={() => onDateChange(toDateInputValue(new Date()))}
          className={buttonClass}
        >
          Today
        </button>
        <button type="button" onClick={() => navigate(1)} className={buttonClass}>
          Next
        </button>
        <div className="ml-1 flex rounded-lg bg-slate-100 p-1 dark:bg-slate-800" role="group">
          {(['day', 'week'] as const).map((option) => (
            <button
              key={option}
              type="button"
              aria-pressed={view === option}
              onClick={() => onViewChange(option)}
              className={`rounded-md px-3 py-1.5 text-sm font-semibold capitalize ${
                view === option
                  ? 'bg-white text-slate-950 shadow-sm dark:bg-slate-700 dark:text-white'
                  : 'text-slate-600 hover:text-slate-950 dark:text-slate-300 dark:hover:text-white'
              }`}
            >
              {option}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
