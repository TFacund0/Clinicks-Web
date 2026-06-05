using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato para el servicio encargado de la generación y gestión de tokens de seguridad (JWT).
    /// Abstrae la lógica de cifrado e identidad fuera del servicio de autenticación.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Genera un token JWT firmado para el médico provisto.
        /// </summary>
        /// <param name="medico">Entidad de médico para la cual generar el token.</param>
        /// <returns>String con el token JWT.</returns>
        string GenerarToken(Medico medico);
    }
}
