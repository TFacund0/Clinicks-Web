// 1. IMPORTACIÓN DE LIBRERÍAS
import { Routes, Route, Navigate } from 'react-router-dom';

// 2. IMPORTACIÓN DE PÁGINAS
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NuevaConsulta from '../pages/medico/NuevaConsulta';
import PatientHistory from '../pages/medico/PatientHistory'; 

const AppRoutes = () => {
  return (
    <Routes>
      {/* Muestra el Dashboard al inicio */}
      <Route path="/" element={<Dashboard />} />
      
      {/* Listado de pacientes */}
      <Route path="/pacientes" element={<Patients />} />

      <Route path="/NuevaConsulta" element={<NuevaConsulta />} />
      {/* Historial clínico dinámico */}
      <Route path="/pacientes/:id/historial" element={<PatientHistory />} />

      {/* Catch-all: Si la ruta no existe, redirigir al Dashboard */}
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
};

export default AppRoutes;