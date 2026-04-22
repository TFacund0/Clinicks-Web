// src/routes/AppRoutes.jsx (o donde lo tengas guardado)
import { Routes, Route, Navigate } from 'react-router-dom';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NewConsultation from '../pages/medico/NewConsultation';
import PatientHistory from '../pages/medico/PatientHistory'; 
import AccessConsultation from '../pages/medico/accessConsultation';

// Componente central que funciona como el "mapa" de nuestra aplicación.
// Conecta cada URL del navegador con la pantalla (componente) que debe dibujarse.
const AppRoutes = () => {
  return (
    <Routes>
      {/* Rutas principales que apuntan a la pantalla de inicio */}
      <Route path="/" element={<Dashboard />} />
      <Route path="/dashboard" element={<Dashboard />} />
      
      {/* Ruta para el listado general de pacientes */}
      <Route path="/pacientes" element={<Patients />} />
      
      {/* Ruta para el formulario de atención médica */}
      <Route path="/nueva-consulta" element={<NewConsultation />} />

      <Route path="/acceso-consulta" element={<AccessConsultation />} />
  
      
      {/* Ruta dinámica: El ":id" es una variable en la URL que nos permite saber el historial de qué paciente buscar (ej. /pacientes/5/historial) */}
      <Route path="/pacientes/:id/historial" element={<PatientHistory />} />

      {/* Ruta salvavidas: Si el usuario escribe a mano una URL que no existe, lo redirige automáticamente al inicio */}
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
};

export default AppRoutes;