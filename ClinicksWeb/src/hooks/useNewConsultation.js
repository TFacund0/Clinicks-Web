// src/hooks/useNewConsultation.js
import { useState } from 'react';
import consultaService from '../services/consultaService';

import { useNavigate } from 'react-router-dom';

export const useNewConsultation = (dniInicial = '') => {
    // 1. Estado del formulario
    const [formData, setFormData] = useState({
        dnipaciente: dniInicial,
        motivo: '',
        fechaconsulta: '',
        diagnostico: '',
        tratamiento: '',
        observaciones: '',
        recomendacion: '',
    });
    const navigate = useNavigate();
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    // 3. MANEJADOR DE CAMBIOS EN LOS INPUTS
    // Cada vez que el médico teclea algo, esta función se ejecuta.
    const handleChange = (e) => {
        // Extraigo el 'name' (ej: 'motivo') y el 'value' (lo que escribió) del input que disparó el evento.
        const { name, value } = e.target;

        // Actualizo mi objeto formData copiando lo que ya tenía y pisando solo el campo que cambió.
        setFormData({ ...formData, [name]: value });

        // Un toque de experiencia de usuario (UX): si el campo tenía un error en rojo, 
        // apenas el médico empieza a escribir para corregirlo, le borro el mensaje de error.
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };

    // 4. LÓGICA DE VALIDACIÓN
    const validarFormulario = () => {
        let erroresTemporales = {};
        const hoy = new Date().toISOString().split('T')[0];

        // Validaciones de obligatoriedad
        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.motivo.trim()) erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
        if (!formData.diagnostico.trim()) erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
        if (!formData.tratamiento.trim()) erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";
        
        // Validación de fecha (no puede ser futura)
        if (formData.fechaconsulta && formData.fechaconsulta > hoy) {
            erroresTemporales.fechaconsulta = "La fecha no puede ser posterior a hoy.";
        }

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    // 5. ENVÍO DEL FORMULARIO A LA API
    // Esta función se dispara cuando el médico hace clic en "Guardar Consulta".
    const handleSubmit = async (e) => {
        e.preventDefault(); // Evito que el navegador recargue la página por defecto.

        // Corto la ejecución acá si la validación falla (si validarFormulario devuelve false).
        if (!validarFormulario()) return;

        // Prendo el estado de carga para cambiar el texto del botón a "Guardando..."
        setIsSubmitting(true);
        setErrorMsg(null);

        try {
            const dataLimpia = {
                ...formData,
                fechaconsulta: formData.fechaconsulta || null
            };

            await consultaService.crearConsulta(dataLimpia);

            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);

            setFormData({ dnipaciente: '', motivo: '', fechaconsulta: '', diagnostico: '', tratamiento: '', observaciones: '', recomendacion: '' });

            setTimeout(() => {
                navigate('/dashboard');
            }, 1500);
        } catch (error) {
            setErrorMsg(error.response?.data?.message || error.response?.data?.mensaje || "Error al conectar con la base de datos.");
            setTimeout(() => setErrorMsg(null), 4000);
        } finally {
            setIsSubmitting(false);
        }
    };

    // 6. CANCELAR Y LIMPIAR
    // Esta función la conecto al botón "Cancelar". 
    // Simplemente devuelve todo a su estado inicial.
    const handleCancel = () => {
        setFormData({
            dnipaciente: '',
            motivo: '',
            fechaconsulta: '',
            diagnostico: '',
            tratamiento: '',
            observaciones: '',
            recomendacion: '',
        });
        setErrors({}); // También limpio los mensajes de error en rojo si los hubiera.
    };

    // 7. LO QUE DEVUELVE MI HOOK
    // Exponemos hacia afuera (hacia la vista visual NewConsultation.jsx) 
    // solo los datos y funciones que necesita para funcionar. Lo demás queda privado acá adentro.
    return {
        formData,
        errors,
        showSuccess,
        errorMsg,
        isSubmitting,
        handleChange,
        handleSubmit,
        handleCancel
    };
};