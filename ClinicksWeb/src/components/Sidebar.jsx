// src/components/Sidebar.jsx
import { NavLink } from 'react-router-dom';
import { LayoutDashboard, Users, ClipboardPlus, Activity, Calendar, User, LogOut } from 'lucide-react';

// Declaramos la función constante "Sidebar". Este componente es la barra lateral izquierda 
const Sidebar = () => {
  
  // "menuItems" es un arreglo (array) que guarda la configuración de cada botón del menú.
  // Guardarlo acá arriba hace que sea facilísimo agregar o quitar pantallas en el futuro 
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
      path: '/acceso-consulta' 
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

  // El "return" es la parte visual que React va a dibujar en la pantalla (HTML/Tailwind).
  // Todo lo que está adentro de este return es lo que el usuario realmente ve.
  return (
    // Contenedor principal de la barra (fondo oscuro, 64px de ancho fijo, ocupa toda la altura).
    <div className="w-64 bg-slate-900 h-screen text-slate-300 flex flex-col border-r border-slate-800 shrink-0">
      
      {/* Cabecera de la barra lateral con el logo y nombre "Clinicks" */}
      <div className="p-6 flex items-center gap-2 text-cyan-400 font-bold text-2xl border-b border-slate-800/50 mb-4">
        <Activity size={32} /> 
        <span>Clinicks</span>
      </div>
      
      {/* Contenedor de la lista de botones de navegación */}
      <nav className="flex-1 px-4 space-y-2">
        
        {/* Usamos el método .map() para recorrer el arreglo "menuItems".
            Por cada ítem en el arreglo, React "dibuja" un componente <NavLink>.*/}
        {menuItems.map((item, index) => (
          <NavLink 
            key={index} // React necesita una "key" única para saber qué elemento de la lista modificar si hay cambios
            to={item.path} // Le dice a React Router a qué URL ir cuando se hace clic
            
            // Función dinámica para los estilos: 
            // Si "isActive" es true (el médico está en esa pantalla), el botón se pinta de celeste.
            // Si es false, se queda oscuro.
            className={({ isActive }) => 
              `flex items-center gap-3 p-3 rounded-xl cursor-pointer transition-all duration-200 ${
                isActive 
                  ? 'bg-cyan-500 text-slate-950 font-bold shadow-lg shadow-cyan-500/20' 
                  : 'hover:bg-slate-800 text-slate-400 hover:text-slate-200' 
              }`
            }
          >
            {/* Adentro del botón, dibujamos el icono y el texto que definimos en el arreglo */}
            {item.icon}
            <span className="text-sm">{item.label}</span>
          </NavLink>
        ))}
      </nav>
    </div>
  );
};

// Exportamos el componente para poder usarlo en AppRoutes o en la vista principal
export default Sidebar;