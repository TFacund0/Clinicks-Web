// src/components/PageLayout.jsx
import Sidebar from './Sidebar';
import Header from './Header';

/**
 * Estructura base de las páginas autenticadas.
 * DRY-2: Elimina la duplicación de los contenedores div, Sidebar, Header y la columna principal en cada vista.
 * 
 * @param {string} title - El título que se pasará al Header (ej. "Dashboard", "Nueva Consulta").
 * @param {ReactNode} children - El contenido específico de la página.
 */
const PageLayout = ({ title, children }) => {
    return (
        <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
            <Sidebar />
            <div className="flex-1 flex flex-col min-w-0">
                <Header paginaActual={title} />
                <main className="p-8 overflow-y-auto">
                    {children}
                </main>
            </div>
        </div>
    );
};

export default PageLayout;
