using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Vin { get; set; }

        [Required]
        [Range(1886, 9999, ErrorMessage = "Please enter a valid year between 1886 and 9999")]
        public int Year { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string TrimLevel { get; set; }
    }
}