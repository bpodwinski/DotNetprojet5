using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class RefreshTokenDto
    {
        public string? token { get; set; }

        public string? refresh_token { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? refresh_token_expiry_time { get; set; }
    }
}