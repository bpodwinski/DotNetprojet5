using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
    public class Vehicle
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Code VIN")]
		public string? Vin { get; set; }

		[Display(Name = "Année")]
		[Required(ErrorMessage = "Il manque l'année du véhicule")]
		[RegularExpression("^[0-9]+$", ErrorMessage = "L'année doit être un nombre")]
		public int Year { get; set; }

		[Display(Name = "Date d'achat")]
		[Required(ErrorMessage = "La date d'achat de la vehicle doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        public DateTime PurchaseDate { get; set; }

		[Display(Name = "Prix d'achat")]
		[Required(ErrorMessage = "Le prix d'achat de la vehicle doit être complétée")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix d'achat doit être un nombre")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        public DateTime? AvailabilityDate { get; set; }

        [Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        public DateTime? SaleDate { get; set; }

        [Display(Name = "Marque")]
        public int BrandId { get; set; }
        [NotMapped]
        [Display(Name = "Ajouter une marque")]
        public string? BrandAdd { get; set; }
        public virtual Brand Brand { get; set; }

        [Display(Name = "Modèle")]
        public int ModelId { get; set; }
        [NotMapped]
        [Display(Name = "Ajouter un modèle")]
        public string? ModelAdd { get; set; }
        public virtual Model Model { get; set; }

        [Display(Name = "Finition")]
        public int TrimLevelId { get; set; }
        [NotMapped]
        [Display(Name = "Ajouter une finition")]
        public string? TrimLevelAdd { get; set; }
        public virtual TrimLevel TrimLevel { get; set; }

		[Display(Name = "Description")]
		public string? Description { get; set; }

		[Display(Name = "Image")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Détails du véhicule")]
        public string VehicleTitle => $"{Year} - {Brand?.Name} {Model?.Name} {TrimLevel?.Name}";

        [NotMapped]
        [Display(Name = "Prix de vente")]
        public decimal? SalePrice => PurchasePrice + (TotalRepairCost ?? 0) + 500;

		[NotMapped]
        [Display(Name = "Coûts réparations")]
        public decimal? TotalRepairCost { get; set; }
        public ICollection<Repair>? Repairs { get; set; }
    }
}
