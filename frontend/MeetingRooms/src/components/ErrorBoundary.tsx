import { ErrorBoundary as ReactErrorBoundary, type FallbackProps } from 'react-error-boundary';
import type { PropsWithChildren } from 'react';

function ErrorFallback({ resetErrorBoundary }: FallbackProps) {
  return (
    <main className="mx-auto max-w-3xl px-4 py-16 text-center">
      <div className="rounded-2xl border border-rose-200 bg-rose-50 p-8 dark:border-rose-900 dark:bg-rose-950/50">
        <h1 className="text-xl font-bold text-rose-900 dark:text-rose-100">
          Something went wrong
        </h1>
        <p className="mt-2 text-rose-700 dark:text-rose-200">
          The page could not be rendered. Please try again
        </p>
        <button
          type="button"
          onClick={resetErrorBoundary}
          className="mt-6 rounded-lg bg-rose-600 px-4 py-2 font-semibold text-white hover:bg-rose-700"
        >
          Try again
        </button>
      </div>
    </main>
  );
}

export function ErrorBoundary({ children }: PropsWithChildren) {
  return <ReactErrorBoundary FallbackComponent={ErrorFallback}>{children}</ReactErrorBoundary>;
}
