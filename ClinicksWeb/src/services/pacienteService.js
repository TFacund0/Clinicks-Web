// src/services/pacienteService.js
import clinicksApi from '../api/clinicksApi';

export const pacienteService = {
    
    // Función para traer todos los pacientes
    obtenerTodos: async () => {
        const respuesta = await clinicksApi.get('/pacientes');
        return respuesta.data; // Devolvemos solo los datos (el JSON)
    },

    // Aquí podrías agregar en el futuro:
  // obtenerPorId: async (id) => { ... },
  // crear: async (nuevoPaciente) => { ... },
};