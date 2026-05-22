// src/api/clinicksApi.js
// REGRESIÓN CORREGIDA: El interceptor 401 ya no llama a localStorage.clear() ni a
// window.location.href directamente. En su lugar dispara un CustomEvent que el AuthContext
// escucha para actualizar el estado reactivo de autenticación de forma coordinada.
// Si usáramos localStorage.clear() aquí sin avisar al contexto, isAuthenticated quedaría
// en true mientras la sesión ya está destruida.

import axios from 'axios';

export const AUTH_EXPIRED_EVENT = 'auth:session-expired';

const clinicksApi = axios.create({
  // Vite lee .env.local si no existe usa el localhost por defecto.
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5056/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json'
  }
});

// Interceptor para agregar el token JWT a TODAS las peticiones
clinicksApi.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor de respuesta para detectar tokens expirados (401).
// En lugar de limpiar la sesión directamente aquí (lo que desincronizaría el AuthContext),
// emitimos un evento global que el AuthContext captura para hacer el logout de forma coordinada.
clinicksApi.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      if (!window.location.pathname.includes('/login')) {
        window.dispatchEvent(new CustomEvent(AUTH_EXPIRED_EVENT));
      }
    }
    return Promise.reject(error);
  }
);

export default clinicksApi;