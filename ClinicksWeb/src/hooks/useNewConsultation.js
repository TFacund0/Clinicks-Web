// src/hooks/useNewConsultation.js
import { useState } from 'react';
import consultaService from '../services/consultaService';
import { pacienteService } from '../services/pacienteService'; // IMPORTANTE: Importamos el servicio de pacientes para la búsqueda

// Acá estoy creando un "Custom Hook" (Hook personalizado). 
// Lo hago para sacar toda la lógica pesada (estados, validaciones, llamadas a la base de datos) 
// fuera de mi componente visual. Así la pantalla queda limpia y acá manejo todo el "cerebro" del formulario.
export const useNewConsultation = () => {
    
    // 1. ESTADO DEL FORMULARIO
    // Acá guardo todo lo que el médico va escribiendo en los inputs. 
    // Lo inicializo vacío para que los campos arranquen en blanco.
    const [formData, setFormData] = useState({
        dnipaciente: '',
        motivo: '',
        fechaconsulta: '', 
        diagnostico: '',
        tratamiento: '',
        observaciones: '',
        recomendacion: '',
    });

    // 2. ESTADOS DE LA INTERFAZ DE USUARIO (UI)
    // Estos estados me sirven para darle feedback visual al médico.
    const [errors, setErrors] = useState({}); // Guardo los mensajes de error de validación ("Falta el DNI", etc.)
    const [showSuccess, setShowSuccess] = useState(false); // Un interruptor (true/false) para mostrar el cartelito verde de éxito
    const [errorMsg, setErrorMsg] = useState(null); // Guardo errores que vengan del backend (ej: "Paciente no encontrado")
    const [isSubmitting, setIsSubmitting] = useState(false); // Me avisa si estoy en medio de un guardado para bloquear el botón y evitar que hagan doble clic.

    // --- NUEVOS ESTADOS PARA EL AUTOCOMPLETE ---
    const [sugerencias, setSugerencias] = useState([]); // Guardará la lista de pacientes que coincidan con lo escrito
    const [buscandoSugerencias, setBuscandoSugerencias] = useState(false); // Para saber si la API de búsqueda está trabajando

    // 3. MANEJADOR DE CAMBIOS EN LOS INPUTS
    // Cada vez que el médico teclea algo, esta función se ejecuta.
    const handleChange = async (e) => {
        // Extraigo el 'name' (ej: 'motivo') y el 'value' (lo que escribió) del input que disparó el evento.
        const { name, value } = e.target; 
        
        // Actualizo mi objeto formData copiando lo que ya tenía y pisando solo el campo que cambió.
        setFormData({ ...formData, [name]: value });
        
        // Un toque de experiencia de usuario (UX): si el campo tenía un error en rojo, 
        // apenas el médico empieza a escribir para corregirlo, le borro el mensaje de error.
        if (errors[name]) {
            setErrors({ ...errors, [name]: null });
        }

        // --- LÓGICA DE BÚSQUEDA PREDICTIVA (AUTOCOMPLETE) ---
        // Si el campo que cambió es el 'dnipaciente'...
        if (name === 'dnipaciente') {
            // Solo buscamos si el médico escribió más de 2 caracteres (para no saturar la API)
            if (value.trim().length > 2) {
                setBuscandoSugerencias(true);
                try {
                    // Llamamos a la API de C# pasando lo que el médico escribió hasta ahora
                    const resultados = await pacienteService.buscarPorDni(value);
                    setSugerencias(resultados); // Guardamos los 5 pacientes encontrados
                } catch (err) {
                    console.error("Error buscando sugerencias:", err);
                    setSugerencias([]);
                } finally {
                    setBuscandoSugerencias(false);
                }
            } else {
                // Si el campo está casi vacío, borramos la lista de sugerencias
                setSugerencias([]);
            }
        }
    };

    // --- FUNCIÓN PARA SELECCIONAR PACIENTE ---
    // Esta función se ejecutará cuando el médico haga CLIC en un paciente de la listita flotante.
    const seleccionarPaciente = (paciente) => {
        setFormData({ ...formData, dnipaciente: paciente.dni }); // Completamos el DNI automáticamente
        setSugerencias([]); // Cerramos la lista de sugerencias
    };

    // 4. LÓGICA DE VALIDACIÓN
    // Antes de molestar al backend, reviso localmente que el médico haya llenado lo obligatorio.
    const validarFormulario = () => {
        let erroresTemporales = {};
        
        // Uso .trim() para evitar que me engañen mandando espacios en blanco.
        if (!formData.motivo.trim()) erroresTemporales.motivo = "El motivo de la consulta es obligatorio.";
        if (!formData.diagnostico.trim()) erroresTemporales.diagnostico = "El diagnóstico es obligatorio.";
        if (!formData.dnipaciente.trim()) erroresTemporales.dnipaciente = "El DNI del paciente es obligatorio.";
        if (!formData.tratamiento.trim()) erroresTemporales.tratamiento = "El tratamiento indicado es obligatorio.";
        if (!formData.observaciones.trim()) erroresTemporales.observaciones = "Las observaciones son obligatorias.";
        if (!formData.recomendacion.trim()) erroresTemporales.recomendacion = "Las recomendaciones son obligatorias.";

        // Guardo los errores encontrados en mi estado para que la vista los muestre en rojo.
        setErrors(erroresTemporales);
        
        // Si la cantidad de errores es 0, devuelvo 'true' (el formulario es válido). Si hay errores, devuelvo 'false'.
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
            // Preparo los datos exactamente como los espera mi backend en C#.
            const dataLimpia = {
                ...formData,
                fechaconsulta: formData.fechaconsulta || null,
                idMedico: 1 // TODO: Reemplazar con el ID del contexto/localStorage cuando haya login
            };

            // Le paso los datos a mi servicio para que haga el POST mediante axios.
            const respuesta = await consultaService.crearConsulta(dataLimpia);
            console.log("Consulta creada con éxito:", respuesta);

            // Si todo salió bien, muestro el cartel verde de éxito.
            setShowSuccess(true);
            // Lo oculto automáticamente después de 3 segundos para que no quede ahí para siempre.
            setTimeout(() => setShowSuccess(false), 3000);

            // Vacío el formulario para que el médico pueda cargar otra consulta.
            setFormData({ dnipaciente: '', motivo: '', fechaconsulta: '', diagnostico: '', tratamiento: '', observaciones: '', recomendacion: '' });

        } catch (error) {
            // Si el backend me rechaza (ej: el DNI no existe en la base de datos), atrapo el error acá.
            console.error("Error al crear la consulta:", error);
            // Seteo el mensaje de error que me mandó C# o pongo uno genérico si se cayó el servidor.
            setErrorMsg(error.response?.data?.message || "Error al conectar con la base de datos.");
            // Borro el cartel de error a los 4 segundos.
            setTimeout(() => setErrorMsg(null), 4000);
        } finally {
            // Ya sea que haya funcionado (try) o fallado (catch), apago el estado de carga para liberar el botón.
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
        setSugerencias([]); // Cerramos cualquier lista de búsqueda abierta
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
        sugerencias, 
        buscandoSugerencias, 
        handleChange,
        handleSubmit,
        handleCancel,
        seleccionarPaciente 
    };
};