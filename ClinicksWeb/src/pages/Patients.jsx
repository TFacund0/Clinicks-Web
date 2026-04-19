import Sidebar from '../components/Sidebar';
import Header from '../components/Header';
import { Search, Filter, ExternalLink, UserPlus } from 'lucide-react';

const Patients = () => {
  return (
    <div className="flex h-screen w-full bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />
      <div className="flex-1 flex flex-col min-w-0">
        <Header />
        <main className="flex-1 p-8 overflow-y-auto w-full">
          <div className="max-w-[1600px] mx-auto">
            
            {/* Header de la Página */}
            <div className="flex justify-between items-center mb-8">
              <div>
                <h1 className="text-3xl font-bold">Listado de Pacientes</h1>
                <p className="text-slate-500 text-sm">Gestiona y consulta el historial de tus pacientes atendidos.</p>
              </div>
              <button className="flex items-center gap-2 bg-cyan-500 text-slate-950 px-4 py-2 rounded-xl font-bold hover:bg-cyan-400 transition-all">
                <UserPlus size={18} /> Nuevo Paciente
              </button>
            </div>

            {/* Barra de Herramientas (Búsqueda y Filtros) */}
            <div className="bg-slate-900 p-4 rounded-2xl border border-slate-800 mb-6 flex flex-wrap gap-4 items-center justify-between">
              <div className="flex items-center gap-3 bg-slate-950 px-4 py-2 rounded-xl border border-slate-800 w-full md:w-96">
                <Search size={18} className="text-slate-500" />
                <input type="text" placeholder="Buscar por Nombre o DNI..." className="bg-transparent border-none outline-none text-sm text-slate-300 w-full" />
              </div>
              <div className="flex items-center gap-2">
                <button className="px-4 py-2 bg-cyan-500/10 text-cyan-400 rounded-lg text-xs font-bold border border-cyan-500/20">Todos</button>
                <button className="px-4 py-2 hover:bg-slate-800 text-slate-400 rounded-lg text-xs font-bold transition-colors">Activos</button>
                <button className="px-4 py-2 hover:bg-slate-800 text-slate-400 rounded-lg text-xs font-bold transition-colors border border-slate-800"><Filter size={14} /></button>
              </div>
            </div>

            {/* Tabla Principal */}
            <div className="bg-slate-900 rounded-2xl border border-slate-800 overflow-hidden">
              <table className="w-full text-left">
                <thead className="bg-slate-950 text-slate-500 text-xs uppercase tracking-widest font-bold">
                  <tr>
                    <th className="p-5">ID</th>
                    <th className="p-5">Paciente</th>
                    <th className="p-5 text-center">Edad</th>
                    <th className="p-5">Última Consulta</th>
                    <th className="p-5">Estado</th>
                    <th className="p-5 text-right">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-800">
                  {[1, 2, 3, 4, 5, 6].map((i) => (
                    <tr key={i} className="hover:bg-slate-800/30 transition-colors group">
                      <td className="p-5 text-cyan-500 font-mono text-sm">#000{i}</td>
                      <td className="p-5">
                        <div className="flex items-center gap-3">
                          <img src={`https://i.pravatar.cc/150?u=pat${i}`} className="w-9 h-9 rounded-full border border-slate-700" alt="p" />
                          <div>
                            <p className="font-bold text-slate-200 text-sm leading-none">Paciente de Prueba {i}</p>
                            <p className="text-[10px] text-slate-500 mt-1 uppercase tracking-tighter">DNI: 45.XXX.XXX</p>
                          </div>
                        </div>
                      </td>
                      <td className="p-5 text-center text-sm text-slate-400">28</td>
                      <td className="p-5 text-sm text-slate-400">Mon, 18 Oct 2023</td>
                      <td className="p-5">
                        <span className="bg-green-500/10 text-green-500 px-2 py-1 rounded text-[10px] font-bold uppercase tracking-wider">Activo</span>
                      </td>
                      <td className="p-5 text-right">
                        <button className="text-cyan-400 text-xs font-bold flex items-center gap-1 ml-auto hover:text-cyan-300 transition-colors">
                          Ver Historial <ExternalLink size={14} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default Patients;