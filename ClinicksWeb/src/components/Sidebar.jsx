// src/components/Sidebar.jsx
// SOC-3 CORREGIDO: Sidebar ya no ejecuta lógica de autenticación directamente.
// El logout se delega a useAuth(), que internamente llama a authService.logout().
import { NavLink, useNavigate } from 'react-router-dom';
import { LayoutDashboard, Users, ClipboardPlus, Activity, Calendar, LogOut } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

/**
 * @param {boolean} isOpen - estado del drawer (< lg), controlado por PageLayout.
 * @param {() => void} onClose - cierra el drawer (overlay click, click en NavLink).
 * @param {React.RefObject} panelRef - ref al panel para gestión de foco desde useDrawer.
 */
const Sidebar = ({ isOpen, onClose, panelRef }) => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    // Delega la destrucción de la sesión al authService (a través del contexto).
    logout();
    // El Context ya marcó isAuthenticated=false; AppRoutes redirigirá al login automáticamente.
    navigate('/login', { replace: true });
  };

  const menuItems = [
    { icon: <LayoutDashboard size={20} />, label: 'Dashboard', path: '/dashboard' },
    { icon: <Calendar size={20} />, label: 'Mi Agenda', path: '/agenda' },
    { icon: <Users size={20} />, label: 'Pacientes', path: '/pacientes' },
    { icon: <ClipboardPlus size={20} />, label: 'Nueva Consulta', path: '/acceso-consulta' },
    { icon: <Activity size={20} />, label: 'Nuevo Procedimiento', path: '/acceso-procedimiento' }
  ];

  return (
    <>
      {/* Overlay de fondo, solo visible debajo de lg cuando el drawer está abierto */}
      {isOpen && (
        <div
          className="fixed inset-0 z-30 bg-slate-950/60 lg:hidden"
          onClick={onClose}
          aria-hidden="true"
        />
      )}

      <div
        id="app-sidebar"
        role="navigation"
        ref={panelRef}
        aria-hidden={!isOpen ? 'true' : undefined}
        inert={!isOpen ? '' : undefined}
        className={`w-64 bg-slate-900 h-screen text-slate-300 flex flex-col border-r border-slate-800 shrink-0
          fixed inset-y-0 left-0 z-40 transition-transform duration-200
          ${isOpen ? 'translate-x-0' : '-translate-x-full'}
          lg:static lg:translate-x-0 lg:z-auto`}
      >

        {/* Cabecera de la barra lateral con el logo y nombre "Clinicks" */}
        <div className="p-6 flex items-center gap-2 text-cyan-400 font-bold text-2xl border-b border-slate-800/50 mb-4">
          <Activity size={32} /> <span>Clinicks</span>
        </div>

        {/* Contenedor de la lista de botones de navegación */}
        <nav className="flex-1 px-4 space-y-2">
          {menuItems.map((item, index) => (
            <NavLink
              key={index}
              to={item.path}
              onClick={onClose}
              className={({ isActive }) =>
                `flex items-center gap-3 p-3 rounded-xl transition-all duration-200 ${isActive
                  ? 'bg-cyan-500 text-slate-950 font-bold shadow-lg shadow-cyan-500/20'
                  : 'hover:bg-slate-800 text-slate-400 hover:text-slate-200'
                }`
              }
            >
              {item.icon} <span className="text-sm">{item.label}</span>
            </NavLink>
          ))}
        </nav>

        {/* BOTÓN CERRAR SESIÓN */}
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
    </>
  );
};

export default Sidebar;