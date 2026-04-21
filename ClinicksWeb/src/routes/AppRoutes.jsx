import { Routes, Route, Navigate } from 'react-router-dom';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NewConsultation from '../pages/medico/NewConsultation';
import PatientHistory from '../pages/medico/PatientHistory'; 

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Dashboard />} />
      <Route path="/dashboard" element={<Dashboard />} />
      
      <Route path="/pacientes" element={<Patients />} />
      
      {/* EL FIX ESTÁ ACÁ: El path debe ser idéntico al del Sidebar */}
      <Route path="/nueva-consulta" element={<NewConsultation />} />
      
      <Route path="/pacientes/:id/historial" element={<PatientHistory />} />

      {/* Si no encuentra la ruta, vuelve al inicio */}
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
};

export default AppRoutes;