import { useMemo, useState } from 'react';
import { ROOM_STATUS } from '../constants/status';
import { PAGE_SIZE } from '../constants/pagination';
import { PageHeader } from '../components/PageHeader';
import { RoomContent } from '../components/RoomContent';
import { RoomManagementPanel } from '../components/RoomManagementPanel';
import { useReservations } from '../hooks/useReservations';
import { useRooms } from '../hooks/useRooms';
import type { Reservation, Room } from '../types/models';
import { EMPTY_ROOM_FILTERS, type RoomFilters } from '../utils/validation';
import {
  getScheduleDays,
  getScheduleFilters,
  toDateInputValue,
  type ScheduleView,
} from '../utils/schedule';

const ALL_ROOMS: RoomFilters = { ...EMPTY_ROOM_FILTERS, includeInactive: true };
const NO_ROOMS: Room[] = [];
const NO_RESERVATIONS: Reservation[] = [];

function filterRooms(rooms: Room[], filters: RoomFilters): Room[] {
  const minimumCapacity = Number(filters.minimumCapacity) || 0;

  return rooms.filter(
    (room) =>
      (filters.includeInactive || room.status === ROOM_STATUS.ACTIVE) &&
      room.capacity >= minimumCapacity &&
      (!filters.floor || room.location === filters.floor) &&
      Object.entries(filters.equipment).every(([name, quantity]) => {
        const minimumQuantity = Math.max(1, Math.ceil(Number(quantity) || 1));
        return room.equipment.some(
          (item) => item.name === name && item.quantity >= minimumQuantity,
        );
      }),
  );
}

export function RoomsPage() {
  const [draftFilters, setDraftFilters] = useState(EMPTY_ROOM_FILTERS);
  const [appliedFilters, setAppliedFilters] = useState(EMPTY_ROOM_FILTERS);
  const [roomPage, setRoomPage] = useState(1);
  const [scheduleView, setScheduleView] = useState<ScheduleView>('day');
  const [selectedDate, setSelectedDate] = useState(() => toDateInputValue(new Date()));

  const roomsQuery = useRooms(ALL_ROOMS);
  const roomData = roomsQuery.data ?? NO_ROOMS;
  const floorOptions = [...new Set(roomData.map((room) => room.location))].sort();
  const equipmentOptions = [
    ...new Set(roomData.flatMap((room) => room.equipment.map((item) => item.name))),
  ].sort();
  const visibleRooms = useMemo(
    () => filterRooms(roomData, appliedFilters),
    [roomData, appliedFilters],
  );
  const pageCount = Math.max(1, Math.ceil(visibleRooms.length / PAGE_SIZE));
  const currentPage = Math.min(roomPage, pageCount);
  const paginatedRooms = visibleRooms.slice((currentPage - 1) * PAGE_SIZE, currentPage * PAGE_SIZE);

  const scheduleDays = getScheduleDays(selectedDate, scheduleView);
  const scheduleFilters = getScheduleFilters(scheduleDays);
  const scheduleQuery = useReservations(scheduleFilters);

  const reservations = scheduleQuery.data ?? NO_RESERVATIONS;
  const scheduleError = scheduleQuery.error?.message ?? '';

  return (
    <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <PageHeader
        title="Find rooms"
        description="Meeting room details and scheduled bookings timelines"
      />
      <section
        aria-label="Room search and schedules"
        className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm dark:border-slate-800 dark:bg-slate-900"
      >
        <div className="p-5">
          <RoomManagementPanel
            filters={draftFilters}
            floorOptions={floorOptions}
            equipmentOptions={equipmentOptions}
            selectedDate={selectedDate}
            view={scheduleView}
            onFiltersChange={setDraftFilters}
            onApplyFilters={() => {
              setAppliedFilters(draftFilters);
              setRoomPage(1);
            }}
            onDateChange={setSelectedDate}
            onViewChange={setScheduleView}
          />
        </div>
        <div className="border-t border-slate-200 p-5 dark:border-slate-800">
          <RoomContent
            isLoading={roomsQuery.isLoading}
            error={roomsQuery.error?.message ?? ''}
            rooms={paginatedRooms}
            reservations={reservations}
            scheduleDays={scheduleDays}
            scheduleView={scheduleView}
            scheduleLoading={scheduleQuery.isLoading}
            scheduleError={scheduleError}
            currentPage={currentPage}
            totalItems={visibleRooms.length}
            onPageChange={setRoomPage}
            onRetryRooms={() => void roomsQuery.refetch()}
            onRetrySchedule={() => void scheduleQuery.refetch()}
          />
        </div>
      </section>
      {scheduleQuery.isRefetching ? (
        <p role="status" className="mt-4 text-sm text-slate-500 dark:text-slate-400">
          Updating room schedules...
        </p>
      ) : null}
    </main>
  );
}
