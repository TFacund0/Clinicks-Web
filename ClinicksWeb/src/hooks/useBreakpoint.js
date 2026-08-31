// src/hooks/useBreakpoint.js
// Envuelve useMediaQuery con los valores min-width de los breakpoints por defecto de Tailwind 4.
// Fases posteriores deben importar BREAKPOINTS en vez de redeclarar valores en px.
import { useMediaQuery } from './useMediaQuery';

export const BREAKPOINTS = Object.freeze({
  sm: 640,
  md: 768,
  lg: 1024,
  xl: 1280,
  '2xl': 1536,
});

/**
 * Devuelve true cuando el viewport está en o por encima del breakpoint nombrado.
 * @param {'sm'|'md'|'lg'|'xl'|'2xl'} name
 */
export const useBreakpoint = (name) => useMediaQuery(`(min-width: ${BREAKPOINTS[name]}px)`);

export default useBreakpoint;
