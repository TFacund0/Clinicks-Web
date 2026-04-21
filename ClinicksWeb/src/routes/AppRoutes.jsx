import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../pages/auth/Login';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import PatientHistory from '../pages/medico/PatientHistory';

const AppRoutes = () => {
  // Verificamos la sesión mirando el disco del navegador
  const estaAutenticado = !!localStorage.getItem('medicoId');

  return (
    <Routes>
      {/* RUTA LOGIN: Si ya tiene sesión, lo mandamos directo al dashboard */}
      <Route 
        path="/login" 
        element={estaAutenticado ? <Navigate to="/dashboard" /> : <Login />} 
      />

      {/* RUTAS PROTEGIDAS: Solo accesibles con sesión iniciada */}
      <Route 
        path="/dashboard" 
        element={estaAutenticado ? <Dashboard /> : <Navigate to="/login" />} 
      />
      
      <Route 
        path="/pacientes" 
        element={estaAutenticado ? <Patients /> : <Navigate to="/login" />} 
      />

      <Route 
        path="/pacientes/:id/historial" 
        element={estaAutenticado ? <PatientHistory /> : <Navigate to="/login" />} 
      />

      {/* REDIRECCIÓN INICIAL: Si entras a localhost:5173/ sin nada más */}
      <Route 
        path="/" 
        element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} 
      />
      
      {/* COMODÍN (Catch-all): Si escribe cualquier cosa mal, decidimos según la sesión */}
      <Route 
        path="*" 
        element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} 
      />
    </Routes>
  );
};

export default AppRoutes;