using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class TokenDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        public string token { get; set; }

        public string? refresh_token { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? refresh_token_expiry_time { get; set; }
    }
}