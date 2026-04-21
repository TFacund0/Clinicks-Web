/**
 * COMPONENTE: Patients (Vista de Listado de Pacientes)
 * PROPÓSITO: Mostrar a los pacientes atendidos por el médico logueado,
 * permitiendo búsquedas rápidas y acceso a sus historiales.
 */

// 1. IMPORTACIÓN DE COMPONENTES Y HERRAMIENTAS
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { useNavigate } from 'react-router-dom'; // Hook para la navegación entre rutas
import { usePatients } from '../../hooks/usePatients'; // Hook personalizado que centraliza la lógica de datos
import { Search, Filter, ExternalLink } from 'lucide-react'; // Iconos de la interfaz

const Patients = () => {
  // Inicializamos la función de navegación para poder ir al historial clínico
  const navigate = useNavigate();

  // 2. GESTIÓN DE SESIÓN DEL MÉDICO (Simulada para desarrollo)
  // Obtenemos el ID del médico guardado en el navegador (ej: después de un login)
  const idGuardado = localStorage.getItem('medicoId');
  // Convertimos a entero. Si no existe, usamos el ID 1 por defecto para no romper la app.
  const MEDICO_ID_ACTUAL = parseInt(idGuardado) || 1; 

  /**
   * 3. CONSUMO DEL CUSTOM HOOK 'usePatients'
   * Delegamos toda la lógica de:
   * - Llamada a la API (useEffect)
   * - Estado de carga y errores
   * - Filtrado de búsqueda en tiempo real
   * al hook personalizado para mantener este componente visual limpio.
   */
  const { 
    pacientesFiltrados, // Lista ya filtrada según el buscador
    cargando,           // Estado booleano de la petición
    error,              // Mensaje de error si falla la API
    searchTerm,         // Valor actual del input de búsqueda
    setSearchTerm       // Función para actualizar la búsqueda
  } = usePatients(MEDICO_ID_ACTUAL);

  // 4. RENDERIZADO DE LA INTERFAZ
  return (
    <div className="flex h-screen w-full bg-slate-950 text-slate-200 overflow-hidden font-sans">
      {/* Barra lateral de navegación */}
      <Sidebar />

      <div className="flex-1 flex flex-col min-w-0">
        {/* Barra superior con info del médico y reloj */}
        <Header paginaActual='Listado de Pacientes'/>
        
        <main className="flex-1 p-8 overflow-y-auto w-full">
            <div className="max-w-7xl mx-auto w-full">
            
            {/* Encabezado de la sección */}
            <div className="flex justify-between items-center mb-8">
              <div>
                <h1 className="text-3xl font-bold">Listado de Pacientes</h1>
                <p className="text-slate-500 text-sm">Gestiona y consulta el historial de tus pacientes.</p>
              </div>
            </div>

            {/* BARRA DE HERRAMIENTAS (Buscador) */}
            <div className="bg-slate-900 p-4 rounded-2xl border border-slate-800 mb-6 flex flex-wrap gap-4 items-center justify-between">
              <div className="flex items-center gap-3 bg-slate-950 px-4 py-2 rounded-xl border border-slate-800 w-full md:w-96">
                <Search size={18} className="text-slate-500" />
                <input 
                  type="text" 
                  placeholder="Buscar por Nombre o DNI..." 
                  className="bg-transparent border-none outline-none text-sm text-slate-300 w-full"
                  value={searchTerm} // Conexión bidireccional con el estado del Hook
                  onChange={(e) => setSearchTerm(e.target.value)} // Dispara el filtrado en cada tecla
                />
              </div>
            </div>

            {/* TABLA DE PACIENTES */}
            <div className="bg-slate-900 rounded-2xl border border-slate-800 overflow-hidden">
              <table className="w-full text-left">
                <thead className="bg-slate-950 text-slate-500 text-xs uppercase tracking-widest font-bold">
                  <tr>
                    <th className="p-5">ID</th>
                    <th className="p-5">Paciente</th>
                    <th className="p-5 text-center">Edad</th>
                    <th className="p-5">Última Consulta</th>
                    <th className="p-5">Estado</th>
                    <th className="p-5 text-right">Acciones</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-800">
                  
                  {/* MANEJO DE ESTADOS LÓGICOS EN LA TABLA */}
                  {cargando ? (
                    // Caso 1: Los datos todavía están viajando desde el servidor
                    <tr><td colSpan="6" className="p-10 text-center text-slate-500 italic">Conectando con el servidor hospitalario...</td></tr>
                  ) : error ? (
                    // Caso 2: Hubo un problema con la API o la base de datos
                    <tr><td colSpan="6" className="p-10 text-center text-red-400">{error}</td></tr>
                  ) : (
                    // Caso 3: Datos listos. Verificamos si hay pacientes tras el filtro
                    pacientesFiltrados.length > 0 ? (
                      pacientesFiltrados.map((paciente) => (
                        <tr key={paciente.id} className="hover:bg-slate-800/30 transition-colors group">
                          {/* ID formateado para estilo médico */}
                          <td className="p-5 text-cyan-500 font-mono text-sm">#000{paciente.id}</td>
                          
                          {/* Info Principal: Nombre y DNI */}
                          <td className="p-5">
                            <div className="flex items-center gap-3">
                              <div className="w-9 h-9 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-xs font-bold text-slate-400">
                                {paciente.nombreCompleto ? paciente.nombreCompleto.charAt(0) : '?'}
                              </div>
                              <div>
                                <p className="font-bold text-slate-200 text-sm leading-none">{paciente.nombreCompleto}</p>
                                <p className="text-[10px] text-slate-500 mt-1 uppercase">DNI: {paciente.dni}</p>
                              </div>
                            </div>
                          </td>

                          <td className="p-5 text-center text-sm text-slate-400">{paciente.edad}</td>
                          <td className="p-5 text-sm text-slate-400">{paciente.fechaUltimaConsulta}</td>
                          
                          {/* Badge dinámico de estado (Activo/Inactivo) */}
                          <td className="p-5">
                            <span className={`px-2 py-1 rounded text-[10px] font-bold uppercase ${
                              paciente.estaActivo 
                              ? "bg-green-500/10 text-green-500" 
                              : "bg-red-500/10 text-red-500"
                            }`}>
                              {paciente.estaActivo ? "Activo" : "Inactivo"}
                            </span>
                          </td>

                          {/* ACCIONES: Salto al Historial Clínico */}
                          <td className="p-5 text-right">
                            <button 
                              // Al hacer clic, enviamos al médico a la ruta dinámica con el ID del paciente
                              onClick={() => navigate(`/pacientes/${paciente.id}/historial`)}
                              className="text-cyan-400 text-xs font-bold flex items-center gap-1 ml-auto hover:text-cyan-300 transition-colors"
                            >
                              Ver Historial <ExternalLink size={14} />
                            </button>
                          </td>
                        </tr>
                      ))
                    ) : (
                      // Caso 4: El médico buscó un nombre/DNI que no existe en su lista
                      <tr><td colSpan="6" className="p-10 text-center text-slate-600">No se encontraron pacientes que coincidan con "{searchTerm}"</td></tr>
                    )
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
};

export default Patients;