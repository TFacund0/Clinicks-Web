// src/utils/errorUtils.js

/**
 * Extrae el mensaje de error de un error lanzado por Axios.
 * DRY-3: Evita repetir la misma lógica de encadenamiento opcional en todos los catch.
 * 
 * @param {Error} error - El error capturado en el bloque catch.
 * @param {string} mensajePorDefecto - Mensaje a devolver si no se encuentra uno específico del backend.
 * @returns {string} - El mensaje de error final a mostrar al usuario.
 */
export const extraerMensajeError = (error, mensajePorDefecto = "Ocurrió un error inesperado.") => {
    // Si la petición fue abortada por timeout (configurado en clinicksApi)
    if (error.code === 'ECONNABORTED') {
        return "El servidor tarda demasiado en responder. Reintente en un momento.";
    }

    // Retorna el mensaje del backend (message o mensaje) o el mensaje por defecto
    return error.response?.data?.message || error.response?.data?.mensaje || mensajePorDefecto;
};
