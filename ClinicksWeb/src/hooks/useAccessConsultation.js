import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import pacienteService from '../services/pacienteService';

export const useAccessConsultation = (destino) => {
    const navigate = useNavigate();

    // 1. Un solo estado para el formulario
    const [formData, setFormData] = useState({ dnipaciente: '' });
    const [errors, setErrors] = useState({});
    
    const [showSuccess, setShowSuccess] = useState(false);
    // 2. Un solo estado de carga y notificaciones
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    // Manejador de cambios en el input con filtro numérico
    const handleChange = (e) => {
        const { name, value } = e.target;
        
        // Filtro: Solo permitimos que el estado se actualice si el valor son números.
        // Esto evita que el médico pueda siquiera escribir letras.
        if (value !== '' && !/^\d+$/.test(value)) return;

        setFormData({ ...formData, [name]: value });
        
        // Limpiamos error si el usuario escribe
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };
    
    // Validación local antes de ir al servidor
    const validarFormulario = () => {
        let erroresTemporales = {};
        // Validamos que no esté vacío y que sean solo números (típico de DNI)
        if (!formData.dnipaciente.trim()) {
            erroresTemporales.dnipaciente = "El DNI es obligatorio.";
        } else if (!/^\d+$/.test(formData.dnipaciente)) {
            erroresTemporales.dnipaciente = "Solo se permiten números.";
        }

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    // El único "Submit" que necesitamos
    const handleSubmit = async (e) => {
        if (e) e.preventDefault();
        
        if (!validarFormulario()) return;

        setIsSubmitting(true);

        try {
            await pacienteService.validarPacientePorDni(formData.dnipaciente);
            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);

            setTimeout(() => {
                navigate(destino, { state: { dniIngresado: formData.dnipaciente } });
            }, 1500);
            
        } catch (error) {
            setErrorMsg(error.response?.data?.message || error.response?.data?.mensaje || "dni no registrado en la base de datos.");
            setTimeout(() => setErrorMsg(null), 4000);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleCancel = () => {
        setFormData({ dnipaciente: '' });
        setErrors({});
    };

    return {
        formData,
        errors,
        showSuccess,
        isSubmitting,
        handleChange,
        handleSubmit,
        errorMsg,
        handleCancel
    };
};