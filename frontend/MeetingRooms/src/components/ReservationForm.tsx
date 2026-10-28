import { useState, type SubmitEvent } from 'react';
import { ROOM_STATUS } from '../constants/status';
import type { ReservationRequest, Room, UserSummary } from '../types/models';
import { toUtcIso } from '../utils/format';
import { validateReservation } from '../utils/validation';
import { Field, inputClass } from './Field';

interface ReservationFormProps {
  rooms: Room[];
  users: UserSummary[];
  isLoading: boolean;
  onSubmit: (request: ReservationRequest) => Promise<void>;
}

export function ReservationForm({ rooms, users, isLoading, onSubmit }: ReservationFormProps) {
  const [roomId, setRoomId] = useState('');
  const [userId, setUserId] = useState('');
  const [title, setTitle] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [attendeeCount, setAttendeeCount] = useState('1');
  const [validationError, setValidationError] = useState('');

  const submit = async (event: SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    const request: ReservationRequest = {
      roomId: Number(roomId),
      userId: Number(userId),
      title,
      startDate: startDate ? toUtcIso(startDate) : '',
      endDate: endDate ? toUtcIso(endDate) : '',
      attendeeCount: Number(attendeeCount),
    };
    const errorMessage = validateReservation(request);
    if (errorMessage) {
      setValidationError(errorMessage);
      return;
    }

    setValidationError('');
    try {
      await onSubmit(request);
      setRoomId('');
      setUserId('');
      setTitle('');
      setStartDate('');
      setEndDate('');
      setAttendeeCount('1');
    } catch (requestError) {
      setValidationError(
        requestError instanceof Error
          ? requestError.message
          : 'The reservation could not be saved.',
      );
    }
  };

  const activeRooms = rooms.filter((room) => room.status === ROOM_STATUS.ACTIVE);

  return (
    <form onSubmit={(event) => void submit(event)}>
      <h2 className="text-lg font-bold text-slate-950 dark:text-white">Create a reservation</h2>
      <div className="mt-4 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <Field label="Room">
          <select
            className={inputClass}
            required
            value={roomId}
            onChange={(event) => setRoomId(event.target.value)}
          >
            <option value="">Select a room</option>
            {activeRooms.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name} · #{room.id}
              </option>
            ))}
          </select>
        </Field>
        <Field label="Organizer">
          <select
            className={inputClass}
            required
            value={userId}
            onChange={(event) => setUserId(event.target.value)}
          >
            <option value="">Select a person</option>
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.firstName} {user.lastName}
              </option>
            ))}
          </select>
        </Field>
        <Field label="Meeting title">
          <input
            className={inputClass}
            required
            maxLength={200}
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            placeholder="e.g. Product review"
          />
        </Field>
        <Field label="Start">
          <input
            className={inputClass}
            type="datetime-local"
            required
            value={startDate}
            onChange={(event) => setStartDate(event.target.value)}
          />
        </Field>
        <Field label="End">
          <input
            className={inputClass}
            type="datetime-local"
            required
            value={endDate}
            onChange={(event) => setEndDate(event.target.value)}
          />
        </Field>
        <Field label="Attendees">
          <input
            className={inputClass}
            type="number"
            min="1"
            required
            value={attendeeCount}
            onChange={(event) => setAttendeeCount(event.target.value)}
          />
        </Field>
      </div>
      {validationError ? (
        <p role="alert" className="mt-4 text-sm font-medium text-rose-700 dark:text-rose-300">
          {validationError}
        </p>
      ) : null}
      <button
        type="submit"
        disabled={isLoading}
        className="mt-5 rounded-lg bg-emerald-600 px-4 py-2 font-semibold text-white hover:bg-emerald-700 disabled:cursor-not-allowed disabled:opacity-50"
      >
        {isLoading ? 'Saving...' : 'Book room'}
      </button>
    </form>
  );
}
