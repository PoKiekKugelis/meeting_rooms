# Meeting Rooms

Meeting room reservation with ASP.NET Core API and a React frontend

## Structure

### Backend (`backend/`)

- `MeetingRooms.Api` — HTTP controllers, application startup, and background worker
- `MeetingRooms.Core` — application services, interfaces, DTOs, validation, policies, and mapping
- `MeetingRooms.Domain` — entities and domain rules
- `MeetingRooms.Infrastructure` — EF Core context, repositories, queries, and database seeding
- `MeetingRooms.Tests` — backend tests

### Frontend (`frontend/MeetingRooms/`)

- `src/pages` — routed Rooms, Reservations, and Users screens
- `src/components` — reusable UI and page-specific components
- `src/api` — HTTP client, endpoint calls, and query definitions
- `src/hooks` — data/mutation hooks and system theme behavior
- `src/constants`, `src/types`, `src/utils` — shared configuration, types, and helpers
