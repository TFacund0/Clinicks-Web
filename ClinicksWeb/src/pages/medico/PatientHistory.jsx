// src/pages/medico/PatientHistory.jsx
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, User, Activity, FileText, Clock, Stethoscope, Search, Filter } from 'lucide-react';
import PageLayout from '../../components/PageLayout';
import { usePatientHistory } from '../../hooks/usePatientHistory';

const PatientHistory = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  // SOC-1: Usar API real
  // SOC-2: Extraer la lógica de filtros del componente UI (ya vienen computados en 'historial')
  const { paciente, historial, cargando, error, filtros, setFiltros } = usePatientHistory(id);

  if (cargando) {
      return (
          <PageLayout title="Historial Clínico">
              <div className="flex h-full items-center justify-center text-slate-400">
                  <div className="animate-pulse flex flex-col items-center">
                      <Activity className="animate-spin mb-4" size={32} />
                      <p>Cargando historial completo...</p>
                  </div>
              </div>
          </PageLayout>
      );
  }

  if (error || !paciente) {
      return (
          <PageLayout title="Historial Clínico">
              <div className="flex flex-col items-center justify-center h-full text-slate-400 gap-4">
                  <p className="text-red-400 font-bold text-lg">{error || "No se encontró el paciente."}</p>
                  <button onClick={() => navigate('/pacientes')} className="text-cyan-500 hover:underline">
                      Volver a la lista de pacientes
                  </button>
              </div>
          </PageLayout>
      );
  }

  return (
    <PageLayout title={`Historial Clínico Integral`}>
      {/* Botón para volver al listado de pacientes */}
      <button 
        onClick={() => navigate('/pacientes')}
        className="flex items-center gap-2 text-slate-500 hover:text-cyan-400 transition-colors mb-6 text-sm font-medium"
      >
        <ArrowLeft size={16} /> Volver al listado
      </button>

      {/* CABECERA DEL PACIENTE (Premium Card) */}
      <div className="bg-gradient-to-r from-slate-900 to-slate-950 border border-slate-800/50 rounded-3xl p-8 mb-10 flex flex-wrap gap-8 justify-between items-center shadow-2xl relative overflow-hidden">
        {/* Glow de fondo para darle un toque premium */}
        <div className="absolute top-0 right-0 w-64 h-64 bg-cyan-500/5 rounded-full blur-3xl -translate-y-1/2 translate-x-1/3"></div>
        
        <div className="flex items-center gap-4 z-10">
            <div className="w-16 h-16 bg-slate-800 rounded-2xl flex items-center justify-center text-cyan-500 border border-slate-700 shadow-inner">
                <User size={32} />
            </div>
            <div>
                <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest mb-1">Paciente</p>
                <p className="font-bold text-2xl text-white">{paciente.nombreCompleto}</p>
            </div>
        </div>
        
        <div className="flex gap-12 z-10">
            <div>
            <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest mb-1">DNI</p>
            <p className="font-mono text-slate-300 text-lg">{paciente.dni}</p>
            </div>
            <div>
            <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest mb-1">Edad</p>
            <p className="font-mono text-slate-300 text-lg">{paciente.edad} años</p>
            </div>
            <div>
            <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest mb-1">Última Visita</p>
            <p className="font-mono text-cyan-400 text-lg">{paciente.fechaUltimaConsulta}</p>
            </div>
            <div>
            <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest mb-1">Estado</p>
            <span className={`px-3 py-1 rounded-md text-xs font-black uppercase tracking-wider inline-block mt-1 ${
                paciente.estaActivo ? "bg-green-500/20 text-green-400 border border-green-500/30" : "bg-red-500/20 text-red-400 border border-red-500/30"
            }`}>
                {paciente.estaActivo ? "ACTIVO" : "INACTIVO"}
            </span>
            </div>
        </div>
      </div>

      {/* BARRA DE FILTROS INTELIGENTES */}
      <div className="bg-slate-900 border border-slate-800 rounded-2xl p-5 mb-8 flex flex-col md:flex-row gap-6 items-center shadow-lg">
        <div className="relative flex-1 w-full group">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-500 group-focus-within:text-cyan-500 transition-colors" size={20} />
          <input
            type="text"
            placeholder="Buscar por diagnóstico, tratamiento, médico o descripción..."
            value={filtros.texto}
            onChange={(e) => setFiltros({ ...filtros, texto: e.target.value })}
            className="w-full bg-slate-950 border border-slate-800 rounded-xl py-3 pl-12 pr-4 text-slate-200 focus:border-cyan-500 outline-none transition-all placeholder:text-slate-600"
          />
        </div>
        
        <div className="flex items-center gap-4 border-l border-slate-800 pl-6">
          <Filter size={20} className="text-slate-500 hidden md:block" />
          <label className="flex items-center gap-2 cursor-pointer group">
            <input 
              type="checkbox" 
              checked={filtros.mostrarConsultas}
              onChange={(e) => setFiltros({ ...filtros, mostrarConsultas: e.target.checked })}
              className="w-5 h-5 accent-cyan-500 cursor-pointer"
            />
            <span className="text-slate-300 font-medium group-hover:text-cyan-400 transition-colors">Consultas</span>
          </label>
          <label className="flex items-center gap-2 cursor-pointer group">
            <input 
              type="checkbox" 
              checked={filtros.mostrarProcedimientos}
              onChange={(e) => setFiltros({ ...filtros, mostrarProcedimientos: e.target.checked })}
              className="w-5 h-5 accent-purple-500 cursor-pointer"
            />
            <span className="text-slate-300 font-medium group-hover:text-purple-400 transition-colors">Procedimientos</span>
          </label>
        </div>
      </div>

      {/* LÍNEA DE TIEMPO (TIMELINE) */}
      <h2 className="text-xl font-bold flex items-center gap-3 mb-8 text-white">
        <Clock size={24} className="text-cyan-500" /> Línea de Tiempo Clínica
      </h2>
      
      {historial.length === 0 ? (
        <div className="p-16 text-center border border-dashed border-slate-800 rounded-3xl bg-slate-900/30">
            <Search className="mx-auto text-slate-600 mb-4" size={48} />
            <p className="text-slate-400 font-medium text-lg">No se encontraron registros que coincidan con tu búsqueda.</p>
            <button 
                onClick={() => setFiltros({ texto: '', mostrarConsultas: true, mostrarProcedimientos: true })}
                className="mt-4 text-cyan-500 font-bold hover:underline"
            >
                Limpiar Filtros
            </button>
        </div>
      ) : (
        <div className="relative border-l-2 border-slate-800 ml-4 sm:ml-8 pl-8 sm:pl-12 pb-8 space-y-10">
          {historial.map((item, index) => {
            const isConsulta = item.tipoRegistro === 'consulta';
            const idUnico = isConsulta ? `C-${item.idConsulta}` : `P-${item.IdProcedimiento || item.idProcedimiento}`;
            
            return (
              <div key={idUnico} className="relative group">
                {/* Timeline Dot */}
                <div className={`absolute -left-[43px] sm:-left-[59px] top-6 w-5 h-5 rounded-full border-4 border-slate-950 transition-all duration-300 ${
                    isConsulta 
                        ? 'bg-cyan-500 group-hover:bg-cyan-400 group-hover:shadow-[0_0_15px_rgba(6,182,212,0.5)]' 
                        : 'bg-purple-500 group-hover:bg-purple-400 group-hover:shadow-[0_0_15px_rgba(168,85,247,0.5)]'
                }`}></div>
                
                {/* Timeline Card */}
                <div className={`bg-slate-900 border rounded-3xl p-6 shadow-xl transition-all duration-300 hover:-translate-y-1 ${
                    isConsulta 
                        ? 'border-cyan-500/20 hover:border-cyan-500/50 hover:shadow-cyan-900/20' 
                        : 'border-purple-500/20 hover:border-purple-500/50 hover:shadow-purple-900/20'
                }`}>
                  
                  {/* Card Header */}
                  <div className="flex justify-between items-start mb-6 pb-6 border-b border-slate-800/50">
                    <div className="flex items-center gap-4">
                      <div className={`p-3 rounded-2xl ${isConsulta ? 'bg-cyan-500/10 text-cyan-500' : 'bg-purple-500/10 text-purple-500'}`}>
                        {isConsulta ? <Stethoscope size={24} /> : <FileText size={24} />}
                      </div>
                      <div>
                        <h3 className={`font-black text-xl tracking-tight ${isConsulta ? 'text-cyan-400' : 'text-purple-400'}`}>
                          {isConsulta ? 'Consulta Médica' : 'Procedimiento Médico'}
                        </h3>
                        <span className="text-slate-500 text-sm font-mono mt-1 block">
                          {new Date(item.fechaOrden).toLocaleString('es-ES', { dateStyle: 'long', timeStyle: 'short' })}
                        </span>
                      </div>
                    </div>
                    <div className="text-right">
                      <p className="text-[10px] uppercase font-black tracking-widest text-slate-500 mb-1">
                        {isConsulta ? 'Atendido por' : 'Realizado por'}
                      </p>
                      <p className={`font-bold ${isConsulta ? 'text-cyan-500' : 'text-purple-500'}`}>
                        {item.medicoAtencion}
                      </p>
                    </div>
                  </div>

                  {/* Card Content */}
                  <div className="mb-6">
                    <h4 className="text-white font-bold text-lg mb-1">
                        {isConsulta ? item.motivo : item.tipo}
                    </h4>
                  </div>

                  <div className={`grid ${isConsulta ? 'grid-cols-2' : 'grid-cols-1'} gap-4 text-sm bg-slate-950/60 p-5 rounded-2xl border ${isConsulta ? 'border-cyan-500/10' : 'border-purple-500/10'}`}>
                    {isConsulta ? (
                        <>
                            <div>
                                <span className="block text-[10px] uppercase tracking-widest font-black text-slate-500 mb-2">Diagnóstico</span>
                                <span className="text-slate-300 leading-relaxed block bg-slate-900/50 p-3 rounded-lg">{item.diagnostico}</span>
                            </div>
                            <div>
                                <span className="block text-[10px] uppercase tracking-widest font-black text-slate-500 mb-2">Tratamiento</span>
                                <span className="text-slate-300 leading-relaxed block bg-slate-900/50 p-3 rounded-lg">{item.tratamiento}</span>
                            </div>
                            {item.observacion && (
                            <div className="col-span-2 mt-2">
                                <span className="block text-[10px] uppercase tracking-widest font-black text-slate-500 mb-2">Observaciones</span>
                                <span className="text-slate-400 leading-relaxed italic block border-l-2 border-slate-700 pl-3">{item.observacion}</span>
                            </div>
                            )}
                            {item.recomendacion && (
                            <div className="col-span-2 mt-2">
                                <span className="block text-[10px] uppercase tracking-widest font-black text-cyan-600 mb-2">Recomendación Médica</span>
                                <span className="text-cyan-100/70 leading-relaxed block bg-cyan-950/30 p-3 rounded-lg border border-cyan-900/30">{item.recomendacion}</span>
                            </div>
                            )}
                        </>
                    ) : (
                        <>
                            <div>
                                <span className="block text-[10px] uppercase tracking-widest font-black text-slate-500 mb-2">Descripción del Procedimiento</span>
                                <span className="text-slate-300 leading-relaxed block bg-slate-900/50 p-4 rounded-xl border-l-2 border-purple-500/30">{item.descripcion || "Sin descripción detallada."}</span>
                            </div>
                            <div className="mt-2">
                                <span className="block text-[10px] uppercase tracking-widest font-black text-purple-600 mb-2">Resultado / Evolución</span>
                                <span className="text-purple-100/70 leading-relaxed block bg-purple-950/30 p-4 rounded-xl border border-purple-900/30">
                                    {item.resultado || "A la espera de resultados."}
                                </span>
                            </div>
                        </>
                    )}
                  </div>

                </div>
              </div>
            );
          })}
        </div>
      )}
    </PageLayout>
  );
};

export default PatientHistory;