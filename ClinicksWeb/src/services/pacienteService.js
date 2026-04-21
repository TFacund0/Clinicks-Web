// src/services/pacienteService.js
import clinicksApi from '../api/clinicksApi';

export const pacienteService = {
    
    // Trae todos los pacientes
    obtenerTodos: async () => {
        const respuesta = await clinicksApi.get('/pacientes');
        return respuesta.data;
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