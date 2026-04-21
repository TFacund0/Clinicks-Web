// src/services/pacienteService.js
import clinicksApi from '../api/clinicksApi';

export const pacienteService = {
    
    // Trae todos los pacientes
    obtenerTodos: async () => {
        try {
            const respuesta = await clinicksApi.get('/pacientes');
            return respuesta.data;
        } catch (error) {
            console.error(`Error al obtener todos los pacientes del médico`, error);
            throw error;
        }
    },

    // Trae solo los pacientes que atendió un médico específico
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