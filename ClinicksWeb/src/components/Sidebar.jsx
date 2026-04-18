import { NavLink } from 'react-router-dom';
import { LayoutDashboard, Users, ClipboardPlus, Activity, Calendar, User, LogOut } from 'lucide-react';

const Sidebar = () => {
  const menuItems = [
    { icon: <LayoutDashboard size={20} />, label: 'Dashboard', path: '/' },
    { icon: <Users size={20} />, label: 'Pacientes', path: '/pacientes' },
    { icon: <ClipboardPlus size={20} />, label: 'Nueva Consulta', path: '/consulta' },
    { icon: <Activity size={20} />, label: 'Nuevo Procedimiento', path: '/procedimiento' },
    { icon: <Calendar size={20} />, label: 'Mi Agenda', path: '/agenda' },
  ];

  return (
    <div className="w-64 bg-slate-900 h-screen text-slate-300 flex flex-col border-r border-slate-800 flex-shrink-0">
      <div className="p-6 flex items-center gap-2 text-cyan-400 font-bold text-2xl border-b border-slate-800/50 mb-4">
        <Activity size={32} /> <span>Clinicks</span>
      </div>
      
      <nav className="flex-1 px-4 space-y-2">
        {menuItems.map((item, index) => (
          <NavLink 
            key={index} 
            to={item.path}
            className={({ isActive }) => 
              `flex items-center gap-3 p-3 rounded-xl cursor-pointer transition-all duration-200 ${
                isActive ? 'bg-cyan-500 text-slate-950 font-bold shadow-lg shadow-cyan-500/20' : 'hover:bg-slate-800 text-slate-400 hover:text-slate-200'
              }`
            }
          >
            {item.icon}
            <span className="text-sm">{item.label}</span>
          </NavLink>
        ))}
      </nav>
      {/* ... Botones de Perfil y Salir ... */}
    </div>
  );
};

export default Sidebar;