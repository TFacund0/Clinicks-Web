// src/hooks/useAccessConsultation.js
import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import pacienteService from '../services/pacienteService';
import { extraerMensajeError } from '../utils/errorUtils';

export const useAccessConsultation = (destino) => {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({ dnipaciente: '' });
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const isMounted = useRef(true);

    // Ref para acumular todos los timers activos y cancelarlos al desmontar.
    // Evita el error "Can't perform a React state update on an unmounted component".
    const timersRef = useRef([]);
    const addTimer = (fn, ms) => {
        const id = setTimeout(fn, ms);
        timersRef.current.push(id);
        return id;
    };

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
            timersRef.current.forEach(clearTimeout);
        };
    }, []);

    // Manejador de cambios con filtro numérico
    const handleChange = (e) => {
        const { name, value } = e.target;
        if (value !== '' && !/^\d+$/.test(value)) return;
        setFormData({ ...formData, [name]: value });
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };

    // Validación local antes de ir al servidor
    const validarFormulario = () => {
        let erroresTemporales = {};
        if (!formData.dnipaciente.trim()) {
            erroresTemporales.dnipaciente = "El DNI es obligatorio.";
        } else if (!/^\d+$/.test(formData.dnipaciente)) {
            erroresTemporales.dnipaciente = "Solo se permiten números.";
        }
        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    const handleSubmit = async (e) => {
        if (e) e.preventDefault();
        if (!validarFormulario()) return;

        setIsSubmitting(true);

        try {
            await pacienteService.validarPacientePorDni(formData.dnipaciente);
            if (isMounted.current) {
                setShowSuccess(true);
                addTimer(() => { if(isMounted.current) setShowSuccess(false); }, 3000);
                addTimer(() => { if(isMounted.current) navigate(destino, { state: { dniIngresado: formData.dnipaciente } }); }, 1500);
            }
        } catch (error) {
            if (isMounted.current) {
                setErrorMsg(extraerMensajeError(error, "DNI no registrado en la base de datos."));
                addTimer(() => { if(isMounted.current) setErrorMsg(null); }, 4000);
            }
        } finally {
            if (isMounted.current) setIsSubmitting(false);
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