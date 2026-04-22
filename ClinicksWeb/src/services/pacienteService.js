// src/services/pacienteService.js
import clinicksApi from '../api/clinicksApi';

// Servicio que concentra y maneja todas las peticiones a la API relacionadas con los pacientes.
export const pacienteService = {
    
    // Pide al servidor el listado completo de todos los pacientes registrados en el sistema (GET).
    obtenerTodos: async () => {
        try {
            const respuesta = await clinicksApi.get('/pacientes');
            return respuesta.data;
        } catch (error) {
            console.error(`Error al obtener todos los pacientes del médico`, error);
            throw error;
        }
    },

    // Busca exclusivamente a los pacientes que ya fueron atendidos por el médico actual usando su ID (GET).
    obtenerAtendidosPorMedico: async (medicoId) => {
        try {
            const respuesta = await clinicksApi.get(`/pacientes/atendidos/${medicoId}`);
            return respuesta.data;
        } catch (error) {
            console.error(`Error al obtener pacientes del médico ${medicoId}:`, error);
            throw error;
        }
    }
};