// src/pages/medico/PatientHistory.jsx
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';

import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Clock, FileText, Activity } from 'lucide-react';

// Vista que muestra la línea de tiempo cronológica con todas las atenciones médicas de un paciente específico.
const PatientHistory = () => {
  // Extrae el "id" directamente desde la URL del navegador (ej: si la URL es /pacientes/5/historial, id vale "5").
  const { id } = useParams(); 
  
  // Herramienta para movernos de pantalla sin recargar.
  const navigate = useNavigate();

  // Datos temporales de prueba (Mock). Más adelante, usaremos el 'id' de arriba para pedirle estos datos reales a C#.
  const historialEjemplo = {
    nombre: "Juan Pérez",
    dni: "32.123.456",
    consultas: [
      { id: 1, fecha: "20/04/2024", motivo: "Control post-operatorio", diagnostico: "Evolución favorable", medico: "Dr. Gregory House" },
      { id: 2, fecha: "01/03/2024", motivo: "Chequeo anual", diagnostico: "Salud general estable", medico: "Dr. Gregory House" }
    ]
  };

  return (
    <div className="flex h-screen w-full bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />
      <div className="flex-1 flex flex-col min-w-0">
        <Header paginaActual='Historial Clínico de paciente'/>
        
        <main className="flex-1 p-8 overflow-y-auto">
          <div className="max-w-4xl mx-auto">
            
            {/* BOTÓN VOLVER: El navigate(-1) funciona como el botón de "Atrás" del navegador */}
            <button 
              onClick={() => navigate(-1)}
              className="flex items-center gap-2 text-slate-500 hover:text-cyan-400 transition-colors mb-6 text-sm font-medium"
            >
              <ArrowLeft size={16} /> Volver al listado
            </button>

            {/* CABECERA DEL PACIENTE: Muestra los datos principales estáticos del paciente */}
            <div className="bg-slate-900 border border-slate-800 rounded-3xl p-8 mb-8 flex justify-between items-center shadow-2xl">
              <div>
                <span className="text-cyan-500 text-xs font-bold uppercase tracking-widest">Historial Clínico</span>
                <h1 className="text-4xl font-black text-white mt-1">{historialEjemplo.nombre}</h1>
                <p className="text-slate-500 mt-1 font-mono">DNI: {historialEjemplo.dni} • Paciente #{id}</p>
              </div>
              <div className="h-16 w-16 rounded-2xl bg-cyan-500/10 border border-cyan-500/20 flex items-center justify-center text-cyan-500">
                <Activity size={32} />
              </div>
            </div>

            {/* LISTADO DE CONSULTAS (Línea de Tiempo / Timeline) */}
            <div className="space-y-6">
              <h2 className="text-lg font-bold flex items-center gap-2 mb-4">
                <Clock size={20} className="text-slate-400" /> Registro de Consultas
              </h2>

              {/* Recorremos el arreglo de consultas para dibujar un "nodo" en la línea de tiempo por cada atención */}
              {historialEjemplo.consultas.map((consulta, index) => (
                <div key={consulta.id} className="relative pl-8 group">
                  
                  {/* Dibuja la línea vertical conectora, excepto en el último elemento de la lista */}
                  {index !== historialEjemplo.consultas.length - 1 && (
                    <div className="absolute left-2.75 top-8 -bottom-6 w-0.5 bg-slate-800 group-hover:bg-cyan-500/30 transition-colors"></div>
                  )}
                  
                  {/* Dibuja el circulito indicador en la línea */}
                  <div className="absolute left-0 top-1 w-6 h-6 rounded-full bg-slate-950 border-2 border-slate-800 group-hover:border-cyan-500 transition-colors flex items-center justify-center">
                    <div className="w-2 h-2 rounded-full bg-slate-700 group-hover:bg-cyan-500 transition-colors"></div>
                  </div>

                  {/* Tarjeta con los detalles específicos de esa consulta en particular */}
                  <div className="bg-slate-900/50 border border-slate-800 p-6 rounded-2xl group-hover:border-slate-700 group-hover:bg-slate-900 transition-all shadow-sm">
                    <div className="flex justify-between items-start mb-4">
                      <span className="text-xs font-bold text-cyan-500 bg-cyan-500/10 px-3 py-1 rounded-full">{consulta.fecha}</span>
                      <span className="text-xs text-slate-500 font-medium italic">Atendido por {consulta.medico}</span>
                    </div>
                    <h3 className="text-white font-bold mb-2 flex items-center gap-2">
                      <FileText size={16} className="text-slate-400" /> {consulta.motivo}
                    </h3>
                    <p className="text-slate-400 text-sm leading-relaxed bg-slate-950/50 p-4 rounded-xl border border-slate-800/50">
                      <strong className="text-slate-300 block mb-1">Diagnóstico:</strong>
                      {consulta.diagnostico}
                    </p>
                  </div>
                </div>
              ))}
            </div>

          </div>
        </main>
      </div>
    </div>
  );
};

export default PatientHistory;