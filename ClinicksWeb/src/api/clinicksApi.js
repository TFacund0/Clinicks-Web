// src/api/clinicksApi.js
import axios from 'axios';

const clinicksApi = axios.create({
  // Vite lee tu .env.local. Si no existe, usa el localhost por defecto.
    baseURL: import.meta.env.VITE_API_URL || 'https://localhost:7135/api',
    headers: {
        'Content-Type': 'application/json'
    }
});

export default clinicksApi;