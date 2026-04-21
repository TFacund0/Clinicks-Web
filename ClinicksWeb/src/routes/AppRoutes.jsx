import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../pages/auth/Login';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import PatientHistory from '../pages/medico/PatientHistory';

const AppRoutes = () => {
  const estaAutenticado = !!localStorage.getItem('medicoId');

  return (
    <Routes>
      <Route path="/login" element={estaAutenticado ? <Navigate to="/dashboard" /> : <Login />} />
      <Route path="/dashboard" element={estaAutenticado ? <Dashboard /> : <Navigate to="/login" />} />
      <Route path="/pacientes" element={estaAutenticado ? <Patients /> : <Navigate to="/login" />} />
      <Route path="/pacientes/:id/historial" element={estaAutenticado ? <PatientHistory /> : <Navigate to="/login" />} />
      <Route path="/" element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} />
      <Route path="*" element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} />
    </Routes>
  );
};

export default AppRoutes;