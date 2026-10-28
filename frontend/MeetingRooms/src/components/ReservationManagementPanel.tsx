import { EMPTY_ROOM_FILTERS } from '../utils/validation';
import type {
  ReservationFilters as ReservationFiltersValue,
  RoomFilters as RoomFiltersValue,
} from '../utils/validation';
import type { ReservationRequest, Room, UserSummary } from '../types/models';
import { useRooms } from '../hooks/useRooms';
import { useUsers } from '../hooks/useUsers';
import { FeedbackState } from './FeedbackState';
import { ReservationFilters } from './ReservationFilters';
import { ReservationForm } from './ReservationForm';

const ALL_ROOMS: RoomFiltersValue = { ...EMPTY_ROOM_FILTERS, includeInactive: true };
const NO_ROOMS: Room[] = [];
const NO_USERS: UserSummary[] = [];

interface ReservationManagementPanelProps {
  filters: ReservationFiltersValue;
  filterError: string;
  isSubmitting: boolean;
  onFiltersChange: (filters: ReservationFiltersValue) => void;
  onApplyFilters: () => void;
  onCreate: (request: ReservationRequest) => Promise<void>;
}

export function ReservationManagementPanel({
  filters,
  filterError,
  isSubmitting,
  onFiltersChange,
  onApplyFilters,
  onCreate,
}: ReservationManagementPanelProps) {
  const rooms = useRooms(ALL_ROOMS);
  const users = useUsers({ includeReservationCount: false });
  const roomData = rooms.data ?? NO_ROOMS;
  const userData = users.data ?? NO_USERS;
  const roomOptions = roomData.map((room) => ({
    id: room.id,
    label: `${room.name} (#${room.id})`,
  }));
  const userOptions = userData.map((user) => ({
    id: user.id,
    label: `${user.firstName} ${user.lastName} (#${user.id})`,
  }));
  const referenceError = rooms.error?.message || users.error?.message;

  return (
    <div>
      {rooms.isLoading || users.isLoading ? (
        <FeedbackState message="Loading rooms and organizers..." />
      ) : referenceError ? (
        <FeedbackState
          message={referenceError}
          actionLabel="Reload"
          onAction={() => {
            void rooms.refetch();
            void users.refetch();
          }}
        />
      ) : (
        <ReservationForm
          rooms={roomData}
          users={userData}
          isLoading={isSubmitting}
          onSubmit={onCreate}
        />
      )}

      <div className="mt-5 border-t border-slate-200 pt-5 dark:border-slate-800">
        <ReservationFilters
          value={filters}
          roomOptions={roomOptions}
          userOptions={userOptions}
          error={filterError}
          onChange={onFiltersChange}
          onApply={onApplyFilters}
        />
      </div>
    </div>
  );
}
