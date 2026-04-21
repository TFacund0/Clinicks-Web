    import axios from 'axios';

    //const API_URL = import.meta.env.VITE_API_URL + '/consultas';
    const API_URL = 'http://localhost:5056/api/consultas';
    const consultaService = {
        /**
         * Envía una nueva consulta médica al Backend
         * @param {Object} consultaData - Los datos del formulario
         */
        crearConsulta: async (consultaData) => {
    try {
            const dataLimpia = {
                ...consultaData,
                fechaconsulta: consultaData.fechaconsulta
                ? consultaData.fechaconsulta
                : null
            };
                const respuesta = await axios.post(API_URL, dataLimpia);
                return respuesta.data;
            } catch (error) {
                console.error(
                "Error en consultaService.crearConsulta:",
                error.response?.data || error.message
            );
            throw error;
            }
        },

        obtenerConsultas: async () => {
            const res = await axios.get(API_URL);
            return res.data;
        },

        obtenerPorId: async (id) => {
            const res = await axios.get(`${API_URL}/${id}`);
            return res.data;
        },
    };

    export default consultaService;