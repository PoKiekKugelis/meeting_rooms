import { PAGE_SIZE } from '../constants/pagination';

interface PaginationProps {
  currentPage: number;
  pageSize?: number;
  totalItems: number;
  onPageChange: (page: number) => void;
}

export function Pagination({
  currentPage,
  pageSize = PAGE_SIZE,
  totalItems,
  onPageChange,
}: PaginationProps) {
  if (totalItems === 0) return null;

  const pageCount = Math.max(1, Math.ceil(totalItems / pageSize));
  const firstItem = (currentPage - 1) * pageSize + 1;
  const lastItem = Math.min(currentPage * pageSize, totalItems);

  return (
    <nav
      aria-label="Pagination"
      className="mt-5 flex flex-wrap items-center justify-between gap-3 text-sm"
    >
      <p className="text-slate-600 dark:text-slate-300">
        Showing{' '}
        <span className="font-semibold">
          {firstItem}–{lastItem}
        </span>{' '}
        of <span className="font-semibold">{totalItems}</span>
      </p>
      <div className="flex items-center gap-3">
        <button
          type="button"
          onClick={() => onPageChange(currentPage - 1)}
          disabled={currentPage <= 1}
          className="rounded-lg border border-slate-300 px-3 py-2 font-semibold text-slate-700 hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50 dark:border-slate-700 dark:text-slate-200 dark:hover:bg-slate-800"
        >
          Previous
        </button>
        <span aria-live="polite" className="whitespace-nowrap text-slate-600 dark:text-slate-300">
          Page {currentPage} of {pageCount}
        </span>
        <button
          type="button"
          onClick={() => onPageChange(currentPage + 1)}
          disabled={currentPage >= pageCount}
          className="rounded-lg border border-slate-300 px-3 py-2 font-semibold text-slate-700 hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-50 dark:border-slate-700 dark:text-slate-200 dark:hover:bg-slate-800"
        >
          Next
        </button>
      </div>
    </nav>
  );
}
