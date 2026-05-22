// src/hooks/useNewConsultation.js
import { useState, useEffect, useRef } from 'react';
import consultaService from '../services/consultaService';
import { useNavigate } from 'react-router-dom';
import { extraerMensajeError } from '../utils/errorUtils';

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

    // Ref para acumular todos los timers activos y cancelarlos al desmontar.
    // Evita el error "Can't perform a React state update on an unmounted component".
    const timersRef = useRef([]);
    const addTimer = (fn, ms) => {
        const id = setTimeout(fn, ms);
        timersRef.current.push(id);
        return id;
    };

    useEffect(() => {
        // Cleanup: cancela cualquier timer pendiente al desmontar el componente.
        return () => timersRef.current.forEach(clearTimeout);
    }, []);

    // 3. MANEJADOR DE CAMBIOS EN LOS INPUTS
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };

    // 4. LÓGICA DE VALIDACIÓN
    const validarFormulario = () => {
        let erroresTemporales = {};
        const hoy = new Date().toISOString().split('T')[0];

        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.motivo.trim()) erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
        if (!formData.diagnostico.trim()) erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
        if (!formData.tratamiento.trim()) erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";

        if (formData.fechaconsulta && formData.fechaconsulta > hoy) {
            erroresTemporales.fechaconsulta = "La fecha no puede ser posterior a hoy.";
        }

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    // 5. ENVÍO DEL FORMULARIO A LA API
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validarFormulario()) return;

        setIsSubmitting(true);
        setErrorMsg(null);

        try {
            const dataLimpia = {
                ...formData,
                fechaconsulta: formData.fechaconsulta || null
            };

            await consultaService.crearConsulta(dataLimpia);

            setShowSuccess(true);
            // Timer registrado: se cancela si el componente se desmonta antes de los 3s.
            addTimer(() => setShowSuccess(false), 3000);

            setFormData({ dnipaciente: '', motivo: '', fechaconsulta: '', diagnostico: '', tratamiento: '', observaciones: '', recomendacion: '' });

            addTimer(() => navigate('/dashboard'), 1500);

        } catch (error) {
            setErrorMsg(extraerMensajeError(error, "Error al conectar con la base de datos."));
            addTimer(() => setErrorMsg(null), 4000);
        } finally {
            setIsSubmitting(false);
        }
    };

    // 6. CANCELAR Y LIMPIAR
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
        setErrors({});
    };

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