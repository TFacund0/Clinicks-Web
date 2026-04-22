// src/api/clinicksApi.js
import axios from 'axios';

// Configura una instancia global de Axios para centralizar la URL de la API y los headers de todas las peticiones del Frontend.
const clinicksApi = axios.create({
<<<<<<< HEAD
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7135/api',
=======
  // Vite lee tu .env.local. Si no existe, usa el localhost por defecto.
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:5056/api',
>>>>>>> feature-ingreso-consulta
    headers: {
        'Content-Type': 'application/json'
    }
});

export default clinicksApi;