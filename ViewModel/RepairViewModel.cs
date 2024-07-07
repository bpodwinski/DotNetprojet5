using ExpressVoituresV2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.ViewModel
{
	public class RepairViewModel
	{
        [Key]
        public int Id { get; set; }

        [Display(Name = "Réparation")]
        public string Name { get; set; }

        [Display(Name = "Coût")]
        public float? Cost { get; set; }

        [Display(Name = "Voiture")]
        public int VehicleId { get; set; }

        [Display(Name = "Voiture")]
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }

        public float? TotalRepairCost { get; set; }
    }
}
