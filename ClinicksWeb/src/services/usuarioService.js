// src/services/usuarioService.js
// Servicio centralizado de sesión y autenticación de usuarios.
// Es el ÚNICO lugar de la app autorizado a leer/escribir datos de sesión en localStorage
// y a llamar al endpoint de login. Ninguna página ni componente debe tocar localStorage directamente.

import clinicksApi from '../api/clinicksApi';

// Claves de almacenamiento definidas una sola vez.
// Si se renombra una clave, solo se cambia aquí.
const SESSION_KEYS = {
    TOKEN:            'token',
    MEDICO_ID:        'medicoId',
    MEDICO_NOMBRE:    'medicoNombre',
    MEDICO_MATRICULA: 'medicoMatricula',
};

const usuarioService = {

    /**
     * Autentica al médico/usuario contra el backend y persiste la sesión en localStorage.
     * @param {string} username - Nombre de usuario o matrícula.
     * @param {string} password - Contraseña en texto plano.
     * @returns {object} Datos del médico autenticado (token, nombre, etc.).
     */
    login: async (username, password) => {
        const response = await clinicksApi.post('/Usuarios/login', {
            username: username.trim(),
            password,
        });
        const data = response.data;

        // Limpiamos cualquier sesión anterior antes de guardar la nueva.
        localStorage.clear();
        localStorage.setItem(SESSION_KEYS.TOKEN,            data.token);
        localStorage.setItem(SESSION_KEYS.MEDICO_ID,        data.idMedico);
        localStorage.setItem(SESSION_KEYS.MEDICO_NOMBRE,    `${data.nombre} ${data.apellido}`);
        localStorage.setItem(SESSION_KEYS.MEDICO_MATRICULA, data.matricula);

        return data;
    },

    /**
     * Destruye la sesión activa limpiando el almacenamiento local.
     */
    logout: () => {
        localStorage.clear();
    },

    /**
     * Indica si hay una sesión activa (token presente).
     * @returns {boolean}
     */
    isAuthenticated: () => !!localStorage.getItem(SESSION_KEYS.TOKEN),

    // --- Getters de datos de sesión ---

    /** @returns {string|null} Token JWT del médico logueado. */
    getToken: () => localStorage.getItem(SESSION_KEYS.TOKEN),

    /** @returns {string} Nombre completo del médico logueado. */
    getMedicoNombre: () => localStorage.getItem(SESSION_KEYS.MEDICO_NOMBRE) || 'Médico',

    /** @returns {string|null} ID del médico logueado. */
    getMedicoId: () => localStorage.getItem(SESSION_KEYS.MEDICO_ID),

    /** @returns {string|null} Matrícula del médico logueado. */
    getMedicoMatricula: () => localStorage.getItem(SESSION_KEYS.MEDICO_MATRICULA),
};

export default usuarioService;
