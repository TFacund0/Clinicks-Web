// src/services/consultaService.js
import clinicksApi from '../api/clinicksApi';

// Servicio que centraliza todas las peticiones a la API relacionadas con las consultas médicas.
const consultaService = {
    
    // Envía los datos del formulario al backend para guardar una nueva consulta en la base de datos (POST).
    crearConsulta: async (consultaData) => {
        try {
            const respuesta = await clinicksApi.post('/consultas', consultaData);
            return respuesta.data;
        } catch (error) {
            console.error("Error en consultaService.crearConsulta:", error.response?.data || error.message);
            throw error;
        }
    },

    // Recupera el listado completo de todas las consultas médicas registradas (GET).
    obtenerConsultas: async () => {
        const res = await clinicksApi.get('/consultas');
        return res.data;
    },

    // Busca y devuelve los detalles de una consulta específica utilizando su ID (GET).
    obtenerPorId: async (id) => {
        const res = await clinicksApi.get(`/consultas/${id}`);
        return res.data;
    }
};

export default consultaService;