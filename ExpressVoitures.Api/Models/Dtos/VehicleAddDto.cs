using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressVoituresApi.Models.Entities;

namespace ExpressVoituresApi.Models.Dtos
{
    public class VehicleAddDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Please enter a valid 17-character VIN")]
        public string vin { get; set; }

        [Required]
        [Range(1990, 9999, ErrorMessage = "Please enter a valid year between 1886 and 9999")]
        public int year { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Brand name must be between 1 and 50 characters")]
        public string brand { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Model name must be between 1 and 50 characters")]
        public string model { get; set; }

        [StringLength(50, ErrorMessage = "Trim Level name cannot exceed 50 characters")]
        public string trim_level { get; set; }
    }
}