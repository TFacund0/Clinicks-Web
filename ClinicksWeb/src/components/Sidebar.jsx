
// Importamos NavLink para manejar la navegación sin recargar la página
import { NavLink } from 'react-router-dom';

// Importamos los iconos de la librería Lucide para la interfaz visual
import { LayoutDashboard, Users, ClipboardPlus, Activity, Calendar, User, LogOut } from 'lucide-react';

//Componente Sidebar: Representa el menú lateral de navegación.
const Sidebar = () => {
  
  // Definimos un arreglo de objetos para centralizar la configuración del menú.
  const menuItems = [
    { 
      icon: <LayoutDashboard size={20} />, 
      label: 'Dashboard', 
      path: '/' // Ruta raíz
    },
    { 
      icon: <Users size={20} />, 
      label: 'Pacientes', 
      path: '/pacientes' 
    },
    { 
      icon: <ClipboardPlus size={20} />, 
      label: 'Nueva Consulta', 
      path: '/NuevaConsulta' 
    },
    { 
      icon: <Activity size={20} />, 
      label: 'Nuevo Procedimiento', 
      path: '/procedimiento' 
    },
    { 
      icon: <Calendar size={20} />, 
      label: 'Mi Agenda', 
      path: '/agenda' 
    },
  ];

  return (
    <div className="w-64 bg-slate-900 h-screen text-slate-300 flex flex-col border-r border-slate-800 shrink-0">
      
      {/* Sección del Logo y nombre del sistema */}
      <div className="p-6 flex items-center gap-2 text-cyan-400 font-bold text-2xl border-b border-slate-800/50 mb-4">
        <Activity size={32} /> 
        <span>Clinicks</span>
      </div>
      
      {/* Navegación dinámica */}
      <nav className="flex-1 px-4 space-y-2">
        
        {/* Usamos .map para recorrer el arreglo y no repetir código manualmente */}
        {menuItems.map((item, index) => (
          <NavLink 
            key={index} // Key requerida por React para identificar elementos en listas
            to={item.path} // Hacia dónde apunta el enlace

            className={({ isActive }) => 
              `flex items-center gap-3 p-3 rounded-xl cursor-pointer transition-all duration-200 ${
                isActive 
                  ? 'bg-cyan-500 text-slate-950 font-bold shadow-lg shadow-cyan-500/20' // Estilo Activo
                  : 'hover:bg-slate-800 text-slate-400 hover:text-slate-200' // Estilo Inactivo / Hover
              }`
            }
          >
            {/* Renderizamos el icono y la etiqueta del item */}
            {item.icon}
            <span className="text-sm">{item.label}</span>
          </NavLink>
        ))}
      </nav>

    </div>
  );
};

export default Sidebar;