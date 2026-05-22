// src/App.jsx
import { BrowserRouter as Router } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
import { AuthProvider } from './context/AuthContext';

// COMPONENTE RAÍZ
// El <AuthProvider> se coloca dentro del <Router> para que los componentes del contexto
// (como Sidebar al hacer logout) puedan usar hooks de react-router como useNavigate.
function App() {
  return (
    <Router>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </Router>
  );
}

export default App;