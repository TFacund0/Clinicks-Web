import React from 'react';
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { ClipboardList, User, FileText, Save, X } from 'lucide-react';
import { useNewProcess } from '../../hooks/useNewProcess';
import { useLocation } from 'react-router-dom';

const NewProcess = () => {
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
        handleCancel,
        tiposDisponibles,
    } = useNewProcess(dniEntrante);

    // PROTECCIÓN DE RUTA: Si no hay DNI (acceso directo por URL), volvemos al buscador.
    React.useEffect(() => {
        if (!dniEntrante) {
            navigate('/acceso-procedimiento');
        }
    }, [dniEntrante, navigate]);


    return (
        <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
            <Sidebar />
            <div className="flex-1 flex flex-col">
                <Header paginaActual='Crear Proceso' />
                <main className="p-8 overflow-y-auto">

                    <div className="flex justify-between items-end mb-8">
                        <div>
                            <h1 className="text-4xl font-bold text-white">Buenos días</h1>
                            <p className="text-slate-500 mt-1">Aquí tienes un resumen de tu actividad de hoy.</p>
                        </div>
                    </div>

                    <form onSubmit={handleSubmit} className="w-full">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
                            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                    <ClipboardList size={20} className="text-cyan-400" />
                                    Contexto del Proceso
                                </h3>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">DNI Paciente</label>
                                        <input type="text" name="dnipaciente" value={formData.dnipaciente} disabled className="w-full bg-slate-800 border border-slate-700 rounded-xl px-4 py-3 text-sm text-slate-400 cursor-not-allowed font-mono" />
                                    </div>
                                    <div>
                                        <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Fecha del proceso</label>
                                        <input type="date" name="fechaproceso" value={formData.fechaproceso} onChange={handleChange} className={`w-full bg-slate-950 border ${errors.fechaproceso ? 'border-red-500' : 'border-slate-800'} rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors scheme-dark`} />
                                        {errors.fechaproceso && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.fechaproceso}</p>}
                                    </div>
                                </div>

                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Descripción del Proceso</label>
                                    <textarea name="descripcion" value={formData.descripcion} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Descripción breve..." />
                                    {errors.descripcion && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.descripcion}</p>}
                                </div>
                            </div>

                            <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800 space-y-4">
                                <h3 className="text-lg font-bold flex items-center gap-2 border-b border-slate-800 pb-3">
                                    <ClipboardList size={20} className="text-cyan-400" />
                                    tipo de proceso
                                </h3>
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Tipo de Proceso</label>
                                    <select name="tipoproceso" value={formData.tipoproceso} onChange={handleChange} className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors scheme-dark">
                                        <option value="">Selecciona un tipo</option>
                                        {Array.isArray(tiposDisponibles) && tiposDisponibles.map((tipo) => (
                                            <option key={tipo.id} value={tipo.nombre}>
                                                {tipo.nombre}
                                            </option>))}
                                    </select>
                                    {errors.tipoproceso && <p className="text-red-500 text-[10px] mt-1 font-bold italic">{errors.tipoproceso}</p>}
                                </div>
                                <div>
                                    <label className="block text-xs text-slate-500 uppercase mb-2 font-bold">Resultado</label>
                                    <textarea name="resultado" value={formData.resultado} onChange={handleChange} rows="2" className="w-full bg-slate-950 border border-slate-800 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-cyan-500 transition-colors resize-none" placeholder="Resultado del procedimiento..." />
                                </div>

                            </div>
                        </div>
                        <div className="flex gap-4">
                            {/* El botón se deshabilita (disabled) si ya se está enviando para evitar doble carga */}
                            <button
                                type="submit"
                                disabled={isSubmitting}
                                className={`px-8 py-3 rounded-xl font-bold flex items-center gap-2 transition-colors shadow-lg shadow-cyan-500/20 ${isSubmitting ? 'bg-cyan-700 text-slate-400 cursor-not-allowed' : 'bg-cyan-500 text-slate-950 hover:bg-cyan-400'}`}
                            >
                                <Save size={20} />
                                {isSubmitting ? 'Guardando...' : 'Guardar Proceso'}
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
            {showSuccess && (
                <div className="fixed bottom-6 right-6 bg-emerald-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
                    <span className="font-semibold">Proceso guardado con éxito</span>
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

export default NewProcess;