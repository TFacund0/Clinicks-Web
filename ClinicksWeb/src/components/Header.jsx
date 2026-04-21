import { useState, useEffect } from 'react';
import { Bell, Calendar as CalendarIcon, User, ChevronRight } from 'lucide-react';

const Header = () => {
  const [currentTime, setCurrentTime] = useState(new Date());

  useEffect(() => {
    const timer = setInterval(() => setCurrentTime(new Date()), 1000);
    return () => clearInterval(timer);
  }, []);

  const medicoNombre = localStorage.getItem('medicoNombre') || "Dr. Alex Carter";

  const horaFormateada = currentTime.toLocaleTimeString('en-US', { 
    hour: '2-digit', 
    minute: '2-digit', 
    hour12: true 
  });

  const opcionesFecha = { weekday: 'short', day: 'numeric', month: 'short', year: 'numeric' };
  const fechaFormateada = currentTime.toLocaleDateString('es-AR', opcionesFecha);

  return (
    <header className="h-20 border-b border-slate-800 flex items-center justify-between px-8 bg-slate-950">
      
      {/* REEMPLAZO DEL BUSCADOR: Breadcrumbs / Ubicación actual */}
      <div className="flex items-center gap-2 text-sm font-medium">
        <span className="text-slate-500">Módulo Médico</span>
        <ChevronRight size={14} className="text-slate-600" />
        <span className="text-cyan-500 bg-cyan-500/10 px-3 py-1 rounded-full">
          Listado de Pacientes
        </span>
      </div>

      {/* Lado derecho: Se mantiene intacto en su posición original */}
      <div className="flex items-center gap-6">
        
        {/* Fecha y Hora */}
        <div className="text-right hidden md:block text-slate-400 text-sm">
          <div className="flex items-center gap-2 justify-end font-bold text-slate-200">
            {horaFormateada} <CalendarIcon size={14} />
          </div>
          <span className="capitalize">{fechaFormateada}</span>
        </div>

        {/* Notificaciones */}
        <div className="relative">
          <Bell size={20} className="text-slate-400 cursor-pointer hover:text-slate-200 transition-colors" />
          <span className="absolute -top-1 -right-1 w-2 h-2 bg-cyan-500 rounded-full border border-slate-950"></span>
        </div>

        {/* Perfil */}
        <div className="flex items-center gap-3 border-l border-slate-800 pl-6">
          <div className="w-10 h-10 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-cyan-500 text-xs font-bold shadow-inner">
            {medicoNombre.split(' ').map(n => n[0]).join('')}
          </div>
          <div className="text-sm">
            <p className="text-slate-200 font-bold">{medicoNombre}</p>
            <p className="text-slate-500 text-[10px] uppercase tracking-wider font-semibold">En línea</p>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;