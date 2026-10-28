import { API_BASE_URL } from '../constants/config';

export class ApiError extends Error {
  readonly status: number;
  readonly code?: string;

  constructor(message: string, status: number, code?: string) {
    super(message);
    this.name = 'ApiError';
    this.status = status;
    this.code = code;
  }
}

type QueryValue = string | number | boolean | undefined | null;
type QueryEntry = QueryValue | readonly QueryValue[];

interface ApiErrorDetails {
  message: string;
  code?: string;
}

export interface RequestOptions extends RequestInit {
  query?: Record<string, QueryEntry>;
}

function buildUrl(path: string, query?: RequestOptions['query']): URL {
  const url = new URL(`${API_BASE_URL}${path}`);

  for (const [key, entry] of Object.entries(query ?? {})) {
    const values = Array.isArray(entry) ? entry : [entry];

    for (const value of values) {
      if (value !== undefined && value !== null && value !== '') {
        url.searchParams.append(key, String(value));
      }
    }
  }

  return url;
}

function getErrorDetails(body: unknown, fallbackMessage: string): ApiErrorDetails {
  if (typeof body === 'object' && body !== null) {
    const candidate = body as { message?: unknown; code?: unknown };
    return {
      message: typeof candidate.message === 'string' ? candidate.message : fallbackMessage,
      code: typeof candidate.code === 'string' ? candidate.code : undefined,
    };
  }

  return { message: fallbackMessage };
}

async function getApiError(response: Response): Promise<ApiError> {
  let body: unknown;

  try {
    body = await response.json();
  } catch {
    body = undefined;
  }

  const fallbackMessage = response.statusText || 'The request could not be completed.';
  const details = getErrorDetails(body, fallbackMessage);

  return new ApiError(details.message || fallbackMessage, response.status, details.code);
}

export async function request<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { query, ...init } = options;
  const headers = new Headers(init.headers);

  if (init.body != null && !headers.has('Content-Type')) {
    headers.set('Content-Type', 'application/json');
  }

  const response = await fetch(buildUrl(path, query), { ...init, headers });
  if (!response.ok) {
    throw await getApiError(response);
  }

  if (response.status === 204) return undefined as T;
  return response.json() as Promise<T>;
}
