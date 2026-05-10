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
        fechaproceso: '',
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

        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.tipoproceso.trim()) erroresTemporales.tipoproceso = "El tipo de proceso es obligatorio.";
        if (!formData.descripcion.trim()) erroresTemporales.descripcion = "La descripción del proceso es obligatoria.";


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
            const res = await procesoService.crearProceso(dataLimpia);
            console.log("Proceso creado:", res);
            setShowSuccess(true);
            setTimeout(() => setShowSuccess(false), 3000);
            setTimeout(() => {
                navigate('/dashboard');
            }, 1500);
        } catch (error) {

            console.error("Error al crear proceso:", error);

            setErrorMsg(error.response?.data?.message || error.response?.data?.mensaje || "Error al conectar con la base de datos.");

            setTimeout(() => setErrorMsg(null), 3000);
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            dnipaciente: '', 
            tipoproceso: '', 
            descripcion: '', 
            fechaproceso: '',
            resultado: ''
        });
        setErrors({});
    };

    const cargarTiposDisponibles = async () => {
        try {
            const res = await procesoService.obtenerTiposProceso();
            setTiposDisponibles(res);
        } catch (error) {
            console.error("Error al cargar tipos de proceso:", error);
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