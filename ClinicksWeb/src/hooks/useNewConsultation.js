// src/hooks/useNuevaConsulta.js
import { useState } from 'react';
import consultaService from '../services/consultaService';

export const useNewConsultation = () => {
    // 1. Estado del formulario
    const [formData, setFormData] = useState({
        dnipaciente: '',
        motivo: '',
        fechaconsulta: '', 
        diagnostico: '',
        tratamiento: '',
        observaciones: '',
        recomendacion: '',
    });

    // 2. Estados de UI (errores y mensajes de éxito)
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    // 3. Manejador de cambios en los inputs
    const handleChange = (e) => {
        const { name, value } = e.target; 
        setFormData({ ...formData, [name]: value });
        
        // Limpiamos el error específico de ese campo cuando el usuario empieza a escribir
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };

    // 4. Lógica de validación
    const validarFormulario = () => {
        let erroresTemporales = {};
        if (!formData.motivo.trim()) erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
        if (!formData.diagnostico.trim()) erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.tratamiento.trim()) erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";
        if (!formData.observaciones.trim()) erroresTemporales.observaciones = "Las observaciones son obligatorias.";
        if (!formData.recomendacion.trim()) erroresTemporales.recomendacion = "Las recomendaciones son obligatorias.";

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    // 5. Envío del formulario a la API
    const handleSubmit = async (e) => {
        e.preventDefault();
        
        if (!validarFormulario()) return;

        setIsSubmitting(true);
        setErrorMsg(null);

        try {
            const dataLimpia = {
                ...formData,
                fechaconsulta: formData.fechaconsulta || null,
                idMedico: 1 // TODO: Reemplazar con el ID del contexto/localStorage cuando haya login
            };

            const respuesta = await consultaService.crearConsulta(dataLimpia);
            console.log("Consulta creada con éxito:", respuesta);

            // Mostrar mensaje de éxito
            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);

            // Resetear el formulario
            setFormData({ dnipaciente: '', motivo: '', fechaconsulta: '', diagnostico: '', tratamiento: '', observaciones: '', recomendacion: '' });

        } catch (error) {
            console.error("Error al crear la consulta:", error);
            setErrorMsg(error.response?.data?.message || "Error al conectar con la base de datos.");
            setTimeout(() => setErrorMsg(null), 4000);
        } finally {
            setIsSubmitting(false);
        }
    };

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
        setErrors({}); // También limpiamos los mensajes de error en rojo si los hubiera
    };
    
    // Exponemos hacia afuera (hacia el componente) solo lo que necesita
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