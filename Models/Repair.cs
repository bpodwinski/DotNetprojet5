﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
    [Display(Name = "Réparation")]
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Réparation")]
        public string Name { get; set; }

        [Display(Name = "Coût")]
        public float Cost { get; set; }

		[Display(Name = "Voiture")]
		public int VehicleId { get; set; }

        [Display(Name = "Voiture")]
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
