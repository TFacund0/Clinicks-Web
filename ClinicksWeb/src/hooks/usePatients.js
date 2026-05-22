// src/hooks/usePatients.js
import { useState, useEffect, useRef } from 'react';
import { pacienteService } from '../services/pacienteService';
import { extraerMensajeError } from '../utils/errorUtils';

// Este Hook gestiona la carga y búsqueda de pacientes del médico logueado.
// El ID del médico ya no es necesario aquí porque el backend lo extrae del Token JWT.
export const usePatients = () => {

  // 1. ESTADOS
  const [pacientes, setPacientes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const isMounted = useRef(true);

  useEffect(() => {
    isMounted.current = true;
    return () => {
      isMounted.current = false;
    };
  }, []);

  // 2. FUNCIÓN DE CARGA (Conexión con la API)
  const fetchData = async () => {
    try {
      setCargando(true);
      setError(null);
      const datos = await pacienteService.obtenerAtendidosPorMedico();
      if (isMounted.current) {
        setPacientes(Array.isArray(datos) ? datos : []);
      }
    } catch (err) {
      if (isMounted.current) {
        setError(extraerMensajeError(err, "No se pudo cargar tu listado de pacientes."));
        setPacientes([]);
      }
    } finally {
      if (isMounted.current) setCargando(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // 3. LÓGICA DE FILTRADO EN TIEMPO REAL
  const pacientesFiltrados = (pacientes || []).filter((p) => {
    if (!p) return false;
    const busqueda = searchTerm.toLowerCase();
    const nombre = (p.nombreCompleto || "").toLowerCase();
    const dni = (p.dni || "").toString();
    return nombre.includes(busqueda) || dni.includes(busqueda);
  });

  // 4. LO QUE DEVUELVE MI HOOK
  return {
    pacientesFiltrados,
    cargando,
    error,
    searchTerm,
    setSearchTerm,
    refetch: fetchData // Exponemos la función de recarga por si queremos un botón de "Refresh"
  };
};