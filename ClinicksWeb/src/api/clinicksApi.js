// src/api/clinicksApi.js
import axios from 'axios';

// Configura una instancia global de Axios para centralizar la URL de la API y los headers de todas las peticiones del Frontend.
const clinicksApi = axios.create({
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7135/api',
    headers: {
        'Content-Type': 'application/json'
    }
});

export default clinicksApi;