// src/hooks/usePatients.js
import { useState, useEffect } from 'react';
import { pacienteService } from '../services/pacienteService';

// Este Hook sirve como el "cerebro" para cargar y buscar pacientes. 
// Le pasamos el ID del médico logueado para que sepa de quién traer la lista.
export const usePatients = (medicoId) => {

  // 1. ESTADOS
  const [pacientes, setPacientes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");

  // 2. EFECTO DE CARGA (Conexión con la API)
  useEffect(() => {
    let montado = true;

    const fetchData = async () => {
      if (!medicoId) return;

      try {
        setCargando(true);
        const datos = await pacienteService.obtenerAtendidosPorMedico(medicoId);

        if (montado) {
          setPacientes(datos);
          setCargando(false);
        }
      } catch (err) {
        if (montado) {
          console.error("Error en usePatients:", err);
          setError("No se pudo cargar tu listado de pacientes.");
          setCargando(false);
        }
      }
    };

    fetchData();
    return () => { montado = false; };
  }, [medicoId]);

  // 3. LÓGICA DE FILTRADO EN TIEMPO REAL
  const pacientesFiltrados = pacientes.filter((p) => {
    const busqueda = searchTerm.toLowerCase();
    const nombre = p.nombreCompleto?.toLowerCase() || "";
    const dni = p.dni || "";
    return nombre.includes(busqueda) || dni.includes(busqueda);
  });

  // 4. LO QUE DEVUELVE MI HOOK
  return {
    pacientesFiltrados,
    cargando,
    error,
    searchTerm,
    setSearchTerm
  };
};