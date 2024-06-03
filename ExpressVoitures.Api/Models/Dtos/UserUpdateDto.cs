using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresApi.Models.Entities
{
    public class UserUpdateDto
    {
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        public string firstname { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters")]
        public string lastname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string email { get; set; }

        public string? token { get; set; }

        public string? refresh_token { get; set; }

        public DateTime? refresh_token_expiry_time { get; set; }
    }
}