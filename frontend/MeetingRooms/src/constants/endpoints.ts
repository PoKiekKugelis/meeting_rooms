export const API_ENDPOINTS = {
  rooms: '/api/rooms',
  users: '/api/users',
  reservations: '/api/reservations',

  reservationById: (id: number) => `/api/reservations/${id}`,
  completeReservation: (id: number) => `/api/reservations/${id}/complete`,
} as const;
