import './index.css' // Asegúrate de que esta línea esté para cargar Tailwind

function App() {
  return (
    <div className="min-h-screen bg-slate-50 flex items-center justify-center p-6">
      <div className="bg-white shadow-2xl rounded-3xl p-10 max-w-md w-full border-t-8 border-blue-600">
        <div className="flex justify-center mb-6">
          <div className="bg-blue-100 p-4 rounded-full">
            <svg xmlns="http://www.w3.org/2000/svg" className="h-12 w-12 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
            </svg>
          </div>
        </div>
        <h1 className="text-3xl font-black text-slate-800 mb-2">Clinicks</h1>
        <p className="text-slate-500 mb-8 uppercase tracking-widest text-sm font-semibold">Gestión Hospitalaria</p>
        
        <div className="space-y-4">
          <button className="w-full bg-blue-600 hover:bg-blue-700 text-white font-bold py-4 rounded-xl transition-all shadow-lg shadow-blue-200">
            Ingresar al Sistema
          </button>
          <button className="w-full bg-white border-2 border-slate-200 hover:border-blue-600 text-slate-600 font-bold py-4 rounded-xl transition-all">
            Ver Turnos Disponibles
          </button>
        </div>
        
        <p className="mt-8 text-xs text-slate-400">
          Proyecto Final - Ingeniería de Software II
        </p>
      </div>
    </div>
  )
}

export default App