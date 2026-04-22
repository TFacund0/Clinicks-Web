// src/hooks/usePatients.js
import { useState, useEffect } from 'react'; 
import { pacienteService } from '../services/pacienteService'; 

// Este Hook sirve como el "cerebro" para cargar y buscar pacientes. 
// Le pasamos el ID del médico logueado para que sepa de quién traer la lista.
export const usePatients = (medicoId) => {
    
  // 1. ESTADOS
  // Guardo la lista original e intocable de pacientes que me manda la base de datos.
    const [pacientes, setPacientes] = useState([]);
  // Aviso si estoy esperando que lleguen los datos (para mostrar un spinner o texto "Cargando...").
    const [cargando, setCargando] = useState(true);
  // Guardo un mensaje si el servidor se cae o falla la conexión.
    const [error, setError] = useState(null);
  // Guardo lo que el usuario va tipeando en la barrita de búsqueda (ej: "Juan" o "42333").
    const [searchTerm, setSearchTerm] = useState("");

  // 2. EFECTO DE CARGA (Conexión con la API)
useEffect(() => {
    // Esta variable 'montado' es un escudo anti-bugs. 
    // Evita que intente actualizar la pantalla si el médico cambió rápido de pestaña antes de que lleguen los datos.
    let montado = true;
    
    // Función asíncrona que va a buscar los datos a C#.
    const fetchData = async () => {
      // Corto la ejecución acá si por algún motivo no tengo el ID del médico.
    if (!medicoId) return;

    try {
        // Prendo el estado de carga antes de ir a buscar.
        setCargando(true);
        // Hago la llamada real usando mi servicio.
        const datos = await pacienteService.obtenerAtendidosPorMedico(medicoId);
        
        // Si el componente sigue vivo en pantalla, guardo los datos y apago la carga.
        if (montado) {
            setPacientes(datos);
            setCargando(false);
    }
} catch (err) {
        // Si C# tira error, lo atrapo acá y muestro un mensaje amigable.
        if (montado) {
            console.error("Error en usePatients:", err);
            setError("No se pudo cargar tu listado de pacientes.");
            setCargando(false);
        }
    }
};

    // Ejecuto la función que acabo de crear.
    fetchData();
    
    // Función de limpieza: si el componente se destruye, 'montado' pasa a false.
    return () => { montado = false; };
    
  // El [medicoId] al final significa: "Si el ID del médico cambia, volvé a ejecutar todo este useEffect de cero".
  }, [medicoId]); 

  // 3. LÓGICA DE FILTRADO EN TIEMPO REAL
  // Esta lista se recalcula a la velocidad de la luz cada vez que el usuario teclea una letra.
    const pacientesFiltrados = pacientes.filter((p) => {
    // Paso lo que tipeó el usuario a minúsculas para que no haya problemas si escribe "juan" o "Juan".
    const busqueda = searchTerm.toLowerCase();
    
    // Me aseguro de que el nombre y el DNI existan antes de intentar pasarlos a minúsculas.
    const nombre = p.nombreCompleto?.toLowerCase() || "";
    const dni = p.dni || "";
    
    // Si el nombre O el dni contienen lo que el usuario tipeó, lo dejo pasar al nuevo arreglo filtrado.
    return nombre.includes(busqueda) || dni.includes(busqueda);
});

  // 4. LO QUE DEVUELVE MI HOOK
  // Le mando a la vista solo lo que precisa para dibujar la pantalla.
return {
    pacientesFiltrados, // La lista ya recortada por la búsqueda (si no buscó nada, tiene todos).
    cargando,           // Para que sepa cuándo mostrar un "loading".
    error,              // Para que pinte un texto rojo si algo explotó.
    searchTerm,         // El valor actual de la barrita de búsqueda.
    setSearchTerm       // La función para que el input pueda actualizar el estado de la búsqueda.
};
};