export const API_ENDPOINTS = {
  rooms: '/api/rooms',
  userViews: '/api/users/views',
  reservations: '/api/reservations',

  reservationById: (id: number) => `/api/reservations/${id}`,
  completeReservation: (id: number) => `/api/reservations/${id}/complete`,
} as const;
