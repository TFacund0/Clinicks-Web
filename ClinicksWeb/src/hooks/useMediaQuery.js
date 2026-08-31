// src/hooks/useMediaQuery.js
// Hook SSR-safe que suscribe un componente al estado de una media query CSS.
// Usa useSyncExternalStore para una lectura sin desgarros (tearing) en React 19.
import { useSyncExternalStore } from 'react';

const isBrowserSupported = () =>
  typeof window !== 'undefined' && typeof window.matchMedia === 'function';

/**
 * Devuelve true/false según si la media query dada matchea el viewport actual.
 * @param {string} query - una media query CSS válida (ej. "(min-width: 1024px)")
 */
export const useMediaQuery = (query) => {
  const subscribe = (callback) => {
    if (!isBrowserSupported()) {
      return () => {};
    }

    const mediaQueryList = window.matchMedia(query);
    mediaQueryList.addEventListener('change', callback);
    return () => mediaQueryList.removeEventListener('change', callback);
  };

  const getSnapshot = () => {
    if (!isBrowserSupported()) {
      return false;
    }
    return window.matchMedia(query).matches;
  };

  const getServerSnapshot = () => false;

  return useSyncExternalStore(subscribe, getSnapshot, getServerSnapshot);
};

export default useMediaQuery;
