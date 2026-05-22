// src/hooks/usePatientHistory.js
import { useState, useEffect } from 'react';
import pacienteService from '../services/pacienteService';
import consultaService from '../services/consultaService';
import { extraerMensajeError } from '../utils/errorUtils';

/**
 * Hook para manejar la carga del paciente y su historial clínico.
 * SOC-1: Delega la obtención de datos a la API en lugar de usar datos estáticos en la vista.
 * 
 * @param {string|number} pacienteId - ID del paciente a consultar.
 */
export const usePatientHistory = (pacienteId) => {
    const [paciente, setPaciente] = useState(null);
    const [historial, setHistorial] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        let isMounted = true;

        const cargarDatos = async () => {
            if (!pacienteId) return;
            
            setCargando(true);
            setError(null);
            
            try {
                // Obtenemos los datos básicos del paciente y su historial de consultas en paralelo
                const [pacienteData, historialData] = await Promise.all([
                    pacienteService.obtenerPorId(pacienteId),
                    consultaService.obtenerHistorialPaciente(pacienteId)
                ]);

                if (isMounted) {
                    setPaciente(pacienteData);
                    setHistorial(historialData);
                }
            } catch (err) {
                if (isMounted) {
                    setError(extraerMensajeError(err, "No se pudo cargar el historial del paciente."));
                }
            } finally {
                if (isMounted) {
                    setCargando(false);
                }
            }
        };

        cargarDatos();

        return () => {
            isMounted = false;
        };
    }, [pacienteId]);

    return { paciente, historial, cargando, error };
};
