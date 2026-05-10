using System;
using System.Threading.Tasks;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    public class ProcesoService : IProcesoService
    {
        private readonly IProcesoRepository _procesoRepo;
        private readonly IPacienteRepository _pacienteRepo;

        public ProcesoService(IProcesoRepository procesoRepo, IPacienteRepository pacienteRepo)
        {
            _procesoRepo = procesoRepo;
            _pacienteRepo = pacienteRepo;
        }

        public async Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProceso(ProcesoAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                // 1. Validaciones
                if (string.IsNullOrWhiteSpace(dto.tipoproceso)) 
                    return (false, "El tipo de proceso es obligatorio.", null);
                
                if (string.IsNullOrWhiteSpace(dto.descripcion)) 
                    return (false, "La descripción es obligatoria.", null);
                
                if (idMedicoLogueado <= 0) 
                    return (false, "El médico logueado es inválido.", null);

                // 2. Verificar paciente
                var paciente = await _pacienteRepo.GetByDniAsync(dto.dnipaciente);
                if (paciente == null) 
                    return (false, "Paciente no encontrado en la base de datos.", null);

                var fechaAUsar = dto.fechaproceso ?? DateTime.Now;

                var nuevoProcedimiento = new Procedimiento
                {
                    Tipo = dto.tipoproceso,
                    Descripcion = dto.descripcion,
                    Resultado = dto.resultado ?? "Sin resultado ingresado",
                    Fecha = fechaAUsar
                };

                var procGuardado = await _procesoRepo.CrearProcedimiento(nuevoProcedimiento);

                // 4. Asegurarnos que el Estado Turno (1 = Realizado) existe en DB vacía
                int idEstadoHecho = 1;
                await _procesoRepo.AsegurarEstadoTurnoExiste(idEstadoHecho, "Realizado");

                // 5. Vincular el procedimiento con el paciente creando el Turno
                var nuevoTurno = new Turno
                {
                    IdPaciente = paciente.IdPaciente,
                    IdMedico = idMedicoLogueado,
                    IdProcedimiento = procGuardado.IdProcedimiento,
                    IdEstadoTurno = idEstadoHecho, // Asignamos el estado verificado
                    FechaTurno = fechaAUsar,
                    Motivo = $"Procedimiento: {dto.tipoproceso}"
                };

                await _procesoRepo.CrearTurnoVinculado(nuevoTurno);

                return (true, "Procedimiento guardado y vinculado con éxito", procGuardado);
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException != null ? $" Detalle: {ex.InnerException.Message}" : "";
                return (false, $"Error interno al registrar proceso: {ex.Message}{detalle}", null);
            }
        }
    }
}
