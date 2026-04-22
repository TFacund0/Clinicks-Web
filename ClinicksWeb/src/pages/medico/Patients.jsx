// src/pages/medico/Patients.jsx

// 1. IMPORTACIÓN DE COMPONENTES Y HERRAMIENTAS
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { useNavigate } from 'react-router-dom'; 
import { usePatients } from '../../hooks/usePatients'; 
import { Search, Filter, ExternalLink } from 'lucide-react'; 

// Vista que muestra la tabla con todos los pacientes del médico, incluyendo un buscador en tiempo real.
const Patients = () => {
  // Inicializa la herramienta para cambiar de página (ej. ir al historial).
  const navigate = useNavigate();

  // 2. GESTIÓN DE SESIÓN DEL MÉDICO
  // Recupera el ID del médico desde el almacenamiento local o usa 1 por defecto.
  const idGuardado = localStorage.getItem('medicoId');
  const MEDICO_ID_ACTUAL = parseInt(idGuardado) || 1; 

  // 3. CONSUMO DEL CUSTOM HOOK 'usePatients'
  // Extrae los datos procesados, estados de carga y controles de búsqueda desde nuestro "cerebro".
  const { 
    pacientesFiltrados, 
    cargando,           
    error,              
    searchTerm,         
    setSearchTerm       
  } = usePatients(MEDICO_ID_ACTUAL);

  // 4. RENDERIZADO DE LA INTERFAZ
  return (
    <div className="flex h-screen w-full bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />

      <div className="flex-1 flex flex-col min-w-0">
        <Header paginaActual='Listado de Pacientes'/>
        
        <main className="flex-1 p-8 overflow-y-auto w-full">
            <div className="max-w-7xl mx-auto w-full">
            
<<<<<<< HEAD
            {/* Encabezado principal de la pantalla */}
            <div className="flex justify-between items-center mb-8">
              <div>
                <h1 className="text-3xl font-bold">Listado de Pacientes</h1>
                <p className="text-slate-500 text-sm">Gestiona y consulta el historial de tus pacientes.</p>
              </div>
            </div>

            {/* BARRA DE BÚSQUEDA */}
=======
            {/* Encabezado de la sección */}
            <div className="flex justify-between items-end mb-8">
            <div>
              <h1 className="text-4xl font-bold text-white">Listado de Pacientes</h1>
              <p className="text-slate-500 mt-1">Gestiona y consulta el historial de tus pacientes.</p>
            </div> 
          </div>
            {/* BARRA DE HERRAMIENTAS (Buscador) */}
>>>>>>> feature-ingreso-consulta
            <div className="bg-slate-900 p-4 rounded-2xl border border-slate-800 mb-6 flex flex-wrap gap-4 items-center justify-between">
              <div className="flex items-center gap-3 bg-slate-950 px-4 py-2 rounded-xl border border-slate-800 w-full md:w-96">
                <Search size={18} className="text-slate-500" />
                <input 
                  type="text" 
                  placeholder="Buscar por Nombre o DNI..." 
                  className="bg-transparent border-none outline-none text-sm text-slate-300 w-full"
                  value={searchTerm} // Conecta lo que se ve en el input con el estado del Hook
                  onChange={(e) => setSearchTerm(e.target.value)} // Actualiza la búsqueda con cada tecla presionada
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
                  {/* Lógica condicional: Decide qué mostrar según el estado de los datos */}
                  {cargando ? (
                    // ESTADO 1: Esperando datos de la API
                    <tr><td colSpan="6" className="p-10 text-center text-slate-500 italic">Conectando con el servidor hospitalario...</td></tr>
                  ) : error ? (
                    // ESTADO 2: Ocurrió un error en la conexión
                    <tr><td colSpan="6" className="p-10 text-center text-red-400">{error}</td></tr>
                  ) : (
                    // ESTADO 3: Hay datos para mostrar
                    pacientesFiltrados.length > 0 ? (
                      pacientesFiltrados.map((paciente) => (
                        <tr key={paciente.id} className="hover:bg-slate-800/30 transition-colors group">
                          <td className="p-5 text-cyan-500 font-mono text-sm">#000{paciente.id}</td>
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
                          <td className="p-5">
                            <span className={`px-2 py-1 rounded text-[10px] font-bold uppercase ${
                              paciente.estaActivo 
                              ? "bg-green-500/10 text-green-500" 
                              : "bg-red-500/10 text-red-500"
                            }`}>
                              {paciente.estaActivo ? "Activo" : "Inactivo"}
                            </span>
                          </td>
                          <td className="p-5 text-right">
                            {/* Botón que navega a la URL específica del historial de este paciente */}
                            <button 
                              onClick={() => navigate(`/pacientes/${paciente.id}/historial`)}
                              className="text-cyan-400 text-xs font-bold flex items-center gap-1 ml-auto hover:text-cyan-300 transition-colors"
                            >
                              Ver Historial <ExternalLink size={14} />
                            </button>
                          </td>
                        </tr>
                      ))
                    ) : (
                      // ESTADO 4: La búsqueda no arrojó resultados o la base está vacía
                      <tr>
                        <td colSpan="6" className="p-10 text-center text-slate-500">
                          {searchTerm === "" 
                            ? "No hay pacientes atendidos registrados" 
                            : `No se encontraron pacientes que coincidan con "${searchTerm}"` 
                          }
                        </td>
                      </tr>
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