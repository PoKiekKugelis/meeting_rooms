import type { RoomFilters as RoomFiltersValue } from '../utils/validation';
import { Field, inputClass } from './Field';

interface RoomFiltersProps {
  value: RoomFiltersValue;
  floorOptions: string[];
  equipmentOptions: string[];
  onChange: (value: RoomFiltersValue) => void;
  onApply: () => void;
}

export function RoomFilters({
  value,
  floorOptions,
  equipmentOptions,
  onChange,
  onApply,
}: RoomFiltersProps) {
  const update = (patch: Partial<RoomFiltersValue>) => onChange({ ...value, ...patch });
  return (
    <div>
      <div className="grid gap-4 md:grid-cols-3">
        <Field label="Minimum capacity">
          <input
            className={inputClass}
            type="number"
            min="1"
            value={value.minimumCapacity}
            onChange={(event) => update({ minimumCapacity: event.target.value })}
            placeholder="Any capacity"
          />
        </Field>
        <Field label="Floor / location">
          <select
            className={inputClass}
            value={value.floor}
            onChange={(event) => update({ floor: event.target.value })}
          >
            <option value="">All locations</option>
            {floorOptions.map((location) => (
              <option key={location} value={location}>
                {location}
              </option>
            ))}
          </select>
        </Field>
      </div>
      <fieldset className="mt-5">
        <legend className="text-sm font-semibold text-slate-700 dark:text-slate-200">
          Required equipment
        </legend>
        <div className="mt-2 flex flex-wrap gap-3">
          {equipmentOptions.map((item) => {
            const minimumQuantity = value.equipment[item] ?? '';
            const isSelected = item in value.equipment;

            return (
              <div
                key={item}
                className="flex items-center gap-3 rounded-lg border border-slate-200 px-3 py-2 dark:border-slate-700"
              >
                <label className="flex cursor-pointer items-center gap-2 text-sm text-slate-700 dark:text-slate-200">
                  <input
                    type="checkbox"
                    checked={isSelected}
                    onChange={(event) => {
                      const equipment = { ...value.equipment };
                      if (event.target.checked) equipment[item] = '1';
                      else delete equipment[item];
                      update({ equipment });
                    }}
                  />
                  {item}
                </label>
                {isSelected ? (
                  <input
                    className="w-20 rounded-md border border-slate-300 bg-white px-2 py-1 text-sm text-slate-900 dark:border-slate-700 dark:bg-slate-950 dark:text-slate-100"
                    type="number"
                    min="1"
                    step="1"
                    value={minimumQuantity}
                    onChange={(event) =>
                      update({
                        equipment: { ...value.equipment, [item]: event.target.value },
                      })
                    }
                  />
                ) : null}
              </div>
            );
          })}
          {equipmentOptions.length === 0 ? (
            <p className="text-sm text-slate-500 dark:text-slate-400">
              No equipment is currently listed for rooms
            </p>
          ) : null}
        </div>
        <p className="mt-1 text-xs text-slate-500 dark:text-slate-400">
          Rooms must have at least the selected quantity of every chosen item
        </p>
      </fieldset>
      <label className="mt-4 flex cursor-pointer items-center gap-2 text-sm font-medium text-slate-700 dark:text-slate-200">
        <input
          type="checkbox"
          checked={value.includeInactive}
          onChange={(event) => update({ includeInactive: event.target.checked })}
        />{' '}
        Show inactive rooms
      </label>
      <button
        type="button"
        onClick={onApply}
        className="mt-4 rounded-lg bg-emerald-600 px-4 py-2 text-sm font-semibold text-white hover:bg-emerald-700"
      >
        Apply filters
      </button>
    </div>
  );
}
