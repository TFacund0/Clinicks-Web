// src/services/consultaService.js
import clinicksApi from '../api/clinicksApi';

// Servicio que centraliza todas las peticiones a la API relacionadas con las consultas médicas.
const consultaService = {

    // Envía los datos del formulario al backend para guardar una nueva consulta en la base de datos (POST).
    registrarConsulta: async (consulta) => {
        const respuesta = await clinicksApi.post('/consultas', consulta);
        return respuesta.data;
    },

    // Recupera el historial clínico (consultas) de un paciente específico.
    obtenerHistorialPaciente: async (pacienteId) => {
        const respuesta = await clinicksApi.get(`/consultas/historial/${pacienteId}`);
        return respuesta.data;
    }
};

export default consultaService;