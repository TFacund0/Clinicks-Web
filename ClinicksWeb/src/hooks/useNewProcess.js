// src/hooks/useNewProcess.js
import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import procesoService from '../services/procesoService';
import { extraerMensajeError } from '../utils/errorUtils';

export const useNewProcess = (dniInicial = '', idTurnoInicial = null) => {
    const navigate = useNavigate();
    const [tiposDisponibles, setTiposDisponibles] = useState([]);
    const [formData, setFormData] = useState({
        dnipaciente: dniInicial,
        tipoproceso: '',
        descripcion: '',
        fechaproceso: new Date().toISOString().split('T')[0],
        resultado: '',
        idTurno: idTurnoInicial,
    });
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const isMounted = useRef(true);

    // Ref para acumular todos los timers activos y cancelarlos al desmontar.
    // Evita el error "Can't perform a React state update on an unmounted component".
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
            // El select quedará vacío; el usuario verá el error al intentar enviar por validación.
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

        if (formData.fechaproceso && formData.fechaproceso > hoy) {
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
            await procesoService.crearProceso(dataLimpia);
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
            ...formData,
            tipoproceso: '',
            descripcion: '',
            fechaproceso: new Date().toISOString().split('T')[0],
            resultado: '',
            idTurno: null
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
        handleCancel,
        tiposDisponibles,
    };
};