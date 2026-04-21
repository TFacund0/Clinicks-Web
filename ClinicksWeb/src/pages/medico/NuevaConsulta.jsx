<script src="http://localhost:8097"></script>
import React, { useState } from 'react';
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import ConsultaService from '/src/pages/medico/ConsultaService';
import { ClipboardList, User, Calendar, FileText, Save, X } from 'lucide-react';

const NuevaConsulta = () => {

const [formData, setFormData] = useState({
    dnipaciente: '',
    motivo: '',
    fechaconsulta: '', 
    diagnostico: '',
    tratamiento: '',
    observaciones: '',
    recomendacion: '',
});

const [errors, setErrors] = useState({});

const [showSuccess, setShowSuccess] = useState(false);
const [errorMsg, setErrorMsg] = useState(null);

const handleChange = (e) => {
    const { name, value } = e.target; 
    setFormData({ ...formData, [name]: value });
    
    if (errors[name]) {
        setErrors({
            ...errors,
            [name]: null
        });
    }
};

const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validarFormulario()) {
        console.log("Formulario con errores, no se envía.");
        return;
    }
    try {
        const dataLimpia = {
            ...formData,
            fechaconsulta: formData.fechaconsulta || null
        };
        const respuesta = await ConsultaService.crearConsulta(dataLimpia);
        console.log("Consulta creada con éxito:", respuesta);

        setShowSuccess(true);
        setErrorMsg(null);

        setTimeout(() => setShowSuccess(false), 3000);

    } catch (error) {
        console.error("Error al crear la consulta:", error.response?.data || error.message);

        setErrorMsg(error.response?.data?.message || "este no es un dni válido o el paciente no existe");

    setTimeout(() => setErrorMsg(null), 4000);
    }
};
const validarFormulario = () => {
    let erroresTemporales = {};

    if (!formData.motivo.trim()) {
        erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
    }

    if (!formData.diagnostico.trim()) {
        erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
    }
    
    if (!formData.dnipaciente.trim()) {
        erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
    }

    if (!formData.tratamiento.trim()) {
        erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";
    }

    if (!formData.observaciones.trim()) {
        erroresTemporales.observaciones = "Las observaciones son obligatorias.";
    }

    if (!formData.recomendacion.trim()) {
        erroresTemporales.recomendacion = "Las recomendaciones son obligatorias.";
    }


    setErrors(erroresTemporales);
    console.log("Errores encontrados:", erroresTemporales)
    return Object.keys(erroresTemporales).length === 0;
};

  return (
    <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />

    <div className="flex-1 flex flex-col">
        <Header />

        <main className="p-8 overflow-y-auto">
        <div className="mb-8">
            <h1 className="text-4xl font-bold flex items-center gap-3">
                <ClipboardList className="text-cyan-500" size={40} />
                Registrar Nueva Consulta
            </h1>
            <p className="text-slate-500 mt-2">Completa los campos médicos para generar la ficha de atención.</p>
        </div>

          {/* FORMULARIO PRINCIPAL */}
        <form onSubmit={handleSubmit} className="max-w-4xl">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
            
              {/* TARJETA: Información de Selección */}
                <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                    <User size={20} className="text-cyan-400" />
                    Contexto de la Consulta
                </h3>
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">fecha de la consulta</label>
                    <input 
                        type="date" 
                        name="fechaconsulta"
                        value={formData.fechaconsulta}
                        onChange={handleChange}
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors scheme-dark"
                    />
                </div>
                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Paciente (DNI)</label>
                    <input 
                        type="text" 
                        name="dnipaciente"
                        value={formData.dnipaciente}
                        onChange={handleChange}
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors"
                        placeholder="Ej: 10230 o Emily Davis..."
                    />
                    {errors.dnipaciente && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.dnipaciente}</p>}
                </div>

                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Motivo de Consulta</label>
                    <textarea 
                        name="motivo"
                        value={formData.motivo}
                        onChange={handleChange}
                        rows="2"
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none"
                        placeholder="Descripción breve del síntoma..."
                    />
                    {errors.motivo && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.motivo}</p>}      
                </div>
            </div>

              {/* TARJETA: Detalles Médicos */}
                <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                    <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                        <User size={20} className="text-cyan-400" />
                        Diagnóstico y Tratamiento
                    </h3>

                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Diagnóstico Presuntivo</label>
                    <input 
                        type="text" 
                        name="diagnostico"
                        value={formData.diagnostico}
                        onChange={handleChange}
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors"
                        placeholder="Resultado de la evaluación..."
                    />
                    {errors.diagnostico && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.diagnostico}</p>}
                </div>

                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Recomendaciones</label>
                    <input 
                        type="text" 
                        name="recomendacion"
                        value={formData.recomendacion}
                        onChange={handleChange}
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors"
                        placeholder="Resultado de la evaluación..."
                    />
                    {errors.recomendacion && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.recomendacion}</p>}
                </div>

                <div>
                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Tratamiento Indicado</label>
                    <input 
                        type="text" 
                        name="tratamiento"
                        value={formData.tratamiento}
                        onChange={handleChange}
                        className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors"
                        placeholder="Medicamentos, reposo, etc."
                    />
                    {errors.tratamiento && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.tratamiento}</p>}
                </div>
            </div>
        </div>

            {/* SECCIÓN ANCHA: Observaciones */}
            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 mb-8">
                <h3 className="text-lg font-bold flex items-center gap-2 mb-4">
                    <FileText size={20} className="text-cyan-400" />
                        Observaciones Adicionales
                </h3>
                <textarea 
                    name="observaciones"
                    value={formData.observaciones}
                    onChange={handleChange}
                    rows="4"
                    className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none"
                    placeholder="Notas internas del médico..."
                />
                {errors.observaciones && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.observaciones}</p>}
            </div>
            
            {/* BOTONES DE ACCIÓN */}
            <div className="flex gap-4">
                <button 
                    type="submit"
                    className="bg-cyan-500 text-slate-950 px-8 py-3 rounded-xl font-bold flex items-center gap-2 hover:bg-cyan-400 transition-colors shadow-lg shadow-cyan-500/20"
                >
                <Save size={20} />
                    Guardar Consulta
                </button>
                <button 
                    type="button"
                    className="bg-slate-800 text-slate-400 px-8 py-3 rounded-xl font-bold flex items-center gap-2 hover:bg-slate-700 hover:text-slate-200 transition-colors"
                >
                <X size={20} />
                Cancelar
              </button>
            </div>
          </form>
        </main>
      </div>
        {showSuccess && (
            <div className="fixed bottom-6 right-6 bg-emerald-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
                <span className="font-semibold">Consulta guardada con éxito</span>
            </div>
        )}
        {errorMsg && (
            <div className="fixed bottom-6 right-6 bg-red-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
                <span className="font-semibold"> {errorMsg}</span>
            </div>
        )}
    </div>
  );
};

export default NuevaConsulta;