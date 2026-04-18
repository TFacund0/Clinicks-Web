import { Routes, Route } from 'react-router-dom';
import Dashboard from '../pages/Dashboard';
import Patients from '../pages/Patients';

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