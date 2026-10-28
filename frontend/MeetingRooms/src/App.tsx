import { Navigate, Route, Routes } from 'react-router-dom';
import { ErrorBoundary } from './components/ErrorBoundary';
import { NavBar } from './components/NavBar';
import { RoomsPage } from './pages/RoomsPage';
import { ReservationsPage } from './pages/ReservationsPage';
import { UsersPage } from './pages/UsersPage';
import { useSystemTheme } from './hooks/useSystemTheme';

export default function App() {
  useSystemTheme();
  return (
    <ErrorBoundary>
      <NavBar />
      <Routes>
        <Route path="/rooms" element={<RoomsPage />} />
        <Route path="/users" element={<UsersPage />} />
        <Route path="/reservations" element={<ReservationsPage />} />
        <Route path="*" element={<Navigate to="/rooms" replace />} />
      </Routes>
    </ErrorBoundary>
  );
}
