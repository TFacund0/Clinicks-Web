// src/api/clinicksApi.js
import axios from 'axios';

const clinicksApi = axios.create({
  // Vite lee tu .env.local. Si no existe, usa el localhost por defecto.
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

// Interceptor de respuesta para detectar tokens expirados o accesos no autorizados (401)
clinicksApi.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      if (!window.location.pathname.includes('/login')) {
        localStorage.clear();
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export default clinicksApi;