import clinicksApi from '../api/clinicksApi';
// Servicio que centraliza todas las peticiones a la API relacionadas con los procesos médicos.
const procesoService = {
    // Envía los datos del formulario al backend para guardar un nuevo proceso en la base de datos (POST).
    crearProceso: async (procesoData) => {
        const respuesta = await clinicksApi.post('/procesos', procesoData);
        return respuesta.data;
    },

    // Recupera los tipos de procesos disponibles (GET).
    obtenerTiposProceso: async () => {
        const res = await clinicksApi.get('/procesos/tipos');
        return res.data;
    },

};

export default procesoService;