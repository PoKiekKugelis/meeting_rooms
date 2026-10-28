interface FeedbackStateProps {
  message: string;
  actionLabel?: string;
  onAction?: () => void;
}

export function FeedbackState({ message, actionLabel, onAction }: FeedbackStateProps) {
  return (
    <div
      role="status"
      className="rounded-2xl border border-slate-200 bg-white p-10 text-center shadow-sm dark:border-slate-800 dark:bg-slate-900"
    >
      <p className="text-slate-600 dark:text-slate-300">{message}</p>
      {actionLabel && onAction ? (
        <button
          type="button"
          onClick={onAction}
          className="mt-5 rounded-lg bg-emerald-600 px-4 py-2 font-semibold text-white hover:bg-emerald-700"
        >
          {actionLabel}
        </button>
      ) : null}
    </div>
  );
}
