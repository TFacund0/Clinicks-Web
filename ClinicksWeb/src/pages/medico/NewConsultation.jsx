// src/pages/medico/NewConsultation.jsx
import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Calendar, User, AlignLeft, AlertCircle, Save, X, Activity, ClipboardList, FileText } from 'lucide-react';
import { useNewConsultation } from '../../hooks/useNewConsultation';
import PageLayout from '../../components/PageLayout';
import Toast from '../../components/Toast';

const NewConsultation = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const dniEntrante = location.state?.dniIngresado || '';
    const idTurnoEntrante = location.state?.idTurno || null;

    const {
        formData,
        errors,
        showSuccess,
        errorMsg,
        isSubmitting,
        handleChange,
        handleSubmit,
        handleCancel
    } = useNewConsultation(dniEntrante, idTurnoEntrante);


    React.useEffect(() => {
        if (!dniEntrante) {
            navigate('/acceso-consulta');
        }
    }, [dniEntrante, navigate]);

    return (
        <PageLayout title="Nueva Consulta">
            <div className="max-w-4xl mx-auto">

                <div className="mb-8">
                    <h1 className="text-4xl font-bold text-white tracking-tight">Nueva Consulta Médica</h1>
                    <p className="text-slate-500 mt-1">Complete el formulario para registrar la atención clínica del paciente.</p>
                </div>


                <form onSubmit={handleSubmit} className="w-full">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">


                        <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                            <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                <ClipboardList size={20} className="text-cyan-400" />
                                Contexto de la Consulta
                            </h3>

                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">DNI Paciente</label>
                                    <input type="text" name="dnipaciente" value={formData.dnipaciente} disabled className="w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-sm text-slate-400 cursor-not-allowed font-mono" />
                                </div>
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Fecha de consulta</label>
                                    <input type="datetime-local" name="fechaconsulta" value={formData.fechaconsulta} disabled className={`w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-sm text-slate-400 cursor-not-allowed`} />
                                    {errors.fechaconsulta && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.fechaconsulta}</p>}
                                </div>
                            </div>

                            <div>
                                <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Motivo de Consulta</label>
                                <textarea name="motivo" value={formData.motivo} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Descripción breve del motivo de la visita..." />
                                {errors.motivo && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.motivo}</p>}
                            </div>
                        </div>


                        <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                            <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                <User size={20} className="text-cyan-400" />
                                Diagnóstico
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


                    <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 mb-8">

                        <h3 className="text-lg font-bold flex items-center gap-2 mb-6 border-b border-slate-800 pb-3">
                            <FileText size={20} className="text-cyan-400" />
                            Detalles Finales de la Consulta
                        </h3>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">

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
                        <button
                            type="submit"
                            disabled={isSubmitting}
                            className={`px-8 py-3 rounded-xl font-bold flex items-center gap-2 transition-colors shadow-lg shadow-cyan-500/20 ${isSubmitting ? 'bg-cyan-700 text-slate-400 cursor-not-allowed' : 'bg-cyan-500 text-slate-950 hover:bg-cyan-400'}`}
                        >
                            <Save size={20} />
                            {isSubmitting ? 'Guardando...' : 'Guardar Consulta'}
                        </button>
                        <button
                            type="button"
                            onClick={handleCancel}
                            className="bg-slate-800 text-slate-400 px-8 py-3 rounded-xl font-bold flex items-center gap-2 hover:bg-slate-700 hover:text-slate-200 transition-colors"
                        >
                            <X size={20} /> Cancelar
                        </button>
                    </div>
                </form>
            </div>

            <Toast
                showSuccess={showSuccess}
                successMsg="Consulta registrada exitosamente"
                errorMsg={errorMsg}
            />
        </PageLayout>
    );
};

export default NewConsultation;