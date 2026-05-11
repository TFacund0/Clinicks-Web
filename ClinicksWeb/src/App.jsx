// src/App.jsx (o App.js)
import { BrowserRouter as Router } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';

// COMPONENTE RAÍZ
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