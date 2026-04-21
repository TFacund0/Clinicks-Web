// IMPORTACIÓN DE LIBRERÍAS DE ENRUTAMIENTO
// Routes: Actúa como un contenedor que evalúa todas las rutas definidas.
// Route: Define una relación específica entre una URL (path) y un Componente (element).
import { Routes, Route } from 'react-router-dom';

// IMPORTACIÓN DE PÁGINAS (Vistas principales)
// Importamos los componentes de nivel de página para la interfaz del médico.
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NuevaConsulta from '../pages/medico/NuevaConsulta';


// Componente AppRoutes: Centraliza la lógica de navegación de la aplicación.
const AppRoutes = () => {
  return (
    // El componente <Routes> analiza la URL actual y renderiza 
    <Routes>

      {/* RUTA RAÍZ: Muestra el Dashboard principal cuando el médico ingresa al sistema */}
      <Route path="/" element={<Dashboard />} />
      
      {/* RUTA DE PACIENTES: Vinculada al path '/pacientes' definido en el Sidebar */}
      <Route path="/pacientes" element={<Patients />} />

      <Route path="/NuevaConsulta" element={<NuevaConsulta />} />
    </Routes>
  );
};

export default AppRoutes;