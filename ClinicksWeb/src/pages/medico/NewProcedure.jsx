import React from 'react';
import Sidebar from '../../components/Sidebar';
import { useNewProcedure } from '../../hooks/useNewProcedure';
import { Calendar, User, AlignLeft, AlertCircle, Save, X, Stethoscope, ClipboardList } from 'lucide-react';
import PageLayout from '../../components/PageLayout';
import Toast from '../../components/Toast';
import { useLocation, useNavigate } from 'react-router-dom';

const NewProcedure = () => {
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
        handleCancel,
        tiposDisponibles,
    } = useNewProcedure(dniEntrante, idTurnoEntrante);


    React.useEffect(() => {
        if (!dniEntrante) {
            navigate('/acceso-procedimiento');
        }
    }, [dniEntrante, navigate]);


    return (
        <PageLayout title="Nuevo Procedimiento">
            <div className="max-w-4xl mx-auto">

                <div className="mb-8">
                    <h1 className="text-4xl font-bold text-white tracking-tight">Nuevo Procedimiento</h1>
                    <p className="text-slate-500 mt-1">Complete el formulario para registrar el procedimiento médico.</p>
                </div>

                    <form onSubmit={handleSubmit} className="w-full">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
                            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                    <ClipboardList size={20} className="text-cyan-400" />
                                    Contexto del Procedimiento
                                </h3>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">DNI Paciente</label>
                                        <input type="text" name="dnipaciente" value={formData.dnipaciente} disabled className="w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-sm text-slate-400 cursor-not-allowed font-mono" />
                                    </div>
                                    <div>
                                        <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Fecha del procedimiento</label>
                                        <input type="datetime-local" name="fechaprocedimiento" value={formData.fechaprocedimiento} disabled className={`w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-sm text-slate-400 cursor-not-allowed`} />
                                        {errors.fechaprocedimiento && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.fechaprocedimiento}</p>}
                                    </div>
                                </div>

                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Descripción del Procedimiento</label>
                                    <textarea name="descripcion" value={formData.descripcion} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Descripción breve..." />
                                    {errors.descripcion && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.descripcion}</p>}
                                </div>
                            </div>

                            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                    <Stethoscope size={20} className="text-cyan-400" />
                                    Detalle Clínico
                                </h3>
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Tipo de Procedimiento</label>
                                    <select name="tipoprocedimiento" value={formData.tipoprocedimiento} onChange={handleChange} className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors scheme-dark">
                                        <option value="">Selecciona un tipo</option>
                                        {Array.isArray(tiposDisponibles) && tiposDisponibles.map((tipo) => (
                                            <option key={tipo.id} value={tipo.nombre}>
                                                {tipo.nombre}
                                            </option>))}
                                    </select>
                                    {errors.tipoprocedimiento && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.tipoprocedimiento}</p>}
                                </div>
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Resultado</label>
                                    <textarea name="resultado" value={formData.resultado} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Resultado del procedimiento..." />
                                </div>

                            </div>
                        </div>
                        <div className="flex gap-4">
                            <button
                                type="submit"
                                disabled={isSubmitting}
                                className={`px-8 py-3 rounded-xl font-bold flex items-center gap-2 transition-colors shadow-lg shadow-cyan-500/20 ${isSubmitting ? 'bg-cyan-700 text-slate-400 cursor-not-allowed' : 'bg-cyan-500 text-slate-950 hover:bg-cyan-400'}`}
                            >
                                <Save size={20} />
                                {isSubmitting ? 'Guardando...' : 'Guardar Procedimiento'}
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
                successMsg="Procedimiento registrado exitosamente"
                errorMsg={errorMsg}
            />
        </PageLayout>
    );
};

export default NewProcedure;
