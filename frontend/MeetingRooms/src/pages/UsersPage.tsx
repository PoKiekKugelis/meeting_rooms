import { useState } from 'react';
import { FeedbackState } from '../components/FeedbackState';
import { PageHeader } from '../components/PageHeader';
import { Pagination } from '../components/Pagination';
import { PAGE_SIZE } from '../constants/pagination';
import { useUsers } from '../hooks/useUsers';
import type { UserSummary } from '../types/models';

function UsersContent() {
  const { data, isLoading, error, refetch, isRefetching } = useUsers();
  const [page, setPage] = useState(1);

  if (isLoading) return <FeedbackState message="Loading users..." />;
  if (error && !data) {
    return (
      <FeedbackState
        message={error.message}
        actionLabel="Try again"
        onAction={() => void refetch()}
      />
    );
  }
  if (!data?.length) return <FeedbackState message="No users found." />;

  const pageCount = Math.max(1, Math.ceil(data.length / PAGE_SIZE));
  const currentPage = Math.min(page, pageCount);
  const paginatedUsers = data.slice((currentPage - 1) * PAGE_SIZE, currentPage * PAGE_SIZE);

  return (
    <>
      {error ? (
        <div
          role="alert"
          className="mb-4 flex items-center justify-between gap-4 rounded-xl border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-800 dark:border-rose-900 dark:bg-rose-950/50 dark:text-rose-200"
        >
          <span>{error.message} Showing the last successfully loaded users</span>
          <button
            type="button"
            onClick={() => void refetch()}
            className="shrink-0 font-semibold underline underline-offset-2"
          >
            Retry
          </button>
        </div>
      ) : null}
      {isRefetching ? (
        <p role="status" className="mb-3 text-sm text-slate-500 dark:text-slate-400">
          Updating users...
        </p>
      ) : null}
      <UsersTable users={paginatedUsers} />
      <Pagination
        currentPage={currentPage}
        pageSize={PAGE_SIZE}
        totalItems={data.length}
        onPageChange={setPage}
      />
    </>
  );
}

function UsersTable({ users }: { users: UserSummary[] }) {
  return (
    <div className="overflow-x-auto rounded-xl border border-slate-200 bg-white shadow-sm dark:border-slate-800 dark:bg-slate-900">
      <table className="w-full table-fixed border-collapse text-left text-sm">
        <caption className="sr-only">
          Meeting room users and their active reservation counts
        </caption>
        <colgroup>
          <col className="w-24" />
          <col />
          <col />
          <col className="w-48" />
        </colgroup>
        <thead className="bg-slate-50 text-xs uppercase tracking-wider text-slate-500 dark:bg-slate-800/70 dark:text-slate-300">
          <tr>
            <th scope="col" className="px-5 py-4 font-bold">
              ID
            </th>
            <th scope="col" className="px-5 py-4 font-bold">
              First name
            </th>
            <th scope="col" className="px-5 py-4 font-bold">
              Last name
            </th>
            <th scope="col" className="px-5 py-4 text-right font-bold">
              Active reservations
            </th>
          </tr>
        </thead>
        <tbody className="divide-y divide-slate-200 dark:divide-slate-800">
          {users.map((user) => (
            <tr key={user.id} className="text-slate-700 dark:text-slate-200">
              <th
                scope="row"
                className="whitespace-nowrap px-5 py-4 font-semibold text-slate-500 dark:text-slate-400"
              >
                #{user.id}
              </th>
              <td className="truncate px-5 py-4 font-medium text-slate-950 dark:text-white">
                {user.firstName}
              </td>
              <td className="truncate px-5 py-4 font-medium text-slate-950 dark:text-white">
                {user.lastName}
              </td>
              <td className="px-5 py-4 text-right font-semibold tabular-nums">
                {user.activeReservationsCount}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export function UsersPage() {
  return (
    <main className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <PageHeader
        eyebrow="People directory"
        title="Organizers"
        description="Meeting room users and their active reservation counts"
      />
      <UsersContent />
    </main>
  );
}
