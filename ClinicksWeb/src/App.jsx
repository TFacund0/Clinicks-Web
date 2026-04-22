// src/App.jsx (o App.js)
import { BrowserRouter as Router } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';

// 1. SIMULACIÓN DE LOGIN (Temporal)
// Inyectamos datos hardcodeados en la memoria del navegador para poder probar 
// el sistema (Dashboard, consultas, etc.) sin tener que armar la pantalla de Login todavía.
localStorage.setItem('medicoId', '1');
localStorage.setItem('medicoNombre', 'Dr. Perez');

// 2. COMPONENTE RAÍZ
// Este es el contenedor principal y punto de partida de toda tu aplicación en React.
function App() {
  return (
    // El <Router> envuelve toda la aplicación y es el "motor" que permite 
    // cambiar de pantallas manipulando la URL sin recargar la página web.
    <Router>
      {/* Adentro cargamos nuestro "mapa" que decide qué pantalla mostrar según la URL */}
      <AppRoutes />
    </Router>
  );
}

export default App;