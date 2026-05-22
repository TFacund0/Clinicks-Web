// src/components/Toast.jsx
import { CheckCircle2, AlertCircle } from 'lucide-react';

/**
 * Componente Toast reutilizable para mostrar notificaciones flotantes de éxito o error.
 * DRY-1: Elimina la duplicación de código de alertas en todos los formularios.
 * 
 * @param {boolean} showSuccess - Determina si se muestra el mensaje de éxito.
 * @param {string} successMsg - El mensaje de éxito a mostrar.
 * @param {string} errorMsg - El mensaje de error a mostrar. Si es null/falso, no se muestra el toast de error.
 */
const Toast = ({ showSuccess, successMsg, errorMsg }) => {
    return (
        <div className="fixed bottom-6 right-6 z-50 flex flex-col gap-3 pointer-events-none">
            {showSuccess && (
                <div className="bg-green-500 text-slate-950 px-6 py-4 rounded-2xl font-bold shadow-2xl flex items-center gap-3 animate-bounce-short">
                    <CheckCircle2 size={24} /> {successMsg}
                </div>
            )}
            
            {errorMsg && (
                <div className="bg-red-500 text-slate-950 px-6 py-4 rounded-2xl font-bold shadow-2xl flex items-center gap-3 animate-pulse">
                    <AlertCircle size={24} /> {errorMsg}
                </div>
            )}
        </div>
    );
};

export default Toast;
