// src/hooks/useDrawer.js
// Encapsula el estado de apertura/cierre del drawer de navegación móvil y sus
// efectos colaterales: cierre por Escape, cierre al navegar, cierre al cruzar
// a viewport de escritorio, y gestión de foco (entra al abrir, vuelve al cerrar).
import { useCallback, useEffect, useRef, useState } from 'react';
import { useLocation } from 'react-router-dom';

/**
 * @param {{ isDesktop: boolean }} params
 * @returns {{ isOpen: boolean, open: () => void, close: () => void, toggle: () => void, panelRef: React.RefObject, triggerRef: React.RefObject }}
 */
export const useDrawer = ({ isDesktop }) => {
  const [isOpen, setIsOpen] = useState(false);
  const panelRef = useRef(null);
  const triggerRef = useRef(null);
  const hasOpenedOnceRef = useRef(false);
  const location = useLocation();

  const close = useCallback(() => setIsOpen(false), []);
  const open = useCallback(() => setIsOpen(true), []);
  const toggle = useCallback(() => setIsOpen((prev) => !prev), []);

  // Cierra el drawer al cruzar hacia el breakpoint de escritorio.
  // Sincroniza el estado interno con una fuente externa (media query), por lo
  // que requiere un efecto; no hay forma de derivarlo en el render.
  useEffect(() => {
    if (isDesktop) {
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setIsOpen(false);
    }
  }, [isDesktop]);

  // Cierra el drawer al navegar a otra ruta.
  // Sincroniza el estado interno con una fuente externa (router), por lo que
  // requiere un efecto; no hay forma de derivarlo en el render.
  useEffect(() => {
    // eslint-disable-next-line react-hooks/set-state-in-effect
    setIsOpen(false);
  }, [location.pathname]);

  // Cierra el drawer al presionar Escape mientras está abierto.
  useEffect(() => {
    if (!isOpen) return undefined;

    const handleKeyDown = (event) => {
      if (event.key === 'Escape') {
        close();
      }
    };

    document.addEventListener('keydown', handleKeyDown, true);
    return () => document.removeEventListener('keydown', handleKeyDown, true);
  }, [isOpen, close]);

  // Mueve el foco hacia adentro del panel al abrir, y lo devuelve al trigger al cerrar.
  useEffect(() => {
    if (isOpen) {
      hasOpenedOnceRef.current = true;
      const focusableElement = panelRef.current?.querySelector(
        'a, button, input, select, textarea, [tabindex]:not([tabindex="-1"])'
      );
      (focusableElement ?? panelRef.current)?.focus();
    } else if (hasOpenedOnceRef.current) {
      // Solo devuelve el foco al trigger si el drawer ya se había abierto alguna vez,
      // evitando robar el foco en el montaje inicial.
      triggerRef.current?.focus();
    }
  }, [isOpen]);

  return { isOpen, open, close, toggle, panelRef, triggerRef };
};

export default useDrawer;
