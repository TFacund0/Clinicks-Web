using System.Security.Claims;

namespace ClinicksApi.Extensions
{
    /// <summary>
    /// Contiene métodos de extensión para facilitar la manipulación y extracción de datos del usuario autenticado (ClaimsPrincipal).
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Extrae y convierte el ID del médico desde los Claims del Token JWT del usuario autenticado.
        /// </summary>
        /// <param name="user">El usuario actual (User) provisto por el contexto HTTP.</param>
        /// <returns>El ID numérico del médico si es válido. Si no existe o es inválido, retorna null.</returns>
        public static int? GetMedicoId(this ClaimsPrincipal user)
        {
            var idMedicoStr = user.FindFirst("idMedico")?.Value;

            if (string.IsNullOrEmpty(idMedicoStr) || !int.TryParse(idMedicoStr, out int idMedico))
            {
                return null;
            }

            return idMedico;
        }
    }
}
