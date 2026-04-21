import { useState } from 'react';
import { Activity, Lock, User, AlertCircle } from 'lucide-react';

const Login = () => {
  const [loading, setLoading] = useState(false);
  const [credentials, setCredentials] = useState({ username: '', password: '' });

  const handleLogin = (e) => {
    e.preventDefault();
    setLoading(true);

    // Simulación de validación (Luego aquí irá tu axios.post)
    setTimeout(() => {
      localStorage.setItem('medicoId', '1');
      localStorage.setItem('medicoNombre', 'Alex Carter');
      
      setLoading(false);
      
      // IMPORTANTE: Forzamos la recarga para que AppRoutes lea la nueva sesión
      window.location.href = '/dashboard'; 
    }, 1200);
  };

  return (
    <div className="h-screen w-full bg-slate-950 flex items-center justify-center p-4 font-sans">
      <div className="max-w-md w-full bg-slate-900 border border-slate-800 rounded-3xl p-10 shadow-2xl relative overflow-hidden">
        
        <div className="absolute top-0 right-0 w-32 h-32 bg-cyan-500/10 rounded-full -mr-16 -mt-16 blur-3xl"></div>

        <div className="text-center mb-10">
          <div className="w-16 h-16 bg-cyan-500/10 border border-cyan-500/20 rounded-2xl flex items-center justify-center mx-auto mb-4 text-cyan-500">
            <Activity size={40} />
          </div>
          <h1 className="text-3xl font-black text-white tracking-tight">Clinicks<span className="text-cyan-500">Web</span></h1>
          <p className="text-slate-500 text-sm mt-2">Gestión Hospitalaria Profesional</p>
        </div>

        <form onSubmit={handleLogin} className="space-y-6">
          <div>
            <label className="text-xs font-black uppercase text-slate-500 ml-1 mb-2 block">Usuario / Matrícula</label>
            <div className="relative">
              <User className="absolute left-4 top-3.5 text-slate-500" size={18} />
              <input 
                required
                className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3.5 pl-12 pr-4 text-slate-300 focus:border-cyan-500 outline-none transition-all"
                placeholder="Ej: MN-12345"
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