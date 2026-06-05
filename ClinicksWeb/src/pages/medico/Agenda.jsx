// src/pages/medico/Agenda.jsx
import React, { useState, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { useAgenda } from '../../hooks/useAgenda';
import { useAuth } from '../../context/AuthContext';
import { ESTADOS_TURNO } from '../../utils/constants';
import { 
  ChevronLeft, 
  ChevronRight, 
  Calendar as CalendarIcon, 
  Clock, 
  User, 
  Play, 
  FileText, 
  CheckCircle2, 
  AlertCircle, 
  X, 
  Activity, 
  ClipboardPlus,
  RefreshCw,
  Search,
  ExternalLink,
  ChevronDown
} from 'lucide-react';

const Agenda = () => {
  const navigate = useNavigate();


  const {
    vistaActual, setVistaActual,
    fechaSeleccionada, setFechaSeleccionada,
    turnos,
    turnoSeleccionado, setTurnoSeleccionado,
    cargandoTurnos, busquedaTurno, setBusquedaTurno,
    navegarTemporal, irAHoy, guardarTurnos
  } = useAgenda();


  const { medicoNombre } = useAuth();


  const tituloFecha = useMemo(() => {
    if (vistaActual === 'dia') {
      return fechaSeleccionada.toLocaleDateString('es-AR', {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric'
      });
    } else if (vistaActual === 'semana') {
      // Calculamos inicio y fin de la semana seleccionada
      const curr = new Date(fechaSeleccionada);
      const first = curr.getDate() - curr.getDay() + 1; // Lunes
      const last = first + 6; // Domingo
      
      const fechaLunes = new Date(curr.setDate(first));
      const fechaDomingo = new Date(curr.setDate(last));
      
      const opciones = { day: 'numeric', month: 'short' };
      return `Semana: ${fechaLunes.toLocaleDateString('es-AR', opciones)} - ${fechaDomingo.toLocaleDateString('es-AR', { ...opciones, year: 'numeric' })}`;
    } else if (vistaActual === 'mes') {
      return fechaSeleccionada.toLocaleDateString('es-AR', {
        month: 'long',
        year: 'numeric'
      });
    }
  }, [fechaSeleccionada, vistaActual]);


  const turnosFiltrados = useMemo(() => {
    return turnos.filter(t => {
      // Filtro de búsqueda textual (Nombre o DNI)
      if (busquedaTurno.trim() !== "") {
        const query = busquedaTurno.toLowerCase();
        const nombreMatch = t.pacienteNombre.toLowerCase().includes(query);
        const dniMatch = t.pacienteDni.includes(query);
        const motivoMatch = t.motivo?.toLowerCase().includes(query);
        if (!nombreMatch && !dniMatch && !motivoMatch) return false;
      }

      const fTurno = t.fecha;
      const fSel = fechaSeleccionada;

      if (vistaActual === 'dia') {
        return fTurno.getDate() === fSel.getDate() &&
               fTurno.getMonth() === fSel.getMonth() &&
               fTurno.getFullYear() === fSel.getFullYear();
      } 
      
      if (vistaActual === 'semana') {
        // Encontrar los límites de la semana de fSel
        const inicioSemana = new Date(fSel);
        const diaSemana = fSel.getDay(); 
        const diff = fSel.getDate() - diaSemana + (diaSemana === 0 ? -6 : 1); // Ajuste lunes como primer día
        inicioSemana.setDate(diff);
        inicioSemana.setHours(0, 0, 0, 0);

        const finSemana = new Date(inicioSemana);
        finSemana.setDate(inicioSemana.getDate() + 6);
        finSemana.setHours(23, 59, 59, 999);

        return fTurno >= inicioSemana && fTurno <= finSemana;
      }

      if (vistaActual === 'mes') {
        return fTurno.getMonth() === fSel.getMonth() &&
               fTurno.getFullYear() === fSel.getFullYear();
      }

      return true;
    }).sort((a, b) => a.fecha - b.fecha); // Siempre ordenados por hora cronológica
  }, [turnos, fechaSeleccionada, vistaActual, busquedaTurno]);

  // Estadísticas dinámicas de la vista actual (Día, Semana o Mes)
  const estadisticasVista = useMemo(() => {
    return {
      totales: turnosFiltrados.length,
      atendidos: turnosFiltrados.filter(t => t.estado === ESTADOS_TURNO.ATENDIDO).length,
      enEspera: turnosFiltrados.filter(t => t.estado === ESTADOS_TURNO.CONFIRMADO || t.estado === ESTADOS_TURNO.PENDIENTE || t.estado === ESTADOS_TURNO.EN_CURSO).length,
      cancelados: turnosFiltrados.filter(t => t.estado === ESTADOS_TURNO.CANCELADO).length
    };
  }, [turnosFiltrados]);

  // ==========================================
  // CONTROLADORES DE ACCIONES MÉDICAS
  // ==========================================
  const [showSelectionModal, setShowSelectionModal] = useState(false);
  const [turnoParaAtencion, setTurnoParaAtencion] = useState(null);

  const iniciarAtencion = (turno) => {
    // Cambiamos el estado del turno a "En Curso" localmente para reflejar que está siendo atendido
    const actualizados = turnos.map(t => {
      if (t.id === turno.id) {
        return { ...t, estado: ESTADOS_TURNO.EN_CURSO };
      }
      // Si había otro "En Curso", lo dejamos como Confirmado para que no haya dos simultáneos
      if (t.estado === ESTADOS_TURNO.EN_CURSO && t.id !== turno.id) {
        return { ...t, estado: ESTADOS_TURNO.CONFIRMADO };
      }
      return t;
    });
    guardarTurnos(actualizados);

    setTurnoParaAtencion(turno);
    setShowSelectionModal(true);
  };

  const seleccionarAtencion = (tipoAtencion) => {
    setShowSelectionModal(false);
    if (turnoParaAtencion) {
      const rutaDestino = tipoAtencion === 'Procedimiento' ? '/nuevo-procedimiento' : '/nueva-consulta';
      navigate(rutaDestino, { 
        state: { 
          idTurno: turnoParaAtencion.id, 
          dniIngresado: turnoParaAtencion.pacienteDni 
        } 
      });
    }
  };

  const verHistorialClinico = (turno) => {
    // Como el backend ahora manda el pacienteId, redirigimos directo sin consultar bases de datos extrañas
    navigate(`/pacientes/${turno.pacienteId}/historial`);
  };

  // Cambiar el estado del turno directamente desde la agenda (por ejemplo, check-in manual)
  const cambiarEstadoTurno = (turnoId, nuevoEstado) => {
    const actualizados = turnos.map(t => {
      if (t.id === turnoId) {
        return { ...t, estado: nuevoEstado };
      }
      return t;
    });
    guardarTurnos(actualizados);
    
    // Actualizamos el detalle seleccionado si es el mismo
    if (turnoSeleccionado && turnoSeleccionado.id === turnoId) {
      setTurnoSeleccionado(prev => ({ ...prev, estado: nuevoEstado }));
    }
  };

  // ==========================================
  // ESTILOS DE ESTADOS
  // ==========================================
  const obtenerEstiloEstado = (estado) => {
    switch (estado) {
      case ESTADOS_TURNO.ATENDIDO:
        return {
          badge: 'bg-slate-800 text-slate-400 border-slate-700/50',
          card: 'border-slate-800 opacity-60 hover:opacity-100 bg-slate-900/30',
          dot: 'bg-slate-600',
          texto: 'text-slate-500 line-through'
        };
      case ESTADOS_TURNO.CONFIRMADO:
        return {
          badge: 'bg-emerald-500/10 text-emerald-400 border-emerald-500/20',
          card: 'border-slate-800 hover:border-emerald-500/40 bg-slate-900/80 shadow-md shadow-emerald-950/5',
          dot: 'bg-emerald-400 animate-pulse',
          texto: 'text-slate-200 font-semibold'
        };
      case ESTADOS_TURNO.EN_CURSO:
        return {
          badge: 'bg-cyan-500/20 text-cyan-400 border-cyan-500/30 animate-pulse',
          card: 'border-cyan-500 bg-slate-900 shadow-lg shadow-cyan-950/20 border-l-4 ring-1 ring-cyan-500/20',
          dot: 'bg-cyan-400 ring-4 ring-cyan-500/25 animate-ping',
          texto: 'text-white font-bold'
        };
      case ESTADOS_TURNO.CANCELADO:
        return {
          badge: 'bg-red-500/10 text-red-400 border-red-500/20',
          card: 'border-slate-800 opacity-40 bg-slate-950',
          dot: 'bg-red-500',
          texto: 'text-slate-600 line-through'
        };
      case ESTADOS_TURNO.PENDIENTE:
      default:
        return {
          badge: 'bg-amber-500/10 text-amber-400 border-amber-500/20',
          card: 'border-slate-800 hover:border-amber-500/30 bg-slate-900/50',
          dot: 'bg-amber-400',
          texto: 'text-slate-300'
        };
    }
  };

  // ==========================================
  // RENDERIZADO DE LAS CUADRÍCULAS DE CALENDARIO
  // ==========================================

  // --- RENDER VISTA DÍA ---
  const renderVistaDia = () => {
    if (turnosFiltrados.length === 0) {
      return (
        <div className="flex flex-col items-center justify-center p-16 bg-slate-900/40 border border-slate-800 rounded-3xl text-center">
          <CalendarIcon size={48} className="text-slate-700 mb-4" />
          <h3 className="text-lg font-bold text-slate-400">Sin turnos para hoy</h3>
          <p className="text-xs text-slate-500 max-w-xs mt-1">Dispones de este día libre de consultas programadas.</p>
        </div>
      );
    }

    return (
      <div className="space-y-4">
        {/* Timeline Layout */}
        <div className="relative border-l border-slate-800 ml-4 md:ml-6 pl-6 md:pl-8 space-y-6 py-2">
          {turnosFiltrados.map((turno) => {
            const estilo = obtenerEstiloEstado(turno.estado);
            return (
              <div key={turno.id} className="relative group">
                {/* Indicador de Hora Flotante a la izquierda (en la línea vertical) */}
                <div className="absolute -left-[45px] md:-left-[53px] top-4 w-9 h-9 rounded-full bg-slate-950 border border-slate-800 flex items-center justify-center text-[10px] font-mono font-bold text-slate-400 shadow-md group-hover:border-cyan-500/50 transition-colors">
                  {turno.hora}
                </div>

                {/* Tarjeta del Turno */}
                <div 
                  onClick={() => setTurnoSeleccionado(turno)}
                  onDoubleClick={(e) => {
                    e.stopPropagation();
                    iniciarAtencion(turno);
                  }}
                  className={`p-5 rounded-2xl border transition-all cursor-pointer flex flex-col md:flex-row md:items-center justify-between gap-4 ${estilo.card}`}
                >
                  <div className="space-y-1">
                    <div className="flex items-center gap-3">
                      {/* Estado Dot */}
                      <span className={`w-2.5 h-2.5 rounded-full ${estilo.dot}`}></span>
                      <h4 className={`text-base tracking-wide ${estilo.texto}`}>
                        {turno.pacienteNombre}
                      </h4>
                      <span className={`px-2 py-0.5 rounded text-[8px] font-black uppercase tracking-wider border ${estilo.badge}`}>
                        {turno.estado}
                      </span>
                    </div>

                    <p className="text-xs text-slate-400 flex items-center gap-4">
                      <span className="font-mono">DNI: {turno.pacienteDni}</span>
                      <span className="text-slate-600">•</span>
                      <span className="flex items-center gap-1">
                        <Clock size={12} className="text-slate-500" /> {turno.duracion} min
                      </span>
                      <span className="text-slate-600">•</span>
                      <span className={`px-2 py-0.5 rounded-full text-[9px] font-bold ${
                        turno.tipo === 'Procedimiento' ? 'bg-purple-500/10 text-purple-400' : 
                        turno.tipo === 'Consulta' ? 'bg-cyan-500/10 text-cyan-400' :
                        'bg-slate-500/10 text-slate-400'
                      }`}>
                        {turno.tipo}
                      </span>
                    </p>
                    {turno.motivo && (
                      <p className="text-xs text-slate-500 max-w-xl truncate italic mt-1">
                        "{turno.motivo}"
                      </p>
                    )}
                  </div>

                  {/* Acciones Rápidas */}
                  <div className="flex items-center gap-2 self-end md:self-center">
                    <button 
                      onClick={(e) => {
                        e.stopPropagation();
                        verHistorialClinico(turno);
                      }}
                      title="Ver Historial Clínico"
                      className="p-2.5 bg-slate-900 border border-slate-800 hover:border-slate-700 rounded-xl text-slate-400 hover:text-slate-200 transition-colors shadow-sm"
                    >
                      <FileText size={16} />
                    </button>
                    
                    {turno.estado !== ESTADOS_TURNO.ATENDIDO && turno.estado !== ESTADOS_TURNO.CANCELADO && (
                      <button 
                        onClick={(e) => {
                          e.stopPropagation();
                          iniciarAtencion(turno);
                        }}
                        className={`px-4 py-2.5 rounded-xl font-bold text-xs flex items-center gap-1.5 transition-all shadow-md active:scale-95 ${
                          turno.estado === ESTADOS_TURNO.EN_CURSO
                          ? 'bg-cyan-500 text-slate-950 hover:bg-cyan-400 shadow-cyan-500/15'
                          : 'bg-slate-800 hover:bg-slate-700 text-cyan-400 border border-slate-700 hover:border-cyan-500/20'
                        }`}
                      >
                        <Play size={12} fill="currentColor" />
                        {turno.estado === ESTADOS_TURNO.EN_CURSO ? 'Retomar' : 'Iniciar'}
                      </button>
                    )}
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    );
  };

  // --- RENDER VISTA SEMANA ---
  const renderVistaSemana = () => {
    // Encontrar los 7 días de la semana activa
    const diasSemana = [];
    const fSel = new Date(fechaSeleccionada);
    const diaSem = fSel.getDay();
    const diff = fSel.getDate() - diaSem + (diaSem === 0 ? -6 : 1); // Lunes como inicio
    
    for (let i = 0; i < 7; i++) {
      const d = new Date(fSel);
      d.setDate(diff + i);
      diasSemana.push(d);
    }

    const nombresDias = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

    return (
      <div className="grid grid-cols-1 md:grid-cols-7 gap-4">
        {diasSemana.map((dia, index) => {
          const esHoy = new Date().toDateString() === dia.toDateString();
          const esSeleccionado = fechaSeleccionada.toDateString() === dia.toDateString();

          // Filtrar turnos estrictamente para este día particular de la semana, respetando la búsqueda global
          const turnosDelDia = turnosFiltrados.filter(t => 
            t.fecha.getDate() === dia.getDate() &&
            t.fecha.getMonth() === dia.getMonth() &&
            t.fecha.getFullYear() === dia.getFullYear()
          ).sort((a, b) => a.fecha - b.fecha);

          return (
            <div 
              key={index} 
              className={`rounded-2xl border flex flex-col p-4 min-h-[300px] transition-colors ${
                esHoy 
                ? 'bg-slate-900/60 border-cyan-500/20 shadow-lg shadow-cyan-950/5' 
                : esSeleccionado 
                  ? 'bg-slate-900/30 border-slate-700' 
                  : 'bg-slate-900/10 border-slate-800/80'
              }`}
            >
              {/* Encabezado del día */}
              <div className="border-b border-slate-800 pb-3 mb-3 text-center">
                <p className="text-[10px] uppercase tracking-widest font-black text-slate-500">
                  {nombresDias[index]}
                </p>
                <div className="flex items-center justify-center gap-1.5 mt-1">
                  <span className={`w-7 h-7 rounded-full flex items-center justify-center text-xs font-bold font-mono transition-colors ${
                    esHoy 
                    ? 'bg-cyan-500 text-slate-950' 
                    : 'text-slate-200'
                  }`}>
                    {dia.getDate()}
                  </span>
                </div>
              </div>

              {/* Listado de turnos cortos */}
              <div className="flex-1 space-y-2 overflow-y-auto max-h-[400px] pr-1 scrollbar-thin">
                {turnosDelDia.length > 0 ? (
                  turnosDelDia.map(turno => {
                    const estilo = obtenerEstiloEstado(turno.estado);
                    return (
                      <div 
                        key={turno.id}
                        onClick={() => setTurnoSeleccionado(turno)}
                        onDoubleClick={(e) => {
                          e.stopPropagation();
                          iniciarAtencion(turno);
                        }}
                        className={`p-2.5 rounded-xl border text-left cursor-pointer transition-all hover:scale-[1.02] ${estilo.card} text-xs`}
                      >
                        <div className="flex items-center justify-between mb-1.5">
                          <span className="font-bold text-[10px] text-slate-400 font-mono">
                            {turno.hora}
                          </span>
                          <span className={`px-1.5 py-0.2 rounded text-[7px] font-black uppercase border ${estilo.badge}`}>
                            {turno.estado}
                          </span>
                        </div>
                        <p className={`font-bold truncate text-slate-200`}>
                          {turno.pacienteNombre.split(' ')[0]} {turno.pacienteNombre.split(' ').slice(-1)[0]}
                        </p>
                        <p className="text-[9px] text-slate-500 truncate mt-0.5">
                          {turno.tipo} • {turno.motivo}
                        </p>
                      </div>
                    );
                  })
                ) : (
                  <p className="text-[10px] text-slate-600 italic text-center py-6">Sin turnos</p>
                )}
              </div>
            </div>
          );
        })}
      </div>
    );
  };

  // --- RENDER VISTA MES ---
  const renderVistaMes = () => {
    const fSel = new Date(fechaSeleccionada);
    const año = fSel.getFullYear();
    const mes = fSel.getMonth();

    // Lógica estándar para rellenar los días de la cuadrícula mensual
    const primerDiaMes = new Date(año, mes, 1);
    const ultimoDiaMes = new Date(año, mes + 1, 0);
    
    // Obtener qué día de la semana cae el primer día (ajuste lunes = 0)
    let primerDiaSemana = primerDiaMes.getDay();
    primerDiaSemana = primerDiaSemana === 0 ? 6 : primerDiaSemana - 1;

    const totalDiasMes = ultimoDiaMes.getDate();
    const celdas = [];

    // Rellenamos días vacíos al inicio (mes anterior)
    const ultimoDiaMesAnterior = new Date(año, mes, 0).getDate();
    for (let i = primerDiaSemana - 1; i >= 0; i--) {
      celdas.push({
        dia: ultimoDiaMesAnterior - i,
        esMesActual: false,
        fecha: new Date(año, mes - 1, ultimoDiaMesAnterior - i)
      });
    }

    // Rellenamos los días del mes actual
    for (let i = 1; i <= totalDiasMes; i++) {
      celdas.push({
        dia: i,
        esMesActual: true,
        fecha: new Date(año, mes, i)
      });
    }

    // Rellenamos días vacíos al final (mes siguiente) para completar múltiplos de 7 (filas completas)
    const celdasFaltantes = 42 - celdas.length;
    for (let i = 1; i <= celdasFaltantes; i++) {
      celdas.push({
        dia: i,
        esMesActual: false,
        fecha: new Date(año, mes + 1, i)
      });
    }

    const nombresDiasSemana = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];

    return (
      <div className="bg-slate-900/20 border border-slate-800/80 rounded-3xl p-5">
        {/* Cabecera de días de la semana */}
        <div className="grid grid-cols-7 gap-2 text-center mb-4">
          {nombresDiasSemana.map((d, idx) => (
            <span key={idx} className="text-[10px] uppercase tracking-widest font-black text-slate-500">
              {d.slice(0, 3)}
            </span>
          ))}
        </div>

        {/* Cuadrícula de celdas */}
        <div className="grid grid-cols-7 gap-2">
          {celdas.map((celda, idx) => {
            const esHoy = new Date().toDateString() === celda.fecha.toDateString();
            const esSeleccionado = fechaSeleccionada.toDateString() === celda.fecha.toDateString();

            // Filtrar turnos programados en este día específico, respetando la búsqueda global
            const turnosDelDia = turnosFiltrados.filter(t => 
              t.fecha.getDate() === celda.fecha.getDate() &&
              t.fecha.getMonth() === celda.fecha.getMonth() &&
              t.fecha.getFullYear() === celda.fecha.getFullYear()
            );

            return (
              <div 
                key={idx}
                onClick={() => {
                  setFechaSeleccionada(celda.fecha);
                  setVistaActual('dia'); // Al hacer clic en un día del mes, bajamos al detalle de ese Día
                }}
                className={`min-h-[85px] p-2 rounded-xl border cursor-pointer flex flex-col justify-between transition-all hover:bg-slate-800/30 ${
                  celda.esMesActual ? 'text-slate-200' : 'text-slate-600 bg-slate-950/20 border-transparent'
                } ${
                  esHoy 
                  ? 'border-cyan-500/30 bg-slate-900/40 shadow-sm' 
                  : esSeleccionado 
                    ? 'border-slate-600 bg-slate-900/20' 
                    : 'border-slate-800/50 bg-slate-900/10'
                }`}
              >
                {/* Número de día */}
                <span className={`text-[11px] font-mono font-bold ${
                  esHoy ? 'text-cyan-400 bg-cyan-500/10 px-2 py-0.5 rounded-full inline-block w-fit' : ''
                }`}>
                  {celda.dia}
                </span>

                {/* Indicadores de turnos en mini burbujas */}
                <div className="space-y-1">
                  {turnosDelDia.length > 0 && (
                    <div className="flex flex-col gap-1">
                      {turnosDelDia.slice(0, 2).map((t, tIdx) => {
                        const estilo = obtenerEstiloEstado(t.estado);
                        return (
                          <div 
                            key={tIdx} 
                            className={`px-1.5 py-0.2 rounded text-[7px] truncate font-bold border ${estilo.badge}`}
                          >
                            {t.hora} {t.pacienteNombre.split(' ')[0]}
                          </div>
                        );
                      })}
                      {turnosDelDia.length > 2 && (
                        <div className="text-[7px] text-slate-500 font-bold text-center">
                          +{turnosDelDia.length - 2} más
                        </div>
                      )}
                    </div>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      </div>
    );
  };

  // ==========================================
  // VISTA FINAL DEL COMPONENTE
  // ==========================================
  return (
    <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
      
      {/* 1. MENÚ LATERAL */}
      <Sidebar />

      {/* 2. AREA DE CONTENIDO */}
      <div className="flex-1 flex flex-col min-w-0">
        
        {/* Cabecera Superior */}
        <Header paginaActual='Agenda Médica'/>

        {/* Zona Scrollable Principal */}
        <main className="p-8 overflow-y-auto flex-1">
          
          <div className="max-w-7xl mx-auto space-y-8">
            
            {/* ENCABEZADO DE SECCIÓN CON SALUDO */}
            <div className="flex flex-col md:flex-row justify-between items-start md:items-end gap-4 border-b border-slate-900 pb-6">
              <div>
                <h1 className="text-4xl font-bold text-white tracking-tight">Mi Agenda</h1>
                <p className="text-slate-500 mt-1">Organice y gestione sus consultas del día, {medicoNombre}.</p>
              </div>
              
              {/* Barra de Búsqueda rápida de turnos */}
              <div className="relative w-full md:w-72 flex items-center gap-2">
                {cargandoTurnos && (
                  <RefreshCw size={18} className="animate-spin text-cyan-500" />
                )}
                <div className="relative flex-1">
                  <Search size={16} className="absolute left-4 top-3.5 text-slate-500" />
                  <input 
                    type="text"
                    placeholder="Buscar paciente o motivo..."
                    value={busquedaTurno}
                    onChange={(e) => setBusquedaTurno(e.target.value)}
                    className="w-full bg-slate-900 border border-slate-800 focus:border-cyan-500/50 rounded-2xl pl-11 pr-4 py-3 text-sm text-slate-300 placeholder-slate-600 focus:outline-none transition-colors"
                  />
                  {busquedaTurno && (
                    <button 
                      onClick={() => setBusquedaTurno('')}
                      className="absolute right-4 top-3.5 text-slate-500 hover:text-slate-300"
                    >
                      <X size={14} />
                    </button>
                  )}
                </div>
              </div>
            </div>

            {/* ESTADÍSTICAS RÁPIDAS DE LA VISTA */}
            <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
              <div className="bg-slate-900/60 p-5 rounded-2xl border border-slate-800 flex items-center justify-between">
                <div>
                  <p className="text-[10px] text-slate-500 uppercase tracking-widest font-black">Total en la Vista</p>
                  <h3 className="text-2xl font-bold text-white mt-1 font-mono">{estadisticasVista.totales}</h3>
                </div>
                <div className="w-10 h-10 rounded-xl bg-slate-800/80 border border-slate-700 flex items-center justify-center text-slate-400">
                  <CalendarIcon size={18} />
                </div>
              </div>

              <div className="bg-slate-900/60 p-5 rounded-2xl border border-slate-800 flex items-center justify-between">
                <div>
                  <p className="text-[10px] text-emerald-500 uppercase tracking-widest font-black">Atendidos</p>
                  <h3 className="text-2xl font-bold text-emerald-400 mt-1 font-mono">{estadisticasVista.atendidos}</h3>
                </div>
                <div className="w-10 h-10 rounded-xl bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center text-emerald-400">
                  <CheckCircle2 size={18} />
                </div>
              </div>

              <div className="bg-slate-900/60 p-5 rounded-2xl border border-slate-800 flex items-center justify-between">
                <div>
                  <p className="text-[10px] text-cyan-500 uppercase tracking-widest font-black">En Espera / Pendiente</p>
                  <h3 className="text-2xl font-bold text-cyan-400 mt-1 font-mono">{estadisticasVista.enEspera}</h3>
                </div>
                <div className="w-10 h-10 rounded-xl bg-cyan-500/10 border border-cyan-500/20 flex items-center justify-center text-cyan-400">
                  <Clock size={18} />
                </div>
              </div>

              <div className="bg-slate-900/60 p-5 rounded-2xl border border-slate-800 flex items-center justify-between">
                <div>
                  <p className="text-[10px] text-red-500 uppercase tracking-widest font-black">Cancelados</p>
                  <h3 className="text-2xl font-bold text-red-400 mt-1 font-mono">{estadisticasVista.cancelados}</h3>
                </div>
                <div className="w-10 h-10 rounded-xl bg-red-500/10 border border-red-500/20 flex items-center justify-center text-red-400">
                  <AlertCircle size={18} />
                </div>
              </div>
            </div>

            {/* TABLERO DE CONTROLES DE LA AGENDA */}
            <div className="bg-slate-900 p-4 md:p-6 rounded-3xl border border-slate-800 shadow-sm flex flex-col md:flex-row justify-between items-center gap-4">
              
              {/* Controles de Navegación del Calendario */}
              <div className="flex items-center gap-3">
                <button 
                  onClick={() => navegarTemporal(-1)}
                  className="p-3 bg-slate-950 border border-slate-850 hover:border-slate-700 text-slate-400 hover:text-slate-200 rounded-xl transition-all shadow-sm active:scale-95"
                >
                  <ChevronLeft size={16} />
                </button>
                
                <h3 className="text-base md:text-lg font-bold text-white capitalize min-w-[200px] text-center">
                  {tituloFecha}
                </h3>

                <button 
                  onClick={() => navegarTemporal(1)}
                  className="p-3 bg-slate-950 border border-slate-850 hover:border-slate-700 text-slate-400 hover:text-slate-200 rounded-xl transition-all shadow-sm active:scale-95"
                >
                  <ChevronRight size={16} />
                </button>

                <button 
                  onClick={irAHoy}
                  className="px-4 py-3 bg-slate-950 border border-slate-850 hover:border-slate-700 text-slate-400 hover:text-slate-200 rounded-xl text-xs font-bold transition-all shadow-sm active:scale-95"
                >
                  Hoy
                </button>
              </div>

              {/* Selector de Vistas (Tabs) */}
              <div className="bg-slate-950 p-1.5 rounded-2xl border border-slate-850 flex items-center shadow-inner">
                {['dia', 'semana', 'mes'].map((vista) => (
                  <button
                    key={vista}
                    onClick={() => {
                      setVistaActual(vista);
                      setTurnoSeleccionado(null);
                    }}
                    className={`px-5 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider transition-all ${
                      vistaActual === vista
                      ? 'bg-cyan-500 text-slate-950 font-black shadow-lg shadow-cyan-500/10'
                      : 'text-slate-500 hover:text-slate-300'
                    }`}
                  >
                    {vista}
                  </button>
                ))}
              </div>

            </div>

            {/* ÁREA DE CONTENIDO PRINCIPAL (CALENDARIOS Y LISTADOS) */}
            <div className="relative">
              {cargandoTurnos ? (
                <div className="flex flex-col items-center justify-center p-24 bg-slate-900/20 border border-slate-800 rounded-3xl text-center">
                  <RefreshCw size={40} className="animate-spin text-cyan-500 mb-4" />
                  <h3 className="text-lg font-bold text-slate-400">Cargando tu agenda...</h3>
                  <p className="text-sm text-slate-500 max-w-xs mt-2">Conectando con el sistema central para recuperar tus turnos.</p>
                </div>
              ) : (
                <>
                  {vistaActual === 'dia' && renderVistaDia()}
                  {vistaActual === 'semana' && renderVistaSemana()}
                  {vistaActual === 'mes' && renderVistaMes()}
                </>
              )}
            </div>

          </div>
        </main>
      </div>

      {/* ==========================================
          MODAL DETALLE DE TURNO (SLIDE-OVER / DRAWER)
          ========================================== */}
      {turnoSeleccionado && (() => {
        const estilo = obtenerEstiloEstado(turnoSeleccionado.estado);
        return (
          <div className="fixed inset-0 z-50 flex justify-end bg-slate-950/60 backdrop-blur-sm animate-fade-in">
            {/* Fondo clickeable para cerrar */}
            <div className="flex-1" onClick={() => setTurnoSeleccionado(null)}></div>
            
            {/* Drawer Lateral */}
            <div className="w-full max-w-md bg-slate-900 border-l border-slate-850 h-screen flex flex-col shadow-2xl animate-slide-left p-8">
              
              {/* Header Drawer */}
              <div className="flex justify-between items-center border-b border-slate-800 pb-6 mb-6">
                <div className="flex items-center gap-3">
                  <span className={`w-3 h-3 rounded-full ${estilo.dot}`}></span>
                  <h3 className="text-xl font-bold text-white">Detalle de Turno</h3>
                </div>
                <button 
                  onClick={() => setTurnoSeleccionado(null)}
                  className="p-2 hover:bg-slate-800 rounded-xl text-slate-400 hover:text-slate-200 transition-colors"
                >
                  <X size={20} />
                </button>
              </div>

              {/* Contenido Detalle */}
              <div className="flex-1 space-y-8 overflow-y-auto pr-1">
                
                {/* Datos del Paciente */}
                <div className="bg-slate-950 p-6 rounded-2xl border border-slate-800/80 space-y-4">
                  <h4 className="text-[10px] text-slate-500 uppercase tracking-widest font-black flex items-center gap-1.5">
                    <User size={12} className="text-cyan-500" /> Datos del Paciente
                  </h4>
                  
                  <div>
                    <h2 className="text-xl font-bold text-white">
                      {turnoSeleccionado.pacienteNombre}
                    </h2>
                    <p className="text-sm font-mono text-cyan-400 mt-1">
                      DNI: {turnoSeleccionado.pacienteDni}
                    </p>
                  </div>
                </div>

                {/* Detalles de la Cita */}
                <div className="space-y-4">
                  <h4 className="text-[10px] text-slate-500 uppercase tracking-widest font-black flex items-center gap-1.5">
                    <CalendarIcon size={12} className="text-cyan-500" /> Información de la Cita
                  </h4>

                  <div className="grid grid-cols-2 gap-4">
                    <div className="bg-slate-900 border border-slate-850 p-4 rounded-xl">
                      <p className="text-[9px] text-slate-500 uppercase font-black">Fecha</p>
                      <p className="text-sm font-bold text-slate-300 mt-1">
                        {turnoSeleccionado.fecha.toLocaleDateString('es-AR', {
                          day: 'numeric',
                          month: 'short',
                          year: 'numeric'
                        })}
                      </p>
                    </div>

                    <div className="bg-slate-900 border border-slate-850 p-4 rounded-xl">
                      <p className="text-[9px] text-slate-500 uppercase font-black">Horario</p>
                      <p className="text-sm font-bold text-slate-300 mt-1">
                        {turnoSeleccionado.hora} HS
                      </p>
                    </div>

                    <div className="bg-slate-900 border border-slate-850 p-4 rounded-xl">
                      <p className="text-[9px] text-slate-500 uppercase font-black">Tipo de Atención</p>
                      <span className={`inline-block px-2.5 py-0.5 rounded-full text-[9px] font-bold mt-1.5 ${
                        turnoSeleccionado.tipo === 'Procedimiento' ? 'bg-purple-500/10 text-purple-400' : 
                        turnoSeleccionado.tipo === 'Consulta' ? 'bg-cyan-500/10 text-cyan-400' :
                        'bg-slate-500/10 text-slate-400'
                      }`}>
                        {turnoSeleccionado.tipo}
                      </span>
                    </div>

                    <div className="bg-slate-900 border border-slate-850 p-4 rounded-xl">
                      <p className="text-[9px] text-slate-500 uppercase font-black">Estado Actual</p>
                      <span className={`inline-block px-2.5 py-0.5 rounded text-[9px] font-black uppercase mt-1.5 border ${estilo.badge}`}>
                        {turnoSeleccionado.estado}
                      </span>
                    </div>
                  </div>

                  {turnoSeleccionado.motivo && (
                    <div className="bg-slate-950 p-5 rounded-2xl border border-slate-800">
                      <p className="text-[9px] text-slate-500 uppercase font-black mb-1">Motivo Médico</p>
                      <p className="text-xs text-slate-300 leading-relaxed italic">
                        "{turnoSeleccionado.motivo}"
                      </p>
                    </div>
                  )}
                </div>



              </div>

              {/* Botones de Acción Médica en el Footer del Drawer */}
              <div className="border-t border-slate-800 pt-6 mt-6 space-y-3">
                <button
                  onClick={() => {
                    setTurnoSeleccionado(null);
                    verHistorialClinico(turnoSeleccionado);
                  }}
                  className="w-full bg-slate-950 hover:bg-slate-800 text-slate-300 py-3.5 rounded-2xl font-bold text-sm flex items-center justify-center gap-2 border border-slate-800 hover:border-slate-750 transition-colors shadow-sm"
                >
                  <FileText size={16} /> Ver Historial Clínico
                </button>

                {turnoSeleccionado.estado !== ESTADOS_TURNO.ATENDIDO && turnoSeleccionado.estado !== ESTADOS_TURNO.CANCELADO ? (
                  <button
                    onClick={() => {
                      setTurnoSeleccionado(null);
                      iniciarAtencion(turnoSeleccionado);
                    }}
                    className="w-full bg-cyan-500 hover:bg-cyan-400 text-slate-950 py-4 rounded-2xl font-bold text-sm flex items-center justify-center gap-2 transition-colors shadow-lg shadow-cyan-500/15"
                  >
                    <Play size={16} fill="currentColor" /> Iniciar Atención
                  </button>
                ) : (
                  <div className="bg-slate-950 border border-slate-800/80 p-4 rounded-2xl text-center text-xs text-slate-500 font-semibold italic">
                    Turno finalizado o cancelado. No requiere acciones médicas adicionales.
                  </div>
                )}
              </div>

            </div>
          </div>
        );
      })()}

      {/* ==========================================
          MODAL DE SELECCIÓN DE TIPO DE ATENCIÓN (CONSULTA VS PROCEDIMIENTO)
          ========================================== */}
      {showSelectionModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/80 backdrop-blur-sm animate-fade-in">
          <div className="bg-slate-900 border border-slate-800 rounded-3xl p-8 max-w-md w-full shadow-2xl relative">
            
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

    </div>
  );
};

export default Agenda;
