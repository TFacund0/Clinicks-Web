// src/services/agendaService.js
import clinicksApi from '../api/clinicksApi';

const agendaService = {
  /**
   * Obtiene la agenda de turnos del médico autenticado.
   * Opcionalmente filtra por fecha de inicio y fin.
   * 
   * @param {Date|string} fechaInicio - Filtro de fecha inicial (opcional)
   * @param {Date|string} fechaFin - Filtro de fecha final (opcional)
   * @returns {Promise<Array>} Lista de turnos
   */
  obtenerMisTurnos: async (fechaInicio = null, fechaFin = null) => {
    try {
      // Construimos los parámetros de la URL (Query String) si hay fechas
      const params = {};
      
      if (fechaInicio) {
        // Convertimos a string ISO si es un objeto Date
        params.fechaInicio = fechaInicio instanceof Date 
          ? fechaInicio.toISOString().split('T')[0] // Ej: "2026-05-24"
          : fechaInicio;
      }
      
      if (fechaFin) {
        params.fechaFin = fechaFin instanceof Date 
          ? fechaFin.toISOString().split('T')[0] 
          : fechaFin;
      }

      // Hacemos la petición GET a /api/Agenda/mis-turnos enviando los parámetros
      const response = await clinicksApi.get('/Agenda/mis-turnos', { params });
      
      // Nuestra API devuelve un objeto que contiene el array de turnos: { turnos: [...] }
      // (Si no había turnos, también trae un 'mensaje', pero siempre trae el array 'turnos')
      return response.data.turnos || [];
    } catch (error) {
      console.error('Error al obtener la agenda de turnos:', error);
      throw error;
    }
  }
};

export default agendaService;