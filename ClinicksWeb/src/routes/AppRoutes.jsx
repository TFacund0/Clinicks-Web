import { Routes, Route } from 'react-router-dom';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Dashboard />} />
      <Route path="/pacientes" element={<Patients />} />
      {/* Aquí agregarás luego /consultas, /procedimientos, etc. */}
    </Routes>
  );
};

export default AppRoutes;