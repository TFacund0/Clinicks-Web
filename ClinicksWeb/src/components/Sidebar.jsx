import { NavLink } from 'react-router-dom';
import { LayoutDashboard, Users, ClipboardPlus, Activity, Calendar, LogOut } from 'lucide-react';

const Sidebar = () => {
  
  const handleLogout = () => {
    // 1. Limpiamos la sesión del navegador
    localStorage.removeItem('medicoId');
    localStorage.removeItem('medicoNombre');
    
    // 2. IMPORTANTE: Forzamos redirección total para "limpiar" el AppRoutes
    window.location.href = '/login';
  };

  const menuItems = [
    { icon: <LayoutDashboard size={20} />, label: 'Dashboard', path: '/dashboard' },
    { icon: <Users size={20} />, label: 'Pacientes', path: '/pacientes' },
    { icon: <ClipboardPlus size={20} />, label: 'Nueva Consulta', path: '/consulta' },
    { icon: <Activity size={20} />, label: 'Nuevo Procedimiento', path: '/procedimiento' },
    { icon: <Calendar size={20} />, label: 'Mi Agenda', path: '/agenda' },
  ];

  return (
    <div className="w-64 bg-slate-900 h-screen text-slate-300 flex flex-col border-r border-slate-800 shrink-0">
      
      {/* LOGO */}
      <div className="p-6 flex items-center gap-2 text-cyan-400 font-bold text-2xl border-b border-slate-800/50 mb-4">
        <Activity size={32} /> <span>Clinicks</span>
      </div>
      
      {/* NAVEGACIÓN */}
      <nav className="flex-1 px-4 space-y-2">
        {menuItems.map((item, index) => (
          <NavLink 
            key={index} to={item.path}
            className={({ isActive }) => 
              `flex items-center gap-3 p-3 rounded-xl transition-all duration-200 ${
                isActive 
                  ? 'bg-cyan-500 text-slate-950 font-bold shadow-lg shadow-cyan-500/20' 
                  : 'hover:bg-slate-800 text-slate-400 hover:text-slate-200'
              }`
            }
          >
            {item.icon} <span className="text-sm">{item.label}</span>
          </NavLink>
        ))}
      </nav>

      {/* BOTÓN CERRAR SESIÓN (mt-auto lo empuja abajo) */}
      <div className="p-4 border-t border-slate-800 mt-auto">
        <button 
          onClick={handleLogout}
          className="w-full flex items-center gap-3 p-3 rounded-xl text-red-400 hover:bg-red-500/10 transition-all duration-200 group"
        >
          <LogOut size={20} className="group-hover:-translate-x-1 transition-transform" />
          <span className="text-sm font-bold">Cerrar Sesión</span>
        </button>
      </div>
    </div>
  );
};

export default Sidebar;