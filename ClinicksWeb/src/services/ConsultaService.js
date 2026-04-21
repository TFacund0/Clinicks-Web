import clinicksApi from '../api/clinicksApi';

const consultaService = {
    crearConsulta: async (consultaData) => {
        try {
            // clinicksApi ya tiene la baseURL (ej: localhost:7135/api)
            const respuesta = await clinicksApi.post('/consultas', consultaData);
            return respuesta.data;
        } catch (error) {
            console.error("Error en consultaService.crearConsulta:", error.response?.data || error.message);
            throw error;
        }
    },

    obtenerConsultas: async () => {
        const res = await clinicksApi.get('/consultas');
        return res.data;
    },

    obtenerPorId: async (id) => {
        const res = await clinicksApi.get(`/consultas/${id}`);
        return res.data;
    }
};

export default consultaService;