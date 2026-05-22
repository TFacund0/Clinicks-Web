// src/routes/ProtectedRoute.jsx
// Protege las rutas privadas consultando el AuthContext reactivo.
// Si el usuario no tiene sesión activa, lo redirige al Login.

import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const ProtectedRoute = () => {
    const { isAuthenticated } = useAuth();

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    // El Outlet representa a cualquier componente hijo definido en el mapa de rutas
    return <Outlet />;
};

export default ProtectedRoute;
