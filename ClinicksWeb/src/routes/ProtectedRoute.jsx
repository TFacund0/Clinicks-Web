import { Navigate, Outlet } from 'react-router-dom';

/**
 * Componente que protege las rutas de la aplicación.
 * Si el usuario no tiene un token de sesión válido, lo redirige al Login.
 * Si está autenticado, renderiza los componentes hijos (Outlet).
 */
const ProtectedRoute = () => {
    const estaAutenticado = !!localStorage.getItem('token');

    if (!estaAutenticado) {
        // Guardamos la intención de navegación para poder volver después del login si fuera necesario
        return <Navigate to="/login" replace />;
    }

    // El Outlet representa a cualquier componente hijo definido en el mapa de rutas
    return <Outlet />;
};

export default ProtectedRoute;
