// src/pages/auth/Login.jsx
// VUL-1 CORREGIDO: Ya no importa ni usa clinicksApi directamente.
// VUL-3 CORREGIDO: No escribe en localStorage. Delega todo al AuthContext que llama a authService.
// Esta página solo maneja el estado del formulario y la UI.

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Activity, Lock, User, AlertCircle } from 'lucide-react';
import { useAuth } from '../../context/AuthContext';

const Login = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [credentials, setCredentials] = useState({ username: '', password: '' });
  const navigate = useNavigate();
  const { login } = useAuth();

  const handleLogin = async (e) => {
    if (e) e.preventDefault();
    if (loading) return;

    setLoading(true);
    setError("");

    try {
      // La lógica de qué endpoint llamar y qué guardar en localStorage
      // la maneja authService a través del contexto. La página solo pide "iniciar sesión".
      await login(credentials.username, credentials.password);
      navigate('/dashboard', { replace: true });

    } catch (err) {
      if (err.code === 'ECONNABORTED') {
        setError("El servidor tarda demasiado en responder. Reintente en un momento.");
      } else {
        const mensajeError = err.response?.data?.message || err.response?.data?.mensaje || "Credenciales inválidas o servidor no disponible.";
        setError(mensajeError);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (field, value) => {
    setCredentials({ ...credentials, [field]: value });
    if (error) setError("");
  };

  return (
    <div className="h-screen w-full bg-slate-950 flex items-center justify-center p-4 font-sans text-slate-200">
      <div className="max-w-md w-full bg-slate-900 border border-slate-800 rounded-3xl p-10 shadow-2xl relative overflow-hidden">
        
        <div className="text-center mb-10">
          <div className="w-16 h-16 bg-cyan-500/10 border border-cyan-500/20 rounded-2xl flex items-center justify-center mx-auto mb-4 text-cyan-500">
            <Activity size={40} />
          </div>
          <h1 className="text-3xl font-black text-white tracking-tight">Clinicks<span className="text-cyan-500">Web</span></h1>
          <p className="text-slate-500 text-sm mt-2 font-medium">Gestión Hospitalaria Profesional</p>
        </div>

        {error && (
          <div className="mb-6 p-4 bg-red-500/10 border border-red-500/20 rounded-xl flex items-center gap-3 text-red-400 text-sm animate-pulse">
            <AlertCircle size={18} className="shrink-0" />
            <span className="font-medium">{error}</span>
          </div>
        )}

        <form onSubmit={handleLogin} className="space-y-6">
          <div>
            <label className="text-[10px] font-black uppercase text-slate-500 ml-1 mb-2 block tracking-widest">Usuario / Matrícula</label>
            <div className="relative group">
              <User className="absolute left-4 top-3.5 text-slate-600 group-focus-within:text-cyan-500 transition-colors" size={18} />
              <input 
                required
                className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3.5 pl-12 pr-4 text-slate-200 focus:border-cyan-500 outline-none transition-all placeholder:text-slate-700"
                placeholder="Ej: MN-12345"
                value={credentials.username}
                onChange={(e) => handleInputChange('username', e.target.value)}
              />
            </div>
          </div>

          <div>
            <label className="text-[10px] font-black uppercase text-slate-500 ml-1 mb-2 block tracking-widest">Contraseña</label>
            <div className="relative group">
              <Lock className="absolute left-4 top-3.5 text-slate-600 group-focus-within:text-cyan-500 transition-colors" size={18} />
              <input 
                type="password" required
                className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3.5 pl-12 pr-4 text-slate-200 focus:border-cyan-500 outline-none transition-all placeholder:text-slate-700"
                placeholder="••••••••"
                value={credentials.password}
                onChange={(e) => handleInputChange('password', e.target.value)}
              />
            </div>
          </div>

          <button 
            type="submit"
            disabled={loading}
            className={`w-full py-4 rounded-xl font-black transition-all shadow-lg flex items-center justify-center gap-2 tracking-widest active:scale-[0.98] ${loading
                ? 'bg-slate-800 text-slate-500 cursor-not-allowed border border-slate-700'
                : 'bg-cyan-500 hover:bg-cyan-400 text-slate-950 shadow-cyan-500/20'
              }`}
          >
            {loading ? "VERIFICANDO..." : "INICIAR SESIÓN"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Login;