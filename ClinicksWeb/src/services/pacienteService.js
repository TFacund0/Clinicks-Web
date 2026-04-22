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
    },

    validarPacientePorDni: async (dni) => {
        try {
            // Mandamos el DNI como parámetro de ruta o query según tu API
            const respuesta = await clinicksApi.get(`/pacientes/validar/${dni}`);
            return respuesta.data; // Debería devolver { success: true, mensaje: "..." }
        } catch (error) {
            console.error("Error al validar paciente:", error.response?.data || error.message);
            throw error;
        }
    }
};
export default pacienteService;