// src/routes/AppRoutes.jsx
// VUL-2 CORREGIDO: El estado de autenticación ahora proviene del AuthContext (reactivo),
// no de una lectura puntual de localStorage al momento del render.
// Ahora si el usuario hace login o logout, las rutas se redirigen automáticamente.

import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import ProtectedRoute from './ProtectedRoute';
import Login from '../pages/auth/Login';
import Dashboard from '../pages/medico/Dashboard';
import Patients from '../pages/medico/Patients';
import NewConsultation from '../pages/medico/NewConsultation';
import PatientHistory from '../pages/medico/PatientHistory';
import AccessConsultation from '../pages/medico/accessConsultation';
import NewProcess from '../pages/medico/NewProcess';

const AppRoutes = () => {
  const { isAuthenticated } = useAuth();

  return (
    <Routes>
      {/* Rutas Públicas */}
      <Route path="/login" element={isAuthenticated ? <Navigate to="/dashboard" /> : <Login />} />

      {/* Rutas Protegidas (Agrupadas) */}
      <Route element={<ProtectedRoute />}>
        <Route path="/dashboard"              element={<Dashboard />} />
        <Route path="/pacientes"              element={<Patients />} />
        <Route path="/pacientes/:id/historial" element={<PatientHistory />} />
        <Route path="/nueva-consulta"         element={<NewConsultation />} />
        <Route path="/nuevo-procedimiento"    element={<NewProcess />} />
        <Route path="/acceso-consulta"        element={<AccessConsultation key="consulta"  destino="/nueva-consulta" />} />
        <Route path="/acceso-procedimiento"   element={<AccessConsultation key="proceso"   destino="/nuevo-procedimiento" />} />
      </Route>

      {/* Redirecciones por defecto */}
      <Route path="/"  element={<Navigate to={isAuthenticated ? "/dashboard" : "/login"} />} />
      <Route path="*"  element={<Navigate to={isAuthenticated ? "/dashboard" : "/login"} />} />
    </Routes>
  );
};

export default AppRoutes;