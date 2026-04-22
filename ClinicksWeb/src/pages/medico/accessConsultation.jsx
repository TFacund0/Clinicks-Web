import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { UserSearch, ArrowRight } from 'lucide-react';
import { useAccessConsultation } from '../../hooks/useAccessConsultation';

const AccessConsultation = () => {
    // Usamos EXACTAMENTE lo que devuelve tu nuevo useAccessConsultation
    const { 
        formData, 
        errors, 
        showSuccess, 
        isSubmitting, 
        handleChange, 
        errorMsg,
        handleSubmit 
    } = useAccessConsultation();

    return (
        <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
            <Sidebar />
            <div className="flex-1 flex flex-col">
                <Header paginaActual='Crear Consulta' />
                
                <main className="p-8 overflow-y-auto">
                    {/* TÍTULO DE SECCIÓN */}
                    <div>
                        <h1 className="text-4xl font-bold text-white tracking-tight">Gestión de Acceso</h1>
                        <p className="text-slate-500 mt-1">Valide la identidad del paciente para iniciar la atención médica.</p>
                    </div>

                    {/* CONTENEDOR DE CENTRADO */}
                    <div className="w-full flex justify-center pt-5">
                        
                        <div className="w-full max-w-sm bg-slate-900 border border-slate-800 rounded-3xl p-8 shadow-2xl relative overflow-hidden">
                            
                            {/* Icono Centralizado */}
                            <div className="flex flex-col items-center mb-2 text-center">
                                <div className="w-24 h-24 bg-cyan-500/10 rounded-full flex items-center justify-center mb-4 border border-cyan-500/20">
                                    <UserSearch className="text-cyan-500" size={48} />
                                </div>
                            </div>

                            {/* Formulario conectado al Hook */}
                            <form onSubmit={handleSubmit} className="space-y-6 relative z-10">
                                <div>
                                    <label className="block text-[10px] text-slate-500 uppercase font-bold mb-2 ml-1 tracking-widest">
                                        DNI del Paciente
                                    </label>
                                    <input
                                        type="text"
                                        name="dnipaciente" // El name debe coincidir con la llave en el Hook
                                        value={formData.dnipaciente} // Usamos formData
                                        onChange={handleChange} // Usamos el handleChange del Hook
                                        placeholder="Ej: 45644949"
                                        className={`w-full bg-slate-950 border rounded-2xl px-5 py-4 text-slate-200 focus:outline-none transition-all text-lg tracking-widest font-mono ${
                                            errors.dnipaciente ? 'border-red-500/50 focus:border-red-500' : 'border-slate-800 focus:border-cyan-500'
                                        }`}
                                    />
                                    {/* Mensaje de error por si mete letras o está vacío */}
                                    {errors.dnipaciente && (
                                        <p className="text-red-500 text-[15px] mt-2 ml-1 font-bold italic">{errors.dnipaciente}</p>
                                    )}
                                </div>
                                
                                <button
                                    type="submit"
                                    disabled={isSubmitting} // Usamos isSubmitting
                                    className={`w-full py-4 rounded-2xl font-bold flex items-center justify-center gap-2 transition-all shadow-lg active:scale-95 ${
                                        isSubmitting 
                                        ? 'bg-cyan-800 text-slate-400 cursor-not-allowed' 
                                        : 'bg-cyan-500 text-slate-950 hover:bg-cyan-400 shadow-cyan-500/20'
                                    }`}
                                >
                                    {isSubmitting ? 'Verificando...' : 'Continuar'}
                                    {!isSubmitting && <ArrowRight size={20} />}
                                </button>
                            </form>
                        </div>
                    </div>
                </main>
            </div>

            {/* Sistema de Notificaciones Unificado (Verde/Rojo) */}
            {showSuccess && (
            <div className="fixed bottom-6 right-6 bg-emerald-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
                <span className="font-semibold">paciente encontrado</span>
            </div>
        )}
      { errorMsg && (
        <div className="fixed bottom-6 right-6 bg-red-500 text-white px-6 py-3 rounded-xl shadow-lg flex items-center gap-2 animate-fade-in">
            <span className="font-semibold">{errorMsg}</span>
        </div>
      )}
        </div>
    );
};

export default AccessConsultation;