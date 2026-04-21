// 1. IMPORTACIONES
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { useNavigate } from 'react-router-dom';
import { usePatients } from '../../hooks/usePatients'; // Reutilizamos el cerebro
import { ClipboardPlus, Activity, ExternalLink, User } from 'lucide-react';

const Dashboard = () => {
  const navigate = useNavigate();

  // 2. LÓGICA DE SESIÓN
  const idGuardado = localStorage.getItem('medicoId');
  const medicoNombre = localStorage.getItem('medicoNombre') || "Alex Carter";
  const MEDICO_ID_ACTUAL = parseInt(idGuardado) || 1; 

  // 3. CONSUMO DE DATOS REALES
  const { pacientesFiltrados, cargando, error } = usePatients(MEDICO_ID_ACTUAL);

  // Tomamos solo los últimos 5 para que el Dashboard no sea gigante
  const pacientesRecientes = pacientesFiltrados.slice(0, 5);

  return (
    <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />

      <div className="flex-1 flex flex-col min-w-0">
        <Header paginaActual='Dashboard'/>

        <main className="p-8 overflow-y-auto">
          
          {/* BIENVENIDA DINÁMICA */}
          <div className="flex justify-between items-end mb-8">
            <div>
              <h1 className="text-4xl font-bold text-white">Buenos días, {medicoNombre}</h1>
              <p className="text-slate-500 mt-1">Aquí tienes un resumen de tu actividad de hoy.</p>
            </div> 
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            
            {/* COLUMNA IZQUIERDA: Acciones y Agenda */}
            <div className="space-y-8">
              <div className="grid grid-cols-2 gap-4">
                <button 
                  onClick={() => navigate('/nueva-consulta')} 
                  className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-cyan-500/50 transition-all group shadow-sm"
                >
                  <div className="p-4 bg-cyan-500/10 rounded-full text-cyan-400 group-hover:bg-cyan-500 group-hover:text-slate-950 transition-colors">
                    <ClipboardPlus size={32} />
                  </div>
                  <span className="font-bold text-sm text-center">Nueva Consulta</span>
                </button>

                <button className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-purple-500/50 transition-all group shadow-sm">
                  <div className="p-4 bg-purple-500/10 rounded-full text-purple-400 group-hover:bg-purple-500 group-hover:text-slate-950 transition-colors">
                    <Activity size={32} />
                  </div>
                  <span className="font-bold text-sm text-center">Procedimiento</span>
                </button>
              </div>

              {/* Agenda Simulada */}
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 shadow-sm">
                <h3 className="text-xl font-bold mb-4 flex items-center gap-2">
                  <div className="w-1 h-6 bg-cyan-500 rounded-full"></div> Mi Agenda
                </h3>
                <div className="space-y-4">
                  <p className="text-xs text-slate-500 italic">No tienes turnos pendientes para la próxima hora.</p>
                </div>
              </div>
            </div>

            {/* COLUMNA DERECHA: Tabla con DATOS REALES */}
            <div className="lg:col-span-2 bg-slate-900 rounded-2xl border border-slate-800 overflow-hidden shadow-xl flex flex-col">
              <div className="p-6 border-b border-slate-800 flex justify-between items-center">
                <h3 className="text-xl font-bold">Pacientes Recientes</h3>
                <button 
                  onClick={() => navigate('/pacientes')}
                  className="text-xs text-cyan-500 hover:underline font-bold"
                >
                  Ver todos
                </button>
              </div>
              
              <table className="w-full text-left">
                <thead className="bg-slate-950/50 text-slate-500 text-[10px] uppercase tracking-widest font-black">
                  <tr>
                    <th className="p-4">Paciente</th>
                    <th className="p-4">DNI</th>
                    <th className="p-4">Estado</th>
                    <th className="p-4 text-right">Acción</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-800">
                  {cargando ? (
                    <tr><td colSpan="4" className="p-10 text-center text-slate-500">Cargando datos...</td></tr>
                  ) : error ? (
                    <tr><td colSpan="4" className="p-10 text-center text-red-500">{error}</td></tr>
                  ) : pacientesRecientes.length > 0 ? (
                    pacientesRecientes.map((paciente) => (
                      <tr key={paciente.id} className="hover:bg-slate-800/30 transition-colors group">
                        <td className="p-4">
                          <div className="flex items-center gap-3">
                            <div className="w-8 h-8 rounded-full bg-slate-800 flex items-center justify-center text-xs font-bold text-cyan-500 border border-slate-700">
                              {paciente.nombreCompleto?.charAt(0)}
                            </div>
                            <span className="text-sm font-bold text-slate-200">{paciente.nombreCompleto}</span>
                          </div>
                        </td>
                        <td className="p-4 text-sm text-slate-400 font-mono">{paciente.dni}</td>
                        <td className="p-4">
                          <span className={`px-2 py-0.5 rounded text-[9px] font-black uppercase ${
                            paciente.estaActivo ? "bg-green-500/10 text-green-500" : "bg-red-500/10 text-red-500"
                          }`}>
                            {paciente.estaActivo ? "Activo" : "OI"}
                          </span>
                        </td>
                        <td className="p-4 text-right">
                          <button 
                            onClick={() => navigate(`/pacientes/${paciente.id}/historial`)}
                            className="p-2 hover:bg-cyan-500/10 rounded-lg text-cyan-400 transition-colors inline-flex items-center gap-1 text-xs font-bold"
                          >
                            Detalles <ExternalLink size={14} />
                          </button>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr><td colSpan="4" className="p-10 text-center text-slate-600 italic">No se encontraron pacientes atendidos.</td></tr>
                  )}
                </tbody>
              </table>
            </div>

          </div>
        </main>
      </div>
    </div>
  );
};

export default Dashboard;