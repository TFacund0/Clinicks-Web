using ClinicksApi.Business.Interfaces;

namespace ClinicksApi.Workers
{
    /// <summary>
    /// Worker de background que se ejecuta periódicamente para actualizar el estado de los turnos vencidos.
    /// Delega toda la lógica de negocio a <see cref="ITurnoService"/>, respetando la arquitectura en capas.
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
                    using var scope = _serviceProvider.CreateScope();
                    var turnoService = scope.ServiceProvider.GetRequiredService<ITurnoService>();

                    int turnosCancelados = await turnoService.CancelarTurnosVencidosAsync();

                    if (turnosCancelados > 0)
                    {
                        _logger.LogInformation(
                            "ServicioLimpiezaTurnos: {Cantidad} turno(s) vencido(s) marcados como Cancelados.",
                            turnosCancelados);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en ServicioLimpiezaTurnos durante la limpieza de turnos vencidos.");
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
