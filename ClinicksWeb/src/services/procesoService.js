import clinicksApi from '../api/clinicksApi';
// Servicio que centraliza todas las peticiones a la API relacionadas con los procesos médicos.
const procesoService = {
    // Envía los datos del formulario al backend para guardar un nuevo proceso en la base de datos (POST).
    registrarProcedimiento: async (procedimientoData) => {
        const respuesta = await clinicksApi.post('/procesos', procedimientoData);
        return respuesta.data;
    },

    // Recupera los tipos de procesos disponibles (GET).
    obtenerTiposProceso: async () => {
        const res = await clinicksApi.get('/procesos/tipos');
        return res.data;
    },

    // Recupera el historial clínico de procedimientos de un paciente específico.
    obtenerHistorialPaciente: async (pacienteId) => {
        const respuesta = await clinicksApi.get(`/procesos/historial/${pacienteId}`);
        return respuesta.data;
    }
};

export default procesoService;