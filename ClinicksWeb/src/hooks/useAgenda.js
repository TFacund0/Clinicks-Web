import { useState, useEffect } from 'react';
import agendaService from '../services/agendaService';
import { ESTADOS_TURNO } from '../utils/constants';

/**
 * Hook para gestionar el estado y la lógica de la Agenda médica.
 */
export const useAgenda = () => {
  const [vistaActual, setVistaActual] = useState('dia'); // 'dia', 'semana', 'mes'
  const [fechaSeleccionada, setFechaSeleccionada] = useState(new Date());
  const [turnos, setTurnos] = useState([]);
  const [turnoSeleccionado, setTurnoSeleccionado] = useState(null);
  const [cargandoTurnos, setCargandoTurnos] = useState(true);
  const [busquedaTurno, setBusquedaTurno] = useState('');

  const mesSeleccionado = fechaSeleccionada.getMonth();
  const anioSeleccionado = fechaSeleccionada.getFullYear();


  useEffect(() => {
    const cargarDatos = async () => {
      try {
        setCargandoTurnos(true);

        const primerDia = new Date(anioSeleccionado, mesSeleccionado, 1);
        const ultimoDia = new Date(anioSeleccionado, mesSeleccionado + 1, 0);

        const turnosBackend = await agendaService.obtenerTurnosMedico(primerDia, ultimoDia);
        
        const turnosMapeados = turnosBackend.map(t => {
          const fechaObj = new Date(t.fechaTurno);
          return {
            id: t.idTurno,
            pacienteId: t.idPaciente,
            pacienteNombre: t.pacienteNombreCompleto || "Paciente sin nombre",
            pacienteDni: t.dniPaciente || "Sin DNI",
            fecha: fechaObj,
            hora: fechaObj.toLocaleTimeString('es-AR', { hour: '2-digit', minute: '2-digit' }),
            duracion: 20,
            tipo: t.idProcedimiento ? "Procedimiento" : (t.idConsulta ? "Consulta" : "A definir"),
            motivo: t.motivo || "Sin motivo especificado",
            estado: t.estado || ESTADOS_TURNO.PENDIENTE
          };
        });

        setTurnos(turnosMapeados);

      } catch (err) {
        console.error("Error al inicializar la agenda", err);
      } finally {
        setCargandoTurnos(false);
      }
    };

    cargarDatos();
  }, [mesSeleccionado, anioSeleccionado]); 


  const navegarTemporal = (direccion) => {
    const nueva = new Date(fechaSeleccionada);
    if (vistaActual === 'dia') {
      nueva.setDate(fechaSeleccionada.getDate() + direccion);
    } else if (vistaActual === 'semana') {
      nueva.setDate(fechaSeleccionada.getDate() + (direccion * 7));
    } else if (vistaActual === 'mes') {
      nueva.setMonth(fechaSeleccionada.getMonth() + direccion);
    }
    setFechaSeleccionada(nueva);
    setTurnoSeleccionado(null);
  };

  const irAHoy = () => {
    setFechaSeleccionada(new Date());
    setTurnoSeleccionado(null);
  };


  const guardarTurnos = (nuevosTurnos) => {
    setTurnos(nuevosTurnos);
  };

  return {
    vistaActual,
    setVistaActual,
    fechaSeleccionada,
    setFechaSeleccionada,
    turnos,
    turnoSeleccionado,
    setTurnoSeleccionado,
    cargandoTurnos,
    busquedaTurno,
    setBusquedaTurno,
    navegarTemporal,
    irAHoy,
    guardarTurnos
  };
};
