/* eslint-disable react-refresh/only-export-components */
// src/context/AuthContext.jsx
// Contexto global de autenticación.
// Resuelve VUL-2: el estado de autenticación ahora es reactivo (useState de React),
// por lo que cualquier cambio de login/logout actualiza automáticamente toda la UI
// sin necesidad de recargar la página ni depender de localStorage directamente.

import { createContext, useContext, useState, useCallback, useEffect } from 'react';
import usuarioService from '../services/usuarioService';
import { AUTH_EXPIRED_EVENT } from '../api/clinicksApi';

const AuthContext = createContext(null);

/**
 * Proveedor que envuelve la aplicación y expone el estado de sesión de forma reactiva.
 * Debe colocarse dentro del <Router> para que los componentes hijos puedan usar useNavigate.
 */
export const AuthProvider = ({ children }) => {
    // Estado derivado de localStorage al arrancar: si ya había token, el usuario sigue logueado.
    const [isAuthenticated, setIsAuthenticated] = useState(usuarioService.isAuthenticated());
    const [medicoNombre, setMedicoNombre] = useState(usuarioService.getMedicoNombre());

    /**
     * Inicia sesión: llama al servicio, persiste la sesión y actualiza el estado de React.
     * @returns {object} Datos del médico autenticado.
     */
    const login = useCallback(async (username, password) => {
        const data = await usuarioService.login(username, password);
        setIsAuthenticated(true);
        setMedicoNombre(`${data.nombre} ${data.apellido}`);
        return data;
    }, []);

    /**
     * Cierra la sesión: limpia el storage y pone el estado en "no autenticado".
     */
    const logout = useCallback(() => {
        usuarioService.logout();
        setIsAuthenticated(false);
        setMedicoNombre('Médico');
    }, []);

    // Escucha el evento que emite el interceptor de Axios cuando el backend devuelve 401.
    // Esto garantiza que si el token expira mid-sesión, el estado de React se actualice
    // y el usuario sea redirigido al login sin que la app quede en un estado inconsistente.
    useEffect(() => {
        const handleSessionExpired = () => logout();
        window.addEventListener(AUTH_EXPIRED_EVENT, handleSessionExpired);
        return () => window.removeEventListener(AUTH_EXPIRED_EVENT, handleSessionExpired);
    }, [logout]);

    return (
        <AuthContext.Provider value={{ isAuthenticated, medicoNombre, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

/**
 * Hook para consumir el contexto de autenticación en cualquier componente.
 * Lanza un error claro si se usa fuera del AuthProvider.
 */
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth debe usarse dentro de un <AuthProvider>.');
    }
    return context;
};
