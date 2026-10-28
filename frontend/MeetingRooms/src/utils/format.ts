export function formatDateTime(value: string | null): string {
  if (!value) return '—';

  return new Intl.DateTimeFormat(undefined, {
    dateStyle: 'medium',
    timeStyle: 'short',
  }).format(new Date(value));
}

export function toUtcIso(value: string): string {
  return new Date(value).toISOString();
}
