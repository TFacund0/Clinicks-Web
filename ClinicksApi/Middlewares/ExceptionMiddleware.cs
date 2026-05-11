using System.Net;
using System.Text.Json;

namespace ClinicksApi.Middlewares
{
    /// <summary>
    /// Middleware de captura global de errores.
    /// Funciona como una red de seguridad: intercepta cualquier excepción (error) no manejada
    /// en la aplicación, evita que el servidor se caiga y devuelve una respuesta HTTP 500
    /// en formato JSON amigable para el frontend.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Constructor del Middleware.
        /// </summary>
        /// <param name="next">El puntero que indica cuál es el siguiente paso en la tubería (Pipeline).</param>
        /// <param name="logger">Herramienta para escribir errores en la consola negra del servidor.</param>
        /// <param name="env">Nos dice si estamos en modo "Development" (Desarrollo) o "Production".</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Método interceptor que se ejecuta en TODAS las peticiones HTTP.
        /// Intenta ejecutar el código normal y, si algo explota, lo atrapa en el catch.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Construye la respuesta de emergencia que se enviará a React cuando ocurra un desastre.
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocurrió un error interno en el servidor. Por favor, intente más tarde.",
                Detail = _env.IsDevelopment() ? exception.StackTrace?.ToString() : null
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
