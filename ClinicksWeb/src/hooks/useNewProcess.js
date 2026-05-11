import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import procesoService from '../services/procesoService';

export const useNewProcess = (dniInicial = '') => {
    const navigate = useNavigate();
    const [tiposDisponibles, setTiposDisponibles] = useState([]);
    const [formData, setFormData] = useState({
        dnipaciente: dniInicial,
        tipoproceso: '',
        descripcion: '',
        fechaproceso: new Date().toISOString().split('T')[0], // Fecha de hoy por defecto
        resultado: '',
    });
    const [errors, setErrors] = useState({});
    const [showSuccess, setShowSuccess] = useState(false);
    const [errorMsg, setErrorMsg] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

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

        // Validación de fecha (no puede ser futura)
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
                fechaproceso: formData.fechaproceso || null
            };
            await procesoService.crearProceso(dataLimpia);
            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);
            setTimeout(() => {
                navigate('/dashboard');
            }, 1500);
        } catch (error) {
            setErrorMsg(error.response?.data?.message || error.response?.data?.mensaje || "Error al conectar con la base de datos.");
            setTimeout(() => setErrorMsg(null), 3000);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            ...formData,
            tipoproceso: '', 
            descripcion: '', 
            fechaproceso: new Date().toISOString().split('T')[0],
            resultado: ''
        });
        setErrors({});
    };

    const cargarTiposDisponibles = async () => {
        try {
            const res = await procesoService.obtenerTiposProceso();
            setTiposDisponibles(res);
        } catch (error) {
            // Manejo de error silencioso o mediante estado de UI si fuera necesario
        }
    };
    useEffect(() => {
        cargarTiposDisponibles();
    }, []);

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
}