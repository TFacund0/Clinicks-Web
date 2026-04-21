/**
 * COMPONENTE: Header
 * PROPÓSITO: Barra superior dinámica que gestiona el reloj en tiempo real, 
 * la ubicación contextual (breadcrumbs) y la sesión del médico.
 * * @param {string} paginaActual - Recibe el nombre de la vista desde el padre (Dashboard, Listado, etc.)
 */

import { useState, useEffect } from 'react';
import { Bell, Calendar as CalendarIcon, User, ChevronRight } from 'lucide-react';

const Header = ({ paginaActual = "Panel de Control" }) => {
  // 1. ESTADO DEL RELOJ
  // Usamos un objeto Date completo para tener acceso a hora, minutos y segundos.
  const [currentTime, setCurrentTime] = useState(new Date());

  // 2. CICLO DE VIDA (RELOJ EN VIVO)
  useEffect(() => {
    // Seteamos un intervalo que refresca el estado cada 1 segundo (1000ms).
    // Esto hace que el componente se re-renderice y el reloj se mueva solo.
    const timer = setInterval(() => {
      setCurrentTime(new Date());
    }, 1000);

    // FUNCIÓN DE LIMPIEZA: Si el médico cierra la página o cambia de sección,
    // matamos el proceso del reloj para que no consuma memoria innecesaria.
    return () => clearInterval(timer);
  }, []);

  // 3. DATOS DEL MÉDICO (SESIÓN)
  // Buscamos el nombre en el "disco" del navegador. Si no hay login, usamos un nombre de prueba.
  const medicoNombre = localStorage.getItem('medicoNombre') || "Alex Carter";

  // 4. FORMATEO DE HORA (AM/PM)
  // Convertimos el objeto Date a un string legible tipo "11:30 AM".
  const horaFormateada = currentTime.toLocaleTimeString('en-US', { 
    hour: '2-digit', 
    minute: '2-digit', 
    hour12: true 
  });

  // 5. FORMATEO DE FECHA (Local)
  // Usamos el estándar de Argentina para que diga "lun, 20 de abr de 2024".
  const opcionesFecha = { weekday: 'short', day: 'numeric', month: 'short', year: 'numeric' };
  const fechaFormateada = currentTime.toLocaleDateString('es-AR', opcionesFecha);

  return (
    <header className="h-20 border-b border-slate-800 flex items-center justify-between px-8 bg-slate-950">
      
      {/* SECCIÓN IZQUIERDA: BREADCRUMBS (MIGAS DE PAN)
          Ahora es DINÁMICO: muestra el nombre de la página que le pase el componente padre. */}
      <div className="flex items-center gap-2 text-sm font-medium">
        <span className="text-slate-500">Módulo Médico</span>
        <ChevronRight size={14} className="text-slate-600" />
        <span className="text-cyan-500 bg-cyan-500/10 px-3 py-1 rounded-full">
          {paginaActual}
        </span>
      </div>

      <div className="flex items-center gap-6">
        
        {/* SECCIÓN CENTRAL: RELOJ Y CALENDARIO
            Renderiza la hora y la fecha formateadas previamente. */}
        <div className="text-right hidden md:block text-slate-400 text-sm">
          <div className="flex items-center gap-2 justify-end font-bold text-slate-200">
            {horaFormateada} <CalendarIcon size={14} />
          </div>
          <span className="capitalize">{fechaFormateada}</span>
        </div>

        {/* SECCIÓN DERECHA: PERFIL Y ESTADO
            Incluye la lógica de las iniciales y el indicador de "En línea". */}
        <div className="flex items-center gap-3 border-l border-slate-800 pl-6">
          
          {/* AVATAR GENERATIVO: 
              Mapea el nombre del médico y toma solo las primeras letras de cada palabra. 
              Ej: "Alex Carter" -> "AC" */}
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