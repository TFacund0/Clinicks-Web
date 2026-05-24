// src/hooks/usePatientHistory.js
import { useState, useEffect } from 'react';
import pacienteService from '../services/pacienteService';
import consultaService from '../services/consultaService';
import procesoService from '../services/procesoService';
import { extraerMensajeError } from '../utils/errorUtils';

/**
 * Hook para manejar la carga del paciente y su historial clínico.
 * 
 * @param {string|number} pacienteId - ID del paciente a consultar.
 */
export const usePatientHistory = (pacienteId) => {
    const [paciente, setPaciente] = useState(null);
    const [historial, setHistorial] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [error, setError] = useState(null);

    // Estados de los filtros separados de la vista
    const [filtros, setFiltros] = useState({
        texto: '',
        mostrarConsultas: true,
        mostrarProcedimientos: true
    });

    useEffect(() => {
        let isMounted = true;

        const cargarDatos = async () => {
            if (!pacienteId) return;
            
            setCargando(true);
            setError(null);
            
            try {
                // Obtenemos los datos básicos del paciente y su historial (consultas y procesos) en paralelo
                const [pacienteData, consultasData, procesosData] = await Promise.all([
                    pacienteService.obtenerPorId(pacienteId),
                    consultaService.obtenerHistorialPaciente(pacienteId),
                    procesoService.obtenerHistorialPaciente(pacienteId)
                ]);

                if (isMounted) {
                    setPaciente(pacienteData);

                    // Etiquetar cada array para diferenciarlos en la UI
                    const consultasMapeadas = consultasData.map(c => ({
                        ...c,
                        tipoRegistro: 'consulta',
                        // Unificamos la propiedad de fecha para ordenar fácilmente
                        fechaOrden: new Date(c.fechaConsulta).getTime()
                    }));

                    const procesosMapeados = procesosData.map(p => ({
                        ...p,
                        tipoRegistro: 'procedimiento',
                        fechaOrden: new Date(p.fecha).getTime()
                    }));

                    // Unificar y ordenar cronológicamente (del más reciente al más antiguo)
                    const historialUnificado = [...consultasMapeadas, ...procesosMapeados]
                        .sort((a, b) => b.fechaOrden - a.fechaOrden);

                    setHistorial(historialUnificado);
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

    // Lógica de filtrado computada dinámicamente
    const historialFiltrado = historial.filter(item => {
        // 1. Filtro por tipo
        if (item.tipoRegistro === 'consulta' && !filtros.mostrarConsultas) return false;
        if (item.tipoRegistro === 'procedimiento' && !filtros.mostrarProcedimientos) return false;

        // 2. Filtro por texto libre
        if (filtros.texto.trim() === '') return true;
        
        const busqueda = filtros.texto.toLowerCase();
        
        // Unificar los textos donde el médico podría buscar, dependiendo de si es consulta o proceso
        let textoBuscable = '';
        if (item.tipoRegistro === 'consulta') {
            textoBuscable = `${item.motivo} ${item.diagnostico} ${item.tratamiento} ${item.observacion || ''} ${item.recomendacion || ''} ${item.medicoAtencion}`;
        } else {
            textoBuscable = `${item.tipo} ${item.descripcion} ${item.resultado} ${item.medicoAtencion}`;
        }

        return textoBuscable.toLowerCase().includes(busqueda);
    });

    return { 
        paciente, 
        historial: historialFiltrado, // Exponemos el array ya filtrado a la vista
        cargando, 
        error,
        filtros,
        setFiltros
    };
};
