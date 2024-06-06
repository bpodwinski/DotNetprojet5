using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class UserLoginDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string password { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? token { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public string? refresh_token { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime? refresh_token_expiry_time { get; set; }
    }
}