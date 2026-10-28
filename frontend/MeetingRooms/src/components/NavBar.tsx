import { NavLink } from 'react-router-dom';

export function NavBar() {
  const linkClass = ({ isActive }: { isActive: boolean }) =>
    `rounded-lg px-3 py-2 text-sm font-semibold transition ${isActive
      ? 'bg-emerald-100 text-emerald-800 dark:bg-emerald-950 dark:text-emerald-300'
      : 'text-slate-600 hover:bg-slate-100 hover:text-slate-950 dark:text-slate-300 dark:hover:bg-slate-800 dark:hover:text-white'
    }`;

  return (
    <header className="border-b border-slate-200 bg-white/90 backdrop-blur dark:border-slate-800 dark:bg-slate-950/90">
      <div className="mx-auto flex max-w-7xl items-center justify-between gap-6 px-4 py-4 sm:px-6 lg:px-8">
        <NavLink
          to="/rooms"
          className="text-lg font-black tracking-tight text-slate-950 dark:text-white"
        >
          Meeting <span className="text-emerald-600">Rooms</span>
        </NavLink>
        <nav aria-label="Main navigation" className="flex gap-1">
          <NavLink to="/rooms" className={linkClass}>
            Rooms
          </NavLink>
          <NavLink to="/users" className={linkClass}>
            Users
          </NavLink>
          <NavLink to="/reservations" className={linkClass}>
            Reservations
          </NavLink>
        </nav>
      </div>
    </header>
  );
}
