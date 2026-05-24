import { useNavigate } from 'react-router-dom';
import PageLayout from '../../components/PageLayout';
import { usePatients } from '../../hooks/usePatients';
import { useAuth } from '../../context/AuthContext';
import { useAgenda } from '../../hooks/useAgenda';
import { ClipboardPlus, Activity, ExternalLink } from 'lucide-react';

// Vista principal que muestra un resumen rápido de acciones, agenda y los últimos pacientes atendidos.
const Dashboard = () => {
  const navigate = useNavigate();

  // El nombre del médico proviene del AuthContext
  const { medicoNombre } = useAuth();

  // Obtiene los pacientes atendidos por este médico manejando estados de carga y error.
  const { pacientes, cargando, error } = usePatients();
  
  // Obtiene los turnos para la agenda
  const { turnos, cargandoTurnos: cargandoAgenda } = useAgenda();

  // Limita la lista a solo 5 pacientes para no sobrecargar la pantalla del panel de control.
  const pacientesRecientes = pacientes ? pacientes.slice(0, 5) : [];

  // Filtra los turnos de hoy que estén pendientes/confirmados
  const turnosHoy = turnos ? turnos.filter(t => {
    const hoy = new Date();
    const esHoy = t.fecha.getDate() === hoy.getDate() &&
                  t.fecha.getMonth() === hoy.getMonth() &&
                  t.fecha.getFullYear() === hoy.getFullYear();
    const estadosActivos = ['Pendiente', 'Confirmado', 'En Curso'];
    return esHoy && estadosActivos.includes(t.estado);
  }).sort((a, b) => a.fecha - b.fecha) : [];

  // Calcula estadísticas generales para llenar el dashboard de forma útil
  const turnosMes = turnos ? turnos.length : 0;
  const totalPacientes = pacientes ? pacientes.length : 0;
  const turnosAtendidosMes = turnos ? turnos.filter(t => t.estado === 'Atendido').length : 0;

  return (
    <PageLayout title="Dashboard">
      {/* Mensaje de bienvenida personalizado con el nombre del médico */}
      <div className="flex justify-between items-end mb-8">
            <div>
              <h1 className="text-4xl font-bold text-white">Buenos días, {medicoNombre}</h1>
              <p className="text-slate-500 mt-1">Aquí tienes un resumen de tu actividad de hoy.</p>
            </div> 
          </div>

          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            
            {/* Botones de acceso rápido para crear consultas o procedimientos */}
            <div className="space-y-8">
              <div className="grid grid-cols-2 gap-4">
                <button 
                  onClick={() => navigate('/acceso-consulta')} 
                  className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-cyan-500/50 transition-all group shadow-sm">
                  <div className="p-4 bg-cyan-500/10 rounded-full text-cyan-400 group-hover:bg-cyan-500 group-hover:text-slate-950 transition-colors">
                    <ClipboardPlus size={32} />
                  </div>
                  <span className="font-bold text-sm text-center">Nueva Consulta</span>
                </button>

                <button 
                  onClick={() => navigate('/acceso-procedimiento')} 
                  className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-purple-500/50 transition-all group shadow-sm">
                  <div className="p-4 bg-purple-500/10 rounded-full text-purple-400 group-hover:bg-purple-500 group-hover:text-slate-950 transition-colors">
                    <Activity size={32} />
                  </div>
                  <span className="font-bold text-sm text-center">Procedimiento</span>
                </button>
              </div>

              {/* Sección de Mi Agenda */}
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 shadow-sm flex flex-col">
                <div className="flex justify-between items-center mb-4">
                  <h3 className="text-xl font-bold flex items-center gap-2">
                    <div className="w-1 h-6 bg-cyan-500 rounded-full"></div> Mi Agenda (Hoy)
                  </h3>
                  <button 
                    onClick={() => navigate('/agenda')}
                    className="text-xs text-cyan-500 hover:underline font-bold"
                  >
                    Ver completa
                  </button>
                </div>
                
                <div className="space-y-3 overflow-y-auto max-h-64 pr-1 scrollbar-thin">
                  {cargandoAgenda ? (
                    <p className="text-xs text-slate-500 italic mt-2">Cargando turnos...</p>
                  ) : turnosHoy.length > 0 ? (
                    turnosHoy.map((turno) => (
                      <div key={turno.id} className="p-3 bg-slate-950 border border-slate-800 rounded-xl flex justify-between items-center group hover:border-cyan-500/30 transition-all cursor-pointer" onClick={() => navigate('/agenda')}>
                        <div className="truncate pr-2">
                          <p className="font-bold text-sm text-slate-200 truncate">{turno.pacienteNombre}</p>
                          <p className="text-[10px] text-slate-500 font-mono mt-0.5 truncate">{turno.tipo} • {turno.motivo}</p>
                        </div>
                        <div className="text-right shrink-0">
                          <p className="font-bold text-cyan-400 font-mono text-sm">{turno.hora}</p>
                          <span className="text-[8px] uppercase tracking-wider font-black text-slate-500 bg-slate-900 px-1.5 py-0.5 rounded border border-slate-800 mt-1 inline-block">{turno.estado}</span>
                        </div>
                      </div>
                    ))
                  ) : (
                    <p className="text-xs text-slate-500 italic mt-2">No tienes turnos pendientes para hoy.</p>
                  )}
                </div>
              </div>
            </div>

            {/* Tabla que lista los últimos pacientes atendidos y permite ir a su historial */}
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
                            {paciente.estaActivo ? "Activo" : "Inactivo"}
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

          {/* NUEVO PANEL DE ESTADÍSTICAS PARA LLENAR EL ESPACIO VACÍO */}
          <div className="mt-8 grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="bg-gradient-to-br from-slate-900 to-slate-900/50 p-6 rounded-2xl border border-slate-800 shadow-sm flex items-center justify-between group hover:border-cyan-500/30 transition-all">
              <div>
                <p className="text-[10px] text-slate-500 uppercase tracking-widest font-black">Pacientes Históricos</p>
                <h3 className="text-3xl font-bold text-white mt-1 font-mono">{totalPacientes}</h3>
              </div>
              <div className="w-12 h-12 rounded-2xl bg-cyan-500/10 border border-cyan-500/20 flex items-center justify-center text-cyan-400 group-hover:scale-110 transition-transform">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M22 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
              </div>
            </div>

            <div className="bg-gradient-to-br from-slate-900 to-slate-900/50 p-6 rounded-2xl border border-slate-800 shadow-sm flex items-center justify-between group hover:border-purple-500/30 transition-all">
              <div>
                <p className="text-[10px] text-slate-500 uppercase tracking-widest font-black">Turnos este Mes</p>
                <h3 className="text-3xl font-bold text-white mt-1 font-mono">{turnosMes}</h3>
              </div>
              <div className="w-12 h-12 rounded-2xl bg-purple-500/10 border border-purple-500/20 flex items-center justify-center text-purple-400 group-hover:scale-110 transition-transform">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><rect width="18" height="18" x="3" y="4" rx="2" ry="2"></rect><line x1="16" x2="16" y1="2" y2="6"></line><line x1="8" x2="8" y1="2" y2="6"></line><line x1="3" x2="21" y1="10" y2="10"></line></svg>
              </div>
            </div>

            <div className="bg-gradient-to-br from-slate-900 to-slate-900/50 p-6 rounded-2xl border border-slate-800 shadow-sm flex items-center justify-between group hover:border-emerald-500/30 transition-all">
              <div>
                <p className="text-[10px] text-slate-500 uppercase tracking-widest font-black">Atendidos en el Mes</p>
                <h3 className="text-3xl font-bold text-emerald-400 mt-1 font-mono">{turnosAtendidosMes}</h3>
              </div>
              <div className="w-12 h-12 rounded-2xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center text-emerald-400 group-hover:scale-110 transition-transform">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"></path><polyline points="22 4 12 14.01 9 11.01"></polyline></svg>
              </div>
            </div>
          </div>

    </PageLayout>
  );
};

export default Dashboard;