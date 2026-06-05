// src/hooks/useNewProcess.js
import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import procesoService from '../services/procesoService';
import { extraerMensajeError } from '../utils/errorUtils';

/**
 * Hook para gestionar el formulario de un nuevo procedimiento médico.
 */
export const useNewProcess = (dniInicial = '', idTurnoInicial = null) => {
    const navigate = useNavigate();
    const [tiposDisponibles, setTiposDisponibles] = useState([]);
    const obtenerFechaHoraLocal = () => {
        const tzoffset = (new Date()).getTimezoneOffset() * 60000;
        return new Date(Date.now() - tzoffset).toISOString().slice(0, 16);
    };

    const [formData, setFormData] = useState({
        dnipaciente: dniInicial,
        tipoproceso: '',
        descripcion: '',
        fechaproceso: obtenerFechaHoraLocal(),
        resultado: '',
        idTurno: idTurnoInicial,
    });
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

    const cargarTiposDisponibles = async () => {
        try {
            const res = await procesoService.obtenerTiposProceso();
            if (isMounted.current) setTiposDisponibles(res);
        } catch {
            console.error("Error al cargar tipos de proceso");
        }
    };

    useEffect(() => {
        isMounted.current = true;
        Promise.resolve().then(() => {
            cargarTiposDisponibles();
        });
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
        if (!formData.tipoproceso.trim()) erroresTemporales.tipoproceso = "El tipo de proceso es obligatorio.";
        if (!formData.descripcion.trim()) erroresTemporales.descripcion = "La descripción del proceso es obligatoria.";

        if (formData.fechaproceso && formData.fechaproceso.split('T')[0] > hoy) {
            erroresTemporales.fechaproceso = "La fecha no puede ser posterior a hoy.";
        }

        setErrors(erroresTemporales);
        return Object.keys(erroresTemporales).length === 0;
    };

    const handleSubmit = async (e) => {
        if (e) e.preventDefault();
        if (!validarFormulario()) return;

        setIsSubmitting(true);
        setErrorMsg(null);
        try {
            const dataLimpia = {
                ...formData,
                fechaproceso: formData.fechaproceso || null,
                idTurno: formData.idTurno || null
            };
            await procesoService.registrarProcedimiento(dataLimpia);
            if (isMounted.current) {
                setShowSuccess(true);
                addTimer(() => { if (isMounted.current) setShowSuccess(false); }, 3000);
                addTimer(() => { if (isMounted.current) navigate('/dashboard'); }, 1500);
            }
        } catch (error) {
            if (isMounted.current) {
                setErrorMsg(extraerMensajeError(error, "Error al conectar con la base de datos."));
                addTimer(() => { if (isMounted.current) setErrorMsg(null); }, 3000);
            }
        } finally {
            if (isMounted.current) setIsSubmitting(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            dnipaciente: '',
            tipoproceso: '',
            descripcion: '',
            fechaproceso: obtenerFechaHoraLocal(),
            resultado: '',
            idTurno: null
        });
        setErrors({});
        navigate('/acceso-procedimiento');
    };

    return {
        formData,
        errors,
        showSuccess,
        errorMsg,
        isSubmitting,
        handleChange,
        handleSubmit,
        handleCancel,
        tiposDisponibles,
    };
};