import clinicksApi from '../api/clinicksApi';
// Servicio que centraliza todas las peticiones a la API relacionadas con los procesos médicos.
const procesoService = {
    // Envía los datos del formulario al backend para guardar un nuevo proceso en la base de datos (POST).
    crearProceso: async (procesoData) => {
        const respuesta = await clinicksApi.post('/procesos', procesoData);
        return respuesta.data;
    },

    // Recupera el listado completo de todos los procesos médicos registrados (GET).
    obtenerProcesos: async () => {
        const res = await clinicksApi.get('/procesos');
        return res.data;
    },

    // Recupera los tipos de procesos disponibles (GET).
    obtenerTiposProceso: async () => {
        const res = await clinicksApi.get('/procesos/tipos');
        return res.data;
    },
    // Busca y devuelve los detalles de un proceso específico utilizando su ID (GET).
    obtenerPorId: async (id) => {
        const res = await clinicksApi.get(`/procesos/${id}`);
        return res.data;
    }
};

export default procesoService;