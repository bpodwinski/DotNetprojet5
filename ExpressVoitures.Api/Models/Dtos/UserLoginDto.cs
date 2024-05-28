using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string password { get; set; }
    }
}