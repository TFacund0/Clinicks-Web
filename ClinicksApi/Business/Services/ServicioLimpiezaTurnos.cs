using ClinicksApi.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClinicksApi.Business.Interfaces;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Servicio de background que se ejecuta periódicamente para limpiar turnos viejos.
    /// Por ejemplo: si un turno "Pendiente" o "Confirmado" ya pasó de su fecha/hora y no fue "Atendido" o "Realizado", 
    /// se marca automáticamente como "Ausente" o "Cancelado".
    /// </summary>
    public class ServicioLimpiezaTurnos : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ServicioLimpiezaTurnos> _logger;

        public ServicioLimpiezaTurnos(IServiceProvider serviceProvider, ILogger<ServicioLimpiezaTurnos> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ServicioLimpiezaTurnos iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var turnoService = scope.ServiceProvider.GetRequiredService<ITurnoService>();
                        
                        int turnosCancelados = await turnoService.CancelarTurnosVencidosAsync();

                        if (turnosCancelados > 0)
                        {
                            _logger.LogInformation($"ServicioLimpiezaTurnos: Se marcaron {turnosCancelados} turnos pasados como Cancelados.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error ocurrido en ServicioLimpiezaTurnos.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
