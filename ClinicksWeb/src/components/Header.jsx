// src/components/Header.jsx
// VUL-3 CORREGIDO: Ya no lee medicoNombre directamente de localStorage.
// Ahora lo obtiene del AuthContext, que es la única fuente de verdad de la sesión.

import { useState, useEffect } from 'react';
import { Calendar as CalendarIcon, ChevronRight } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

// Se define el componente Header. Recibe una "prop" llamada 'paginaActual'.
// Si el componente padre (como Dashboard o Patients) no le pasa nada, por defecto dirá "Panel de Control".
const Header = ({ paginaActual = "Panel de Control" }) => {
  // Aquí guardamos el estado de la hora actual. Lo inicializamos con el momento exacto en que carga el componente.
  const [currentTime, setCurrentTime] = useState(new Date());

  // Usamos useEffect para arrancar un "reloj interno" cuando el Header aparece en pantalla.
  useEffect(() => {
    // Seteamos un intervalo para que cada 1 segundo (1000 milisegundos), actualice el estado 'currentTime' con la nueva hora.
    const timer = setInterval(() => {
      setCurrentTime(new Date());
    }, 1000);

    // Esta es una función de limpieza. Si el Header desaparece de la pantalla, detenemos el reloj para no gastar recursos del navegador inútilmente.
    return () => clearInterval(timer);
  }, []);

  // Obtenemos el nombre del médico logueado desde el contexto reactivo de autenticación.
  const { medicoNombre } = useAuth();

  // Generamos las iniciales de forma segura para evitar errores si el nombre es corto o nulo.
  const iniciales = medicoNombre
    .split(' ')
    .filter(n => n.length > 0)
    .map(n => n[0].toUpperCase())
    .slice(0, 2)
    .join('');
  const horaFormateada = currentTime.toLocaleTimeString('en-US', { 
    hour: '2-digit', 
    minute: '2-digit', 
    hour12: true 
  });

  // Hacemos lo mismo con la fecha, formateándola al estilo de Argentina (ej: "mié, 22 de abr de 2026").
  const opcionesFecha = { weekday: 'short', day: 'numeric', month: 'short', year: 'numeric' };
  const fechaFormateada = currentTime.toLocaleDateString('es-AR', opcionesFecha);

  // Todo esto es la parte visual (HTML/Tailwind) del Header.
  return (
    <header className="h-20 border-b border-slate-800 flex items-center justify-between px-8 bg-slate-950">
      
      {/* Las "Migas de pan" (Breadcrumbs) muestran dónde está el usuario. Aquí uso la prop 'paginaActual' para que el texto cambie dinámicamente. */}
      <div className="flex items-center gap-2 text-sm font-medium">
        <span className="text-slate-500">Módulo Médico</span>
        <ChevronRight size={14} className="text-slate-600" />
        <span className="text-cyan-500 bg-cyan-500/10 px-3 py-1 rounded-full">
          {paginaActual}
        </span>
      </div>

      <div className="flex items-center gap-6">
        
        {/* Aquí muestro el reloj en tiempo real y la fecha que formateamos arriba. */}
        <div className="text-right hidden md:block text-slate-400 text-sm">
          <div className="flex items-center gap-2 justify-end font-bold text-slate-200">
            {horaFormateada} <CalendarIcon size={14} />
          </div>
          <span className="capitalize">{fechaFormateada}</span>
        </div>

        {/* Esta sección muestra el perfil del médico. */}
        <div className="flex items-center gap-3 border-l border-slate-800 pl-6">
          
          {/* Un truquito visual: Tomo el nombre del médico (ej. "Alex Carter"), lo separo por espacios y extraigo la primera letra de cada parte para formar un "Avatar" de iniciales (ej. "AC"). */}
          <div className="w-10 h-10 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-cyan-500 text-xs font-bold shadow-inner">
            {iniciales}
          </div>

          <div className="text-sm">
            <p className="text-slate-200 font-bold">{medicoNombre}</p>
            <p className="text-slate-500 text-[10px] uppercase tracking-wider font-semibold">
              En línea
            </p>
          </div>
        </div>
      </div>

    </header>
  );
};

export default Header;