import clinicksApi from '../api/clinicksApi';

// Servicio que centraliza todas las peticiones a la API relacionadas con los procedimientos médicos.
const procedimientoService = {
    // Envía los datos del formulario al backend para guardar un nuevo procedimiento en la base de datos (POST).
    registrarProcedimiento: async (procedimiento) => {
        const respuesta = await clinicksApi.post('/procedimientos', procedimiento);
        return respuesta.data;
    },

    // Recupera los tipos de procedimientos disponibles (GET).
    obtenerTiposProcedimiento: async () => {
        const res = await clinicksApi.get('/procedimientos/tipos');
        return res.data;
    },
};

export default procedimientoService;
