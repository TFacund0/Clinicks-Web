// src/services/pacienteService.js
import clinicksApi from '../api/clinicksApi';

// Servicio que concentra y maneja todas las peticiones a la API relacionadas con los pacientes.
export const pacienteService = {
    
    // Pide al servidor el listado completo de todos los pacientes registrados en el sistema (GET).
    obtenerTodos: async () => {
        const respuesta = await clinicksApi.get('/pacientes');
        return respuesta.data;
    },

    obtenerAtendidosPorMedico: async () => {
        const respuesta = await clinicksApi.get('/pacientes/atendidos');
        return respuesta.data;
    },

    validarPacientePorDni: async (dni) => {
        const respuesta = await clinicksApi.get(`/pacientes/validar/${dni}`);
        return respuesta.data;
    }
};
export default pacienteService;