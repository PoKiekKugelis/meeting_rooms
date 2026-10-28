import { RoomFilters } from './RoomFilters';
import { RoomScheduleControls } from './RoomScheduleControls';
import type { RoomFilters as RoomFiltersValue } from '../utils/validation';
import type { ScheduleView } from '../utils/schedule';

interface RoomManagementPanelProps {
  filters: RoomFiltersValue;
  floorOptions: string[];
  equipmentOptions: string[];
  selectedDate: string;
  view: ScheduleView;
  onFiltersChange: (filters: RoomFiltersValue) => void;
  onApplyFilters: () => void;
  onDateChange: (date: string) => void;
  onViewChange: (view: ScheduleView) => void;
}

export function RoomManagementPanel({
  filters,
  floorOptions,
  equipmentOptions,
  selectedDate,
  view,
  onFiltersChange,
  onApplyFilters,
  onDateChange,
  onViewChange,
}: RoomManagementPanelProps) {
  return (
    <div>
      <RoomFilters
        value={filters}
        floorOptions={floorOptions}
        equipmentOptions={equipmentOptions}
        onChange={onFiltersChange}
        onApply={onApplyFilters}
      />
      <RoomScheduleControls
        selectedDate={selectedDate}
        view={view}
        onDateChange={onDateChange}
        onViewChange={onViewChange}
      />
    </div>
  );
}
