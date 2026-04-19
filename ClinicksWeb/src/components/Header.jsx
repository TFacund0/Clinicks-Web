// Importamos los iconos necesarios de la librería Lucide para la parte visual
import { Search, Bell, Calendar as CalendarIcon } from 'lucide-react';

//Componente Header: Representa la barra superior de la interfaz.
 
const Header = () => {
  return (
    <header className="h-20 border-b border-slate-800 flex items-center justify-between px-8 bg-slate-950">

      {/* Contenedor del Buscador de Pacientes */}
      <div className="flex items-center gap-4 bg-slate-900 px-4 py-2 rounded-xl border border-slate-800 w-96">
        
        {/* Icono de lupa de Lucide */}
        <Search size={18} className="text-slate-500" />
        
        <input 
          type="text" 
          placeholder="Buscar paciente..." 
          className="bg-transparent border-none outline-none text-slate-300 w-full" 
        />
      </div>

      {/* Lado derecho del Header: Fecha, Notificaciones y Perfil */}
      <div className="flex items-center gap-6">

        {/* Sección de Fecha y Hora */}
        <div className="text-right hidden md:block text-slate-400 text-sm">
          <div className="flex items-center gap-2 justify-end font-bold text-slate-200">
            11:00 AM <CalendarIcon size={14} />
          </div>
          <span>Mon, 18 Oct 2023</span>
        </div>

        {/* Icono de Notificaciones (Campana) */}
        <Bell size={20} className="text-slate-400 cursor-pointer" />

        {/* Información del Usuario (Médico): Simboliza el contexto del médico logueado */}
        <div className="flex items-center gap-3 border-l border-slate-800 pl-6">
          {/* Imagen de perfil */}
          <img 
            src="https://i.pravatar.cc/150?u=dr" 
            alt="Profile" 
            className="w-10 h-10 rounded-full border-2 border-cyan-500" 
          />
          
          <div className="text-sm">
            {/* Nombre y Especialidad del profesional */}
            <p className="text-slate-200 font-bold">Dr. Alex Carter</p>
            <p className="text-slate-500 text-xs">Médico Clínico</p>
          </div>
        </div>

      </div>

    </header>
  );
};

export default Header;