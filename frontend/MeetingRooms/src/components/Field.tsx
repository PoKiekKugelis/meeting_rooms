import type { LabelHTMLAttributes } from 'react';

interface FieldProps extends LabelHTMLAttributes<HTMLLabelElement> {
  label: string;
}

export function Field({ label, children, className = '', ...props }: FieldProps) {
  return (
    <label
      {...props}
      className={`block text-sm font-semibold text-slate-700 dark:text-slate-200 ${className}`}
    >
      {label}
      <span className="mt-1 block">{children}</span>
    </label>
  );
}

export const inputClass =
  'w-full rounded-lg border border-slate-300 bg-white px-3 py-2 text-slate-900 shadow-sm placeholder:text-slate-400 dark:border-slate-700 dark:bg-slate-950 dark:text-slate-100 dark:placeholder:text-slate-500';
