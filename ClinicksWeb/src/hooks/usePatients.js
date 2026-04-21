import { useState, useEffect } from 'react'; 
import { pacienteService } from '../services/pacienteService'; 

export const usePatients = (medicoId) => {
  // 1. ESTADOS
    const [pacientes, setPacientes] = useState([]);
    const [cargando, setCargando] = useState(true);
    const [error, setError] = useState(null);
    const [searchTerm, setSearchTerm] = useState("");

  // 2. EFECTO DE CARGA
useEffect(() => {
    let montado = true;
    
    const fetchData = async () => {
      // Si no hay ID de médico, no buscamos nada
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
  }, [medicoId]); // Se dispara si el ID del médico cambia

  // 3. LÓGICA DE FILTRADO (Se calcula en cada render)
    const pacientesFiltrados = pacientes.filter((p) => {
    const busqueda = searchTerm.toLowerCase();
    const nombre = p.nombreCompleto?.toLowerCase() || "";
    const dni = p.dni || "";
    return nombre.includes(busqueda) || dni.includes(busqueda);
});

  // 4. RETORNO
return {
    pacientesFiltrados,
    cargando,
    error,
    searchTerm,
    setSearchTerm // Exponemos esto para que el input pueda escribir
};
};