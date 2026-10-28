import { PAGE_SIZE } from '../constants/pagination';
import type { Reservation, Room } from '../types/models';
import type { ScheduleView } from '../utils/schedule';
import { FeedbackState } from './FeedbackState';
import { Pagination } from './Pagination';
import { RoomCard } from './RoomCard';

interface RoomContentProps {
  isLoading: boolean;
  error: string;
  rooms: Room[];
  reservations: Reservation[];
  scheduleDays: Date[];
  scheduleView: ScheduleView;
  scheduleLoading: boolean;
  scheduleError: string;
  currentPage: number;
  totalItems: number;
  onPageChange: (page: number) => void;
  onRetryRooms: () => void;
  onRetrySchedule: () => void;
}

export function RoomContent({
  isLoading,
  error,
  rooms,
  reservations,
  scheduleDays,
  scheduleView,
  scheduleLoading,
  scheduleError,
  currentPage,
  totalItems,
  onPageChange,
  onRetryRooms,
  onRetrySchedule,
}: RoomContentProps) {
  if (isLoading) return <FeedbackState message="Loading rooms..." />;
  if (error) {
    return <FeedbackState message={error} actionLabel="Try again" onAction={onRetryRooms} />;
  }
  if (rooms.length === 0) return <FeedbackState message="No rooms match your filters." />;

  return (
    <div>
      <div className="grid gap-5 xl:grid-cols-3">
        {rooms.map((room) => (
          <RoomCard
            key={room.id}
            room={room}
            reservations={reservations}
            scheduleDays={scheduleDays}
            scheduleView={scheduleView}
            scheduleLoading={scheduleLoading}
            scheduleError={scheduleError}
            onRetrySchedule={onRetrySchedule}
          />
        ))}
      </div>
      <Pagination
        currentPage={currentPage}
        pageSize={PAGE_SIZE}
        totalItems={totalItems}
        onPageChange={onPageChange}
      />
    </div>
  );
}
