// src/pages/medico/NewConsultation.jsx
import React from 'react';
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { ClipboardList, User, FileText, Save, X } from 'lucide-react';
import { useNewConsultation } from '../../hooks/useNewConsultation'; 
import { useLocation } from 'react-router-dom';

// Componente visual principal para registrar una nueva consulta médica.
const NewConsultation = () => {
  // Destructuramos (extraemos) todo lo que nos devuelve el hook
  const location = useLocation();
  const dniEntrante = location.state?.dniIngresado || '';

  const {
    formData,
    errors,
    showSuccess,
    errorMsg,
    isSubmitting,
    handleChange,
    handleSubmit,
    handleCancel
  } = useNewConsultation(dniEntrante);

  return (
    // Contenedor principal que estructura la pantalla: Sidebar a la izquierda y contenido a la derecha.
    <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />
      <div className="flex-1 flex flex-col">
        <Header paginaActual='Crear Consulta'/>
        <main className="p-8 overflow-y-auto">
          
          <div className="flex justify-between items-end mb-8">
            <div>
              <h1 className="text-4xl font-bold text-white">Buenos días</h1>
              <p className="text-slate-500 mt-1">Aquí tienes un resumen de tu actividad de hoy.</p>
            </div> 
          </div>

          {/* Formulario principal: el evento onSubmit está conectado a la función del Hook */}
          <form onSubmit={handleSubmit} className="w-full">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
              
              {/* Panel Izquierdo: Datos básicos de contexto (DNI, Motivo, Fecha) */}
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                    <User size={20} className="text-cyan-400" />
                    Contexto de la Consulta
                </h3>
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Fecha de la consulta</label>
                    <input type="date" name="fechaconsulta" value={formData.fechaconsulta} onChange={handleChange} className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors scheme-dark" />
                </div>
                
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Motivo de Consulta</label>
                    <textarea name="motivo" value={formData.motivo} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Descripción breve..." />
                    {errors.motivo && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.motivo}</p>}      
                </div>
              </div>

              {/* Panel Derecho: Datos de la evaluación médica */}
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                    <User size={20} className="text-cyan-400" />
                    Diagnóstico y Tratamiento
                </h3>
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Diagnóstico Presuntivo</label>
                    <input type="text" name="diagnostico" value={formData.diagnostico} onChange={handleChange} className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors" placeholder="Resultado de la evaluación..." />
                    {errors.diagnostico && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.diagnostico}</p>}
                </div>
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Recomendaciones</label>
                    <textarea name="recomendacion" value={formData.recomendacion} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Recomendaciones para el paciente..." />
                    {errors.recomendacion && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.recomendacion}</p>}
                </div>
                
              </div>
            </div>

            {/* Fila Inferior: Campo extenso para notas privadas */}
            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 mb-8">
    
    {/* TÍTULO GENERAL (OPCIONAL) */}
    <h3 className="text-lg font-bold flex items-center gap-2 mb-6 border-b border-slate-800 pb-3">
        <FileText size={20} className="text-cyan-400" />
        Detalles Finales de la Consulta
    </h3>

    {/* GRID PARA LAS DOS COLUMNAS */}
    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        
        {/* COLUMNA IZQUIERDA: Observaciones */}
        <div className="space-y-3">
            <label className="block text-xs text-slate-500 uppercase font-bold">
                Observaciones Adicionales
            </label>
            <textarea 
                name="observaciones"
                value={formData.observaciones}
                onChange={handleChange}
                rows="4"
                className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none"
                placeholder="Notas internas del médico..."
            />
            {errors.observaciones && (
                <p className="text-red-500 text-[10px] font-bold italic">{errors.observaciones}</p>
            )}
        </div>

        {/* COLUMNA DERECHA: Recomendaciones */}
        <div className="space-y-3">
            <label className="block text-xs text-slate-500 uppercase font-bold">
                tratamiento
            </label>
            <textarea 
                name="tratamiento"
                value={formData.tratamiento}
                onChange={handleChange}
                rows="4"
                className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none"
                placeholder="Indicaciones para el paciente..."
            />
            {errors.tratamiento && (
                <p className="text-red-500 text-[10px] font-bold italic">{errors.tratamiento}</p>
            )}
        </div>

    </div>
</div>
            
            {/* Controles de Acción */}
            <div className="flex gap-4">
                {/* El botón se deshabilita (disabled) si ya se está enviando para evitar doble carga */}
                <button 
                  type="submit" 
                  disabled={isSubmitting} 
                  className={`px-8 py-3 rounded-xl font-bold flex items-center gap-2 transition-colors shadow-lg shadow-cyan-500/20 ${isSubmitting ? 'bg-cyan-700 text-slate-400 cursor-not-allowed' : 'bg-cyan-500 text-slate-950 hover:bg-cyan-400'}`}
                >
                    <Save size={20} /> 
                    {isSubmitting ? 'Guardando...' : 'Guardar Consulta'}
                </button>
                {/* Conecta el botón cancelar a la función que vacía el formulario */}
                <button 
                  type="button" 
                  onClick={handleCancel} 
                  className="bg-slate-800 text-slate-400 px-8 py-3 rounded-xl font-bold flex items-center gap-2 hover:bg-slate-700 hover:text-slate-200 transition-colors"
                >
                    <X size={20} /> Cancelar
                </button>
            </div>
          </form>
        </main>
      </div>
      
      {/* Alertas Flotantes (Toasts): Solo se renderizan si showSuccess o errorMsg tienen valor */}
      {showSuccess && (
        <div className="fixed bottom-6 right-6 bg-emerald-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
            <span className="font-semibold">Consulta guardada con éxito</span>
        </div>
      )}
      {errorMsg && (
        <div className="fixed bottom-6 right-6 bg-red-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
            <span className="font-semibold">{errorMsg}</span>
        </div>
      )}
    </div>
  );
};

export default NewConsultation;