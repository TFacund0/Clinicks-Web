// src/pages/medico/TurnAttentionDetail.jsx
import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { 
  ArrowLeft, 
  User, 
  Clock, 
  Calendar as CalendarIcon, 
  Stethoscope, 
  FileText, 
  Activity, 
  Search, 
  Filter, 
  Play, 
  X,
  ClipboardPlus,
  ChevronDown
} from 'lucide-react';
import PageLayout from '../../components/PageLayout';
import { usePatientHistory } from '../../hooks/usePatientHistory';
import agendaService from '../../services/agendaService';

const TurnAttentionDetail = () => {
  const { idTurno } = useParams();
  const navigate = useNavigate();
  const location = useLocation();

  const [turno, setTurno] = useState(() => location.state?.turno || null);
  const [cargandoTurno, setCargandoTurno] = useState(() => !location.state?.turno);
  const [errorTurno, setErrorTurno] = useState(null);
  const [showSelectionModal, setShowSelectionModal] = useState(false);

  // Intentamos obtener el turno del estado de navegación o consultando la API
  useEffect(() => {
    if (turno) {
      return;
    }

    const cargarTurno = async () => {
      await Promise.resolve();
      try {
        setCargandoTurno(true);
        setErrorTurno(null);
        const data = await agendaService.obtenerTurnoPorId(idTurno);
        
        // Mapear el turno tal como lo espera la aplicación
        const fechaObj = new Date(data.fechaTurno);
        const turnoMapeado = {
          id: data.idTurno,
          pacienteId: data.idPaciente,
          pacienteNombre: data.pacienteNombreCompleto || "Paciente sin nombre",
          pacienteDni: data.dniPaciente || "Sin DNI",
          fecha: fechaObj,
          hora: fechaObj.toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }),
          duracion: 20,
          tipo: "Consulta", 
          motivo: data.motivo || "Sin motivo especificado",
          estado: data.estado || "Pendiente"
        };
        setTurno(turnoMapeado);
      } catch (err) {
        console.error("Error al cargar el turno por ID:", err);
        setErrorTurno("No se pudo cargar el turno especificado. Verifique si existe.");
      } finally {
        setCargandoTurno(false);
      }
    };

    cargarTurno();
  }, [idTurno, turno]);

  // Usamos el hook de historial clínico, pasando el pacienteId una vez esté cargado el turno
  const pacienteId = turno?.pacienteId || null;
  const { 
    paciente, 
    historial, 
    cargando: cargandoHistorial, 
    error: errorHistorial, 
    filtros, 
    setFiltros 
  } = usePatientHistory(pacienteId);

  const alIniciarAtencion = () => {
    setShowSelectionModal(true);
  };

  const seleccionarAtencion = (tipoAtencion) => {
    setShowSelectionModal(false);
    
    // Cambiamos temporalmente el estado en la interfaz si es necesario
    if (turno) {
      const rutaDestino = tipoAtencion === 'Procedimiento' ? '/nuevo-procedimiento' : '/nueva-consulta';
      navigate(rutaDestino, { 
        state: { 
          idTurno: turno.id, 
          dniIngresado: turno.pacienteDni 
        } 
      });
    }
  };

  if (cargandoTurno) {
    return (
      <PageLayout title="Atención de Turno">
        <div className="flex h-full items-center justify-center text-slate-400">
          <div className="animate-pulse flex flex-col items-center">
            <Activity className="animate-spin mb-4 text-cyan-500" size={32} />
            <p>Cargando información del turno...</p>
          </div>
        </div>
      </PageLayout>
    );
  }

  if (errorTurno || !turno) {
    return (
      <PageLayout title="Atención de Turno">
        <div className="flex flex-col items-center justify-center h-full text-slate-400 gap-4 p-8">
          <p className="text-red-400 font-bold text-lg">{errorTurno || "No se pudo recuperar el turno."}</p>
          <button onClick={() => navigate('/agenda')} className="px-4 py-2 bg-slate-900 border border-slate-800 rounded-xl hover:border-slate-700 text-cyan-400">
            Volver a la Agenda
          </button>
        </div>
      </PageLayout>
    );
  }

  return (
    <PageLayout title="Atención de Turno">
      {/* Botón para volver a la agenda */}
      <button 
        onClick={() => navigate('/agenda')}
        className="flex items-center gap-2 text-slate-500 hover:text-cyan-400 transition-colors mb-6 text-sm font-medium"
      >
        <ArrowLeft size={16} /> Volver a la Agenda
      </button>

      {/* CABECERA RESUMEN DE TURNO Y PACIENTE (Premium Glassmorphic Card) */}
      <div className="bg-linear-to-r from-slate-900 to-slate-950 border border-slate-800/60 rounded-3xl p-8 mb-8 relative overflow-hidden shadow-2xl">
        <div className="absolute top-0 right-0 w-80 h-80 bg-cyan-500/5 rounded-full blur-3xl -translate-y-1/2 translate-x-1/3"></div>
        
        <div className="flex flex-col lg:flex-row justify-between items-start lg:items-center gap-6 z-10 relative">
          
          {/* Información del Paciente */}
          <div className="flex items-center gap-5">
            <div className="w-16 h-16 bg-slate-800 rounded-2xl flex items-center justify-center text-cyan-500 border border-slate-700 shadow-inner">
              <User size={32} />
            </div>
            <div>
              <div className="flex items-center gap-3">
                <p className="text-slate-500 text-[10px] uppercase font-black tracking-widest">Atención Activa</p>
                <span className={`px-2 py-0.5 rounded text-[8px] font-black uppercase tracking-wider border ${
                  turno.estado === 'Confirmado' ? 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20' : 
                  turno.estado === 'En Curso' ? 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30' : 
                  'bg-slate-800 text-slate-400 border-slate-700/50'
                }`}>
                  {turno.estado}
                </span>
              </div>
              <h2 className="font-bold text-2xl text-white mt-1">{turno.pacienteNombre}</h2>
              <p className="text-xs text-slate-400 font-mono mt-1">DNI: {turno.pacienteDni}</p>
            </div>
          </div>

          {/* Información del Turno actual */}
          <div className="grid grid-cols-2 sm:flex sm:gap-8 bg-slate-950/50 p-4 rounded-2xl border border-slate-800/80">
            <div>
              <p className="text-slate-500 text-[9px] uppercase font-black tracking-widest mb-1">Fecha</p>
              <p className="font-bold text-slate-200 text-sm flex items-center gap-1.5">
                <CalendarIcon size={14} className="text-slate-400" />
                {turno.fecha.toLocaleDateString('es-AR', { day: 'numeric', month: 'short' })}
              </p>
            </div>
            <div className="border-l border-slate-800/50 pl-4">
              <p className="text-slate-500 text-[9px] uppercase font-black tracking-widest mb-1">Horario</p>
              <p className="font-bold text-slate-200 text-sm flex items-center gap-1.5">
                <Clock size={14} className="text-slate-400" />
                {turno.hora} HS
              </p>
            </div>
            {paciente && (
              <div className="border-l border-slate-800/50 pl-4 col-span-2 sm:col-span-1 mt-3 sm:mt-0">
                <p className="text-slate-500 text-[9px] uppercase font-black tracking-widest mb-1">Edad</p>
                <p className="font-bold text-slate-200 text-sm">{paciente.edad} años</p>
              </div>
            )}
          </div>

          {/* Botón de Acción Principal para Iniciar */}
          {turno.estado !== 'Atendido' && turno.estado !== 'Cancelado' ? (
            <button
              onClick={alIniciarAtencion}
              className="px-6 py-4 bg-cyan-500 hover:bg-cyan-400 text-slate-950 rounded-2xl font-bold text-sm flex items-center gap-2 transition-all shadow-lg shadow-cyan-500/15 active:scale-95 z-20"
            >
              <Play size={16} fill="currentColor" /> Iniciar Atención
            </button>
          ) : (
            <div className="bg-slate-950 border border-slate-800/80 px-4 py-3 rounded-2xl text-xs text-slate-500 font-semibold italic">
              Turno finalizado
            </div>
          )}
        </div>

        {/* Motivo de la Cita */}
        <div className="mt-6 pt-6 border-t border-slate-800/40">
          <p className="text-[10px] text-slate-500 uppercase tracking-widest font-black mb-1">Motivo de consulta programada</p>
          <p className="text-sm text-slate-300 italic">"{turno.motivo}"</p>
        </div>
      </div>

      {/* SECCIÓN DEL HISTORIAL CLÍNICO COMPLETO */}
      <div className="space-y-6">
        <div className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
          <h2 className="text-xl font-bold text-white flex items-center gap-2">
            <Clock size={20} className="text-cyan-500" /> Historial Clínico del Paciente
          </h2>

          {/* Filtros rápidos */}
          {paciente && (
            <div className="flex items-center gap-4 bg-slate-900 border border-slate-800 px-4 py-2.5 rounded-xl text-xs">
              <span className="text-slate-400 font-medium">Filtrar:</span>
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input 
                  type="checkbox" 
                  checked={filtros.mostrarConsultas}
                  onChange={(e) => setFiltros({ ...filtros, mostrarConsultas: e.target.checked })}
                  className="accent-cyan-500"
                />
                <span className="text-slate-200">Consultas</span>
              </label>
              <label className="flex items-center gap-1.5 cursor-pointer">
                <input 
                  type="checkbox" 
                  checked={filtros.mostrarProcedimientos}
                  onChange={(e) => setFiltros({ ...filtros, mostrarProcedimientos: e.target.checked })}
                  className="accent-purple-500"
                />
                <span className="text-slate-200">Procedimientos</span>
              </label>
            </div>
          )}
        </div>

        {cargandoHistorial ? (
          <div className="p-16 text-center border border-slate-800 rounded-3xl bg-slate-900/30">
            <Activity className="animate-spin mx-auto text-slate-600 mb-4" size={32} />
            <p className="text-slate-400">Cargando antecedentes clínicos...</p>
          </div>
        ) : errorHistorial ? (
          <p className="text-red-400 text-sm font-semibold">{errorHistorial}</p>
        ) : historial.length === 0 ? (
          <div className="p-16 text-center border border-dashed border-slate-800 rounded-3xl bg-slate-900/30">
            <p className="text-slate-400 font-medium text-lg">No hay registros clínicos anteriores cargados para este paciente.</p>
          </div>
        ) : (
          <div className="relative border-l-2 border-slate-800 ml-4 pl-8 pb-4 space-y-8">
            {historial.map((item) => {
              const isConsulta = item.tipoRegistro === 'consulta';
              const idUnico = isConsulta ? `C-${item.idConsulta}` : `P-${item.IdProcedimiento || item.idProcedimiento}`;
              
              return (
                <div key={idUnico} className="relative group">
                  {/* Timeline Dot */}
                  <div className={`absolute -left-10.75 top-5 w-5 h-5 rounded-full border-4 border-slate-950 transition-all duration-300 ${
                    isConsulta ? 'bg-cyan-500 group-hover:bg-cyan-400' : 'bg-purple-500 group-hover:bg-purple-400'
                  }`}></div>
                  
                  {/* Timeline Card */}
                  <div className={`bg-slate-900 border rounded-2xl p-5 shadow-xl transition-all ${
                    isConsulta ? 'border-cyan-500/10 hover:border-cyan-500/30' : 'border-purple-500/10 hover:border-purple-500/30'
                  }`}>
                    
                    {/* Header */}
                    <div className="flex justify-between items-start mb-4 pb-4 border-b border-slate-800/40">
                      <div className="flex items-center gap-3">
                        <div className={`p-2 rounded-xl ${isConsulta ? 'bg-cyan-500/10 text-cyan-500' : 'bg-purple-500/10 text-purple-500'}`}>
                          {isConsulta ? <Stethoscope size={18} /> : <FileText size={18} />}
                        </div>
                        <div>
                          <h4 className={`font-bold text-base ${isConsulta ? 'text-cyan-400' : 'text-purple-400'}`}>
                            {isConsulta ? 'Consulta Médica' : 'Procedimiento'}
                          </h4>
                          <span className="text-slate-500 text-xs font-mono mt-0.5 block">
                            {new Date(item.fechaOrden).toLocaleDateString('es-ES', { dateStyle: 'medium' })}
                          </span>
                        </div>
                      </div>
                      <div className="text-right">
                        <p className="text-[8px] uppercase font-black text-slate-500">Médico</p>
                        <p className="font-bold text-xs text-slate-300">{item.medicoAtencion}</p>
                      </div>
                    </div>

                    {/* Content */}
                    <div className="mb-4">
                      <h5 className="text-white font-bold text-sm mb-1">
                        {isConsulta ? item.motivo : item.tipo}
                      </h5>
                    </div>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3 text-xs bg-slate-950/60 p-4 rounded-xl border border-slate-800/40">
                      {isConsulta ? (
                        <>
                          <div>
                            <span className="block text-[8px] uppercase tracking-widest font-black text-slate-500 mb-1">Diagnóstico</span>
                            <span className="text-slate-300 bg-slate-900/50 p-2 rounded block">{item.diagnostico}</span>
                          </div>
                          <div>
                            <span className="block text-[8px] uppercase tracking-widest font-black text-slate-500 mb-1">Tratamiento</span>
                            <span className="text-slate-300 bg-slate-900/50 p-2 rounded block">{item.tratamiento}</span>
                          </div>
                          {item.observacion && (
                            <div className="col-span-1 md:col-span-2 mt-1">
                              <span className="block text-[8px] uppercase tracking-widest font-black text-slate-500 mb-1">Observaciones</span>
                              <span className="text-slate-400 leading-relaxed italic block border-l border-slate-700 pl-2">{item.observacion}</span>
                            </div>
                          )}
                        </>
                      ) : (
                        <>
                          <div className="col-span-1 md:col-span-2">
                            <span className="block text-[8px] uppercase tracking-widest font-black text-slate-500 mb-1">Descripción</span>
                            <span className="text-slate-300 bg-slate-900/50 p-2 rounded block">{item.descripcion || "Sin descripción."}</span>
                          </div>
                          <div className="col-span-1 md:col-span-2 mt-1">
                            <span className="block text-[8px] uppercase tracking-widest font-black text-purple-600 mb-1">Resultado / Evolución</span>
                            <span className="text-purple-100/70 bg-purple-950/20 p-2 rounded block border border-purple-900/20">{item.resultado || "A la espera."}</span>
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
      </div>

      {/* ==========================================
          MODAL DE SELECCIÓN DE TIPO DE ATENCIÓN (CONSULTA VS PROCEDIMIENTO)
          ========================================== */}
      {showSelectionModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 backdrop-blur-sm">
          <div className="bg-slate-900 border border-slate-800 rounded-3xl p-8 max-w-md w-full shadow-2xl relative animate-scale-up">
            
            {/* Botón cerrar */}
            <button 
              onClick={() => setShowSelectionModal(false)}
              className="absolute top-4 right-4 p-2 hover:bg-slate-800 rounded-xl text-slate-400 hover:text-slate-200 transition-colors"
            >
              <X size={20} />
            </button>

            {/* Encabezado */}
            <div className="text-center mb-6">
              <div className="w-12 h-12 bg-cyan-500/10 rounded-2xl flex items-center justify-center text-cyan-400 mx-auto mb-4 border border-cyan-500/20">
                <Play size={20} fill="currentColor" />
              </div>
              <h3 className="text-xl font-bold text-white">Iniciar Atención</h3>
              <p className="text-xs text-slate-400 mt-2">
                Seleccione el tipo de registro que desea realizar para el paciente.
              </p>
            </div>

            {/* Opciones */}
            <div className="space-y-4">
              
              {/* Opción 1: Consulta Médica */}
              <button
                onClick={() => seleccionarAtencion('Consulta')}
                className="w-full p-4 bg-slate-950 hover:bg-slate-800/50 border border-slate-800 hover:border-cyan-500/30 rounded-2xl flex items-center gap-4 text-left transition-all group"
              >
                <div className="w-10 h-10 rounded-xl bg-cyan-500/10 text-cyan-400 flex items-center justify-center group-hover:scale-110 transition-transform">
                  <ClipboardPlus size={20} />
                </div>
                <div>
                  <p className="font-bold text-slate-200 group-hover:text-cyan-400 transition-colors text-sm">Registrar Consulta Médica</p>
                  <p className="text-[10px] text-slate-500 mt-0.5">Control de rutina, diagnóstico y receta de fármacos.</p>
                </div>
              </button>

              {/* Opción 2: Procedimiento Médico */}
              <button
                onClick={() => seleccionarAtencion('Procedimiento')}
                className="w-full p-4 bg-slate-950 hover:bg-slate-800/50 border border-slate-800 hover:border-purple-500/30 rounded-2xl flex items-center gap-4 text-left transition-all group"
              >
                <div className="w-10 h-10 rounded-xl bg-purple-500/10 text-purple-400 flex items-center justify-center group-hover:scale-110 transition-transform">
                  <Activity size={20} />
                </div>
                <div>
                  <p className="font-bold text-slate-200 group-hover:text-purple-400 transition-colors text-sm">Registrar Procedimiento</p>
                  <p className="text-[10px] text-slate-500 mt-0.5">Estudios, curaciones, cirugías menores u otros.</p>
                </div>
              </button>

            </div>

            {/* Cancelar */}
            <button
              onClick={() => setShowSelectionModal(false)}
              className="w-full mt-6 py-3 bg-slate-850 hover:bg-slate-800 text-slate-400 hover:text-slate-300 font-bold rounded-xl text-xs transition-colors"
            >
              Cancelar
            </button>

          </div>
        </div>
      )}
    </PageLayout>
  );
};

export default TurnAttentionDetail;
