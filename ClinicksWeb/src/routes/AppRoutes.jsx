import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../pages/auth/Login';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NewConsultation from '../pages/medico/NewConsultation';
import PatientHistory from '../pages/medico/PatientHistory'; 
import AccessConsultation from '../pages/medico/accessConsultation';
import NewProcess from '../pages/medico/NewProcess';

// Componente central que funciona como el "mapa" de nuestra aplicación.
// Conecta cada URL del navegador con la pantalla (componente) que debe dibujarse.
const AppRoutes = () => {
  const estaAutenticado = !!localStorage.getItem('token');

  return (
    <Routes>
      {/* Ruta de Login */}
      <Route path="/login" element={estaAutenticado ? <Navigate to="/dashboard" /> : <Login />} />
      
      {/* Rutas Protegidas */}
      <Route path="/dashboard" element={estaAutenticado ? <Dashboard /> : <Navigate to="/login" />} />
      <Route path="/pacientes" element={estaAutenticado ? <Patients /> : <Navigate to="/login" />} />
      <Route path="/pacientes/:id/historial" element={estaAutenticado ? <PatientHistory /> : <Navigate to="/login" />} />
      
      {/* Nuevas rutas protegidas del compañero */}
      <Route path="/nueva-consulta" element={estaAutenticado ? <NewConsultation /> : <Navigate to="/login" />} />
      <Route path="/nuevo-procedimiento" element={estaAutenticado ? <NewProcess /> : <Navigate to="/login" />} />
      <Route path="/acceso-consulta" element={estaAutenticado ? <AccessConsultation key="consulta" destino="/nueva-consulta" /> : <Navigate to="/login" />} />
      <Route path="/acceso-procedimiento" element={estaAutenticado ? <AccessConsultation key="proceso" destino="/nuevo-procedimiento" /> : <Navigate to="/login" />} />

      <Route path="/" element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} />
      <Route path="*" element={<Navigate to={estaAutenticado ? "/dashboard" : "/login"} />} />
    </Routes>
  );
};

export default AppRoutes;