// src/components/Header.jsx

// Este componente representa la barra superior de la interfaz. 
// Su función principal es mostrar el contexto en el que se encuentra el médico (ej. en qué página está), 
// la hora actual (que se actualiza en tiempo real), y la información básica de su sesión.

import { useState, useEffect } from 'react';
import { Bell, Calendar as CalendarIcon, User, ChevronRight } from 'lucide-react';

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

  // Obtenemos el nombre del médico logueado. Como aún no tenemos sistema de Login conectado, si no encuentra un nombre, usa "Alex Carter" de prueba.
  const medicoNombre = localStorage.getItem('medicoNombre') || "Alex Carter";

  // Aquí formateamos la hora bruta de JavaScript a algo fácil de leer, como "08:23 AM".
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
            {medicoNombre.split(' ').map(n => n[0]).join('')}
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