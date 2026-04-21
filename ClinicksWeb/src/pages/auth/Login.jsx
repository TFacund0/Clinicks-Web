import { useState } from 'react';
import { Activity, Lock, User, AlertCircle } from 'lucide-react';
import clinicksApi from '../../api/clinicksApi'; 

const Login = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [credentials, setCredentials] = useState({ username: '', password: '' });

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      // Usamos tu instancia personalizada. 
      // La URL final será: baseURL + '/Auth/login'
      const response = await clinicksApi.post('/Auth/login', {
        username: credentials.username,
        password: credentials.password
      });

      const data = response.data;
      
      // Guardamos la info real del médico que viene de C#
      localStorage.setItem('medicoId', data.idMedico);
      localStorage.setItem('medicoNombre', `${data.nombre} ${data.apellido}`);
      localStorage.setItem('medicoMatricula', data.matricula);
      
      // Redirección con recarga para que AppRoutes valide la sesión
      window.location.href = '/dashboard';

    } catch (err) {
      console.error("Error en login:", err);
      // Capturamos el mensaje de error que configuramos en el Backend
      const mensajeError = err.response?.data?.message || "Error de conexión con la API";
      setError(mensajeError);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="h-screen w-full bg-slate-950 flex items-center justify-center p-4 font-sans">
      <div className="max-w-md w-full bg-slate-900 border border-slate-800 rounded-3xl p-10 shadow-2xl relative overflow-hidden">
        
        <div className="text-center mb-10">
          <div className="w-16 h-16 bg-cyan-500/10 border border-cyan-500/20 rounded-2xl flex items-center justify-center mx-auto mb-4 text-cyan-500">
            <Activity size={40} />
          </div>
          <h1 className="text-3xl font-black text-white tracking-tight">Clinicks<span className="text-cyan-500">Web</span></h1>
          <p className="text-slate-500 text-sm mt-2">Gestión Hospitalaria Profesional</p>
        </div>

        {error && (
          <div className="mb-6 p-4 bg-red-500/10 border border-red-500/20 rounded-xl flex items-center gap-3 text-red-400 text-sm">
            <AlertCircle size={18} />
            <span>{error}</span>
          </div>
        )}

        <form onSubmit={handleLogin} className="space-y-6">
          <div>
            <label className="text-xs font-black uppercase text-slate-500 ml-1 mb-2 block">Usuario / Matrícula</label>
            <div className="relative">
              <User className="absolute left-4 top-3.5 text-slate-500" size={18} />
              <input 
                required
                className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3.5 pl-12 pr-4 text-slate-300 focus:border-cyan-500 outline-none transition-all"
                placeholder="Ej: MN-12345"
                value={credentials.username}
                onChange={(e) => setCredentials({...credentials, username: e.target.value})}
              />
            </div>
          </div>

          <div>
            <label className="text-xs font-black uppercase text-slate-500 ml-1 mb-2 block">Contraseña</label>
            <div className="relative">
              <Lock className="absolute left-4 top-3.5 text-slate-500" size={18} />
              <input 
                type="password" required
                className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3.5 pl-12 pr-4 text-slate-300 focus:border-cyan-500 outline-none transition-all"
                placeholder="••••••••"
                value={credentials.password}
                onChange={(e) => setCredentials({...credentials, password: e.target.value})}
              />
            </div>
          </div>

          <button 
            type="submit" disabled={loading}
            className="w-full bg-cyan-500 hover:bg-cyan-400 text-slate-950 font-black py-4 rounded-xl transition-all shadow-lg flex items-center justify-center gap-2"
          >
            {loading ? "VERIFICANDO..." : "INICIAR SESIÓN"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;