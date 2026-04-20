import { useState, useEffect } from 'react'; 
// 1. ELIMINAMOS AXIOS DE ACÁ E IMPORTAMOS EL SERVICIO
import { pacienteService } from '../../services/pacienteService'; 
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
import { Search, Filter, ExternalLink } from 'lucide-react';

const Patients = () => {
  const [pacientes, setPacientes] = useState([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let montado = true;

    const fetchData = async () => {
      try {
        // 2. USAMOS EL SERVICIO EN LUGAR DE AXIOS DIRECTO
        const datos = await pacienteService.obtenerTodos();
        
        if (montado) {
          setPacientes(datos);
          setCargando(false);
        }
      } catch (err) {
        if (montado) {
          console.error("Error en la capa de servicio:", err);
          setError("No se pudo cargar la lista de pacientes.");
          setCargando(false);
        }
      }
    };

    fetchData();

    return () => {
      montado = false;
    };
  }, []);

  return (
    <div className="flex h-screen w-full bg-slate-950 text-slate-200 overflow-hidden font-sans">
      <Sidebar />
      <div className="flex-1 flex flex-col min-w-0">
        <Header />
        
        <main className="flex-1 p-8 overflow-y-auto w-full">
            <div className="max-w-7xl mx-auto w-full">
            
            <div className="flex justify-between items-center mb-8">
              <div>
                <h1 className="text-3xl font-bold">Listado de Pacientes</h1>
                <p className="text-slate-500 text-sm">Gestiona y consulta el historial de tus pacientes.</p>
              </div>
            </div>

            {/* BARRA DE HERRAMIENTAS */}
            <div className="bg-slate-900 p-4 rounded-2xl border border-slate-800 mb-6 flex flex-wrap gap-4 items-center justify-between">
              <div className="flex items-center gap-3 bg-slate-950 px-4 py-2 rounded-xl border border-slate-800 w-full md:w-96">
                <Search size={18} className="text-slate-500" />
                <input type="text" placeholder="Buscar por Nombre o DNI..." className="bg-transparent border-none outline-none text-sm text-slate-300 w-full" />
              </div>
            </div>

            {/* TABLA DE DATOS */}
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
                  
                  {/* LÓGICA DE CARGA Y RENDERIZADO */}
                  {cargando ? (
                    <tr><td colSpan="6" className="p-10 text-center text-slate-500 italic">Conectando con el servidor hospitalario...</td></tr>
                  ) : error ? (
                    <tr><td colSpan="6" className="p-10 text-center text-red-400">{error}</td></tr>
                  ) : (
                    pacientes.map((paciente) => (
                      <tr key={paciente.id} className="hover:bg-slate-800/30 transition-colors group">
                        <td className="p-5 text-cyan-500 font-mono text-sm">#000{paciente.id}</td>
                        <td className="p-5">
                          <div className="flex items-center gap-3">
                            <div className="w-9 h-9 rounded-full bg-slate-800 border border-slate-700 flex items-center justify-center text-xs font-bold text-slate-400">
                              {paciente.nombreCompleto.charAt(0)}
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
                          <button className="text-cyan-400 text-xs font-bold flex items-center gap-1 ml-auto hover:text-cyan-300 transition-colors">
                            Ver Historial <ExternalLink size={14} />
                          </button>
                        </td>
                      </tr>
                    ))
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