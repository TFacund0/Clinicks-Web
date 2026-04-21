using System.Text.Json.Serialization;

namespace ClinicksApi.Business.Dtos
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}