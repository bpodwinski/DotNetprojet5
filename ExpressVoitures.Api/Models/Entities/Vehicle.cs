using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime CreateDate { get; set; }

        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Please enter a valid 17-character VIN")]
        public string Vin { get; set; }

        [Required]
        [Range(1886, 9999, ErrorMessage = "Please enter a valid year between 1886 and 9999")]
        public int Year { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Brand name must be between 1 and 50 characters")]
        public string Brand { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Model name must be between 1 and 50 characters")]
        public string Model { get; set; }

        [StringLength(50, ErrorMessage = "Trim Level name cannot exceed 50 characters")]
        public string TrimLevel { get; set; }

        public Purchase Purchase { get; set; }
    }
}