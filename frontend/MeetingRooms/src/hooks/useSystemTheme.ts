import { useEffect } from 'react';

export function useSystemTheme() {
  useEffect(() => {
    const media = window.matchMedia('(prefers-color-scheme: dark)');
    const applyTheme = (event: MediaQueryList | MediaQueryListEvent) =>
      document.documentElement.classList.toggle('dark', event.matches);
    applyTheme(media);
    media.addEventListener('change', applyTheme);
    return () => media.removeEventListener('change', applyTheme);
  }, []);
}
