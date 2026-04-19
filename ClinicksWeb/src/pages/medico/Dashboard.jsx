// 1. IMPORTACIÓN DE COMPONENTES Y LIBRERÍAS
// Importamos los componentes que ya documentamos antes para armar la estructura general
import Sidebar from '../../components/Sidebar';
import Header from '../../components/Header';
// Importamos iconos específicos para las acciones del médico
import { ClipboardPlus, Activity, ExternalLink } from 'lucide-react';

/**
 * Componente Dashboard: Es la vista principal del Médico.
 * Organiza la información en un layout de tres secciones: Navegación, Cabecera y Contenido Principal.
 */
const Dashboard = () => {
  return (
    // CONTENEDOR RAÍZ 
    <div className="flex h-screen bg-slate-950 text-slate-200 overflow-hidden font-sans">
      
      {/* Lado Izquierdo: Menú de navegación */}
      <Sidebar />

      {/* Lado Derecho: Cabecera + Área de trabajo */}
      <div className="flex-1 flex flex-col">
        <Header />

        {/* CONTENIDO PRINCIPAL (MAIN) */}
        <main className="p-8 overflow-y-auto">
          
          {/* SECCIÓN DE BIENVENIDA Y PACIENTE ACTUAL
              Muestra el contexto actual del médico en tiempo real.
          */}
          <div className="flex justify-between items-end mb-8">
            <h1 className="text-4xl font-bold">Buenos días, Dr. Alex Carter</h1>
            
            {/* Widget de Paciente Actual (Atención en curso) */}
            <div className="bg-slate-900 p-3 rounded-xl border border-slate-800 flex items-center gap-4">
               <div className="text-xs">
                  <p className="text-slate-500">Paciente Actual:</p>
                  <p className="font-bold text-cyan-400">Emily Davis (ID: 10230)</p>
               </div>
               <button className="bg-cyan-500 text-slate-950 px-3 py-1 rounded-lg text-xs font-bold hover:bg-cyan-400 transition-colors">
                 Ver Detalles
               </button>
            </div>
          </div>

          {/* GRID PRINCIPAL */}
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            
            {/* COLUMNA IZQUIERDA (1 de 3): Acciones Rápidas y Agenda */}
            <div className="space-y-8">
              
              {/* Botones de Acción (Nueva Consulta / Procedimiento) */}
              <div className="grid grid-cols-2 gap-4">
                
                {/* ... Botón de Nueva Consulta ... */}
                <button className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-cyan-500/50 transition-all group">
                  <div className="p-4 bg-cyan-500/10 rounded-full text-cyan-400 group-hover:bg-cyan-500 group-hover:text-slate-950 transition-colors">
                    <ClipboardPlus size={32} />
                  </div>
                  <span className="font-bold">Crear Nueva Consulta</span>
                </button>

                {/* ... Botón de Nuevo Procedimiento ... */}
                <button className="bg-slate-900 p-6 rounded-2xl border border-slate-800 flex flex-col items-center gap-4 hover:border-cyan-500/50 transition-all group">
                  <div className="p-4 bg-cyan-500/10 rounded-full text-cyan-400 group-hover:bg-cyan-500 group-hover:text-slate-950 transition-colors">
                    <ClipboardPlus size={32} />
                  </div>
                  <span className="font-bold">Crear Nuevo procedimiento</span>
                </button>
              </div>

              {/* Mi Agenda: Lista breve de los próximos turnos */}
              <div className="bg-slate-900 p-6 rounded-2xl border border-slate-800">
                <h3 className="text-xl font-bold mb-4">Mi Agenda de Hoy</h3>
                <div className="space-y-4">
                  
                  {/* .map() para simular la lista de turnos (Iteración) */}
                  {[1, 2].map((i) => (
                    <div key={i} className="flex items-center justify-between p-4 bg-slate-950 rounded-xl border border-slate-800">
                      <div className="flex items-center gap-3">
                        <span className="text-xs text-slate-500">11:30 AM</span>
                        <div>
                          <p className="text-sm font-bold">Paciente Ejemplo {i}</p>
                          <p className="text-[10px] text-slate-500 uppercase">Consulta General</p>
                        </div>
                      </div>
                      <button className="bg-cyan-500/20 text-cyan-400 px-4 py-1 rounded-lg text-xs font-bold">Iniciar</button>
                    </div>
                  ))}
                </div>
              </div>
            </div>

            {/* COLUMNA DERECHA (2 de 3): Tabla de Pacientes Recientes */}
            <div className="lg:col-span-2 bg-slate-900 rounded-2xl border border-slate-800 overflow-hidden">
              <div className="p-6 border-b border-slate-800">
                <h3 className="text-xl font-bold">Listado de Pacientes Recientes</h3>
              </div>
              
              {/* TABLA DE DATOS: Estructura estándar para mostrar registros de la DB */}
              <table className="w-full text-left">
                <thead className="bg-slate-950 text-slate-500 text-xs uppercase tracking-widest">
                  <tr>
                    <th className="p-4">ID</th>
                    <th className="p-4">Paciente</th>
                    <th className="p-4">Edad</th>
                    <th className="p-4">Estado</th>
                    <th className="p-4">Acción</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-800">
                  
                  {/* Simulamos 5 pacientes recientes */}
                  {[1, 2, 3, 4, 5].map((i) => (
                    <tr key={i} className="hover:bg-slate-800/30 transition-colors">
                      <td className="p-4 text-cyan-500 font-mono">#000{i}</td>
                      <td className="p-4 flex items-center gap-3 text-sm text-slate-300">
                         Nombre Apellido
                      </td>
                      <td className="p-4 text-sm text-slate-400">28</td>
                      <td className="p-4">
                        <span className="bg-green-500/10 text-green-500 px-2 py-1 rounded text-[10px] font-bold uppercase">Activo</span>
                      </td>
                      <td className="p-4">
                        <button className="text-cyan-400 text-xs flex items-center gap-1 hover:underline">
                          Ver Detalles <ExternalLink size={12} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

          </div>
        </main>
      </div>
    </div>
  );
};

export default Dashboard;