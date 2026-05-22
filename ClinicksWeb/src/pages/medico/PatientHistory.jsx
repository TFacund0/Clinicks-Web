// src/pages/medico/PatientHistory.jsx
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, User, Activity, FileText, Clock } from 'lucide-react';
import PageLayout from '../../components/PageLayout';
import { usePatientHistory } from '../../hooks/usePatientHistory';

const PatientHistory = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  // SOC-1: Usar API real en lugar de historialEjemplo hardcodeado
  const { paciente, historial, cargando, error } = usePatientHistory(id);

  if (cargando) {
      return (
          <PageLayout title="Historial Clínico">
              <div className="flex h-full items-center justify-center text-slate-400">
                  <div className="animate-pulse flex flex-col items-center">
                      <Activity className="animate-spin mb-4" size={32} />
                      <p>Cargando historial...</p>
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
    <PageLayout title={`Historial: ${paciente.nombre} ${paciente.apellido || ''}`}>
      {/* Botón para volver al listado de pacientes */}
      <button 
        onClick={() => navigate('/pacientes')}
        className="flex items-center gap-2 text-slate-500 hover:text-cyan-400 transition-colors mb-6 text-sm font-medium"
      >
        <ArrowLeft size={16} /> Volver al listado
      </button>

      {/* CABECERA DEL PACIENTE */}
      <div className="bg-slate-900 border border-slate-800 rounded-3xl p-8 mb-8 flex justify-between items-center shadow-2xl">
        <div>
          <p className="text-slate-500 text-[10px] uppercase font-bold tracking-widest mb-1">Nombre Completo</p>
          <p className="font-bold text-lg">{paciente.nombre} {paciente.apellido}</p>
        </div>
        <div>
          <p className="text-slate-500 text-[10px] uppercase font-bold tracking-widest mb-1">DNI</p>
          <p className="font-mono text-slate-300">{paciente.dni}</p>
        </div>
        <div>
          <p className="text-slate-500 text-[10px] uppercase font-bold tracking-widest mb-1">Fecha de Nacimiento</p>
          <p className="font-mono text-slate-300">
            {paciente.fechaNacimiento ? new Date(paciente.fechaNacimiento).toLocaleDateString() : 'No registrada'}
          </p>
        </div>
        <div>
          <p className="text-slate-500 text-[10px] uppercase font-bold tracking-widest mb-1">Teléfono</p>
          <p className="font-mono text-slate-300">{paciente.telefono || 'No registrado'}</p>
        </div>
        <div>
          <p className="text-slate-500 text-[10px] uppercase font-bold tracking-widest mb-1">Email</p>
          <p className="text-slate-300">{paciente.email || 'No registrado'}</p>
        </div>
      </div>

      {/* LISTADO DE CONSULTAS */}
      <div className="bg-slate-900 border border-slate-800 rounded-3xl overflow-hidden">
        <h2 className="text-lg font-bold flex items-center gap-2 p-6 border-b border-slate-800">
          <Clock size={20} className="text-cyan-500" /> Registro de Consultas
        </h2>
        
        {historial.length === 0 ? (
          <div className="p-12 text-center text-slate-500">No hay consultas registradas para este paciente.</div>
        ) : (
          historial.map((consulta) => (
            <div key={consulta.id} className="p-6 border-b border-slate-800 last:border-0 hover:bg-slate-800/20 transition-colors">
              <div className="flex justify-between items-start mb-4">
                <div>
                  <span className="text-cyan-500 font-mono text-sm border border-cyan-500/20 bg-cyan-500/10 px-2 py-1 rounded">
                    {new Date(consulta.fechaconsulta).toLocaleDateString()}
                  </span>
                  <h4 className="text-lg font-bold text-white mt-2">{consulta.motivo}</h4>
                </div>
              </div>
              <div className="grid grid-cols-2 gap-4 text-sm bg-slate-950/50 p-4 rounded-xl border border-slate-800/50">
                <div>
                  <span className="text-slate-500 font-bold">Diagnóstico: </span>
                  <span className="text-slate-300">{consulta.diagnostico}</span>
                </div>
                <div>
                  <span className="text-slate-500 font-bold">Tratamiento: </span>
                  <span className="text-slate-300">{consulta.tratamiento}</span>
                </div>
                {consulta.observaciones && (
                  <div className="col-span-2">
                    <span className="text-slate-500 font-bold">Observaciones: </span>
                    <span className="text-slate-300">{consulta.observaciones}</span>
                  </div>
                )}
                {consulta.recomendacion && (
                  <div className="col-span-2">
                    <span className="text-slate-500 font-bold">Recomendación: </span>
                    <span className="text-slate-300">{consulta.recomendacion}</span>
                  </div>
                )}
              </div>
            </div>
          ))
        )}
      </div>
    </PageLayout>
  );
};

export default PatientHistory;