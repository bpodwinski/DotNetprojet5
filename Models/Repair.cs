using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
    [Display(Name = "Réparation")]
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Réparation")]
        [Required(ErrorMessage = "La réparation doit être complétée.")]
        public string Name { get; set; } = string.Empty;

		[Display(Name = "Coût")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le coût doit être supérieur à 0.")]
        public decimal? Cost { get; set; }

		[Display(Name = "Voiture")]
		public int VehicleId { get; set; }

		[Display(Name = "Voiture")]
        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }
    }
}
