import { BrowserRouter as Router } from 'react-router-dom';
import AppRoutes from './routes/AppRoutes';
// Simulamos que el login guardó estos datos en el navegador
localStorage.setItem('medicoId', '2');
localStorage.setItem('medicoNombre', 'Dr. Perez');

function App() {
  return (
    <Router>
      <AppRoutes />
    </Router>
  );
}

export default App;