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
    },

    // Llama al endpoint de búsqueda predictiva mandando los números que el médico escribe.
    buscarPorDni: async (dni) => {
        try {
            const respuesta = await clinicksApi.get(`/pacientes/buscar?dni=${dni}`);
            return respuesta.data; // Devuelve la lista de 5 coincidencias.
        } catch (error) {
            console.error("Error al buscar sugerencias de DNI:", error);
            return [];
        }
    }
};