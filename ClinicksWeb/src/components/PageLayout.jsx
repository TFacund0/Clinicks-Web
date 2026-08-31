// src/components/PageLayout.jsx
import Sidebar from './Sidebar';
import Header from './Header';
import { useBreakpoint } from '../hooks/useBreakpoint';
import { useDrawer } from '../hooks/useDrawer';

/**
 * Estructura base de las páginas autenticadas.
 * DRY-2: Elimina la duplicación de los contenedores div, Sidebar, Header y la columna principal en cada vista.
 * Dueño del estado del drawer de navegación móvil (< lg): lo baja por props a Header y Sidebar, sin Context nuevo.
 *
 * @param {string} title - El título que se pasará al Header (ej. "Dashboard", "Nueva Consulta").
 * @param {ReactNode} children - El contenido específico de la página.
 */
const PageLayout = ({ title, children }) => {
    const isDesktop = useBreakpoint('lg');
    const { isOpen, toggle, close, panelRef, triggerRef } = useDrawer({ isDesktop });

    return (
        <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
            <Sidebar isOpen={isOpen} onClose={close} panelRef={panelRef} />
            <div className="flex-1 flex flex-col min-w-0">
                <Header
                    paginaActual={title}
                    isDrawerOpen={isOpen}
                    onToggleDrawer={toggle}
                    triggerRef={triggerRef}
                />
                <main className="flex-1 min-h-0 overflow-y-auto p-4 md:p-6 lg:p-8">
                    {children}
                </main>
            </div>
        </div>
    );
};

export default PageLayout;
