// src/hooks/useNewConsultation.js
import { useState, useEffect, useRef } from 'react';
import consultaService from '../services/consultaService';
import { useNavigate } from 'react-router-dom';
import { extraerMensajeError } from '../utils/errorUtils';

/**
 * Hook para gestionar el formulario de una nueva consulta médica.
 */
export const useNewConsultation = (dniInicial = '', idTurnoInicial = null) => {
    const obtenerFechaHoraLocal = () => {
        const tzoffset = (new Date()).getTimezoneOffset() * 60000;
        return new Date(Date.now() - tzoffset).toISOString().slice(0, 16);
    };

    const [formData, setFormData] = useState({
        dnipaciente: dniInicial,
        motivo: '',
        fechaconsulta: obtenerFechaHoraLocal(),
        diagnostico: '',
        tratamiento: '',
        observaciones: '',
        recomendacion: '',
        idTurno: idTurnoInicial,
    });
    const navigate = useNavigate();
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const isMounted = useRef(true);

    const timersRef = useRef([]);
    const addTimer = (fn, ms) => {
        const id = setTimeout(fn, ms);
        timersRef.current.push(id);
        return id;
    };

    useEffect(() => {
        isMounted.current = true;
        const currentTimers = timersRef.current;
        return () => {
            isMounted.current = false;
            currentTimers.forEach(clearTimeout);
        };
    }, []);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }
    };

    const validarFormulario = () => {
        let erroresTemporales = {};
        const hoy = new Date().toISOString().split('T')[0];

        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.motivo.trim()) erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
        if (!formData.diagnostico.trim()) erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
        if (!formData.tratamiento.trim()) erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";

        if (formData.fechaconsulta && formData.fechaconsulta.split('T')[0] > hoy) {
            erroresTemporales.fechaconsulta = "La fecha no puede ser posterior a hoy.";
        }

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validarFormulario()) return;

        setIsSubmitting(true);
        setErrorMsg(null);

        try {
            const dataLimpia = {
                ...formData,
                fechaconsulta: formData.fechaconsulta || null,
                idTurno: formData.idTurno || null
            };

            await consultaService.registrarConsulta(dataLimpia);

            if (isMounted.current) {
                setShowSuccess(true);
                addTimer(() => { if(isMounted.current) setShowSuccess(false); }, 3000);
                setFormData({ dnipaciente: '', motivo: '', fechaconsulta: obtenerFechaHoraLocal(), diagnostico: '', tratamiento: '', observaciones: '', recomendacion: '', idTurno: null });
                addTimer(() => { if(isMounted.current) navigate('/dashboard'); }, 1500);
            }
        } catch (error) {
            if (isMounted.current) {
                setErrorMsg(extraerMensajeError(error, "Error al conectar con la base de datos."));
                addTimer(() => { if(isMounted.current) setErrorMsg(null); }, 4000);
            }
        } finally {
            if (isMounted.current) setIsSubmitting(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            dnipaciente: '',
            motivo: '',
            fechaconsulta: obtenerFechaHoraLocal(),
            diagnostico: '',
            tratamiento: '',
            observaciones: '',
            recomendacion: '',
            idTurno: null,
        });
        setErrors({});
        navigate('/acceso-consulta');
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