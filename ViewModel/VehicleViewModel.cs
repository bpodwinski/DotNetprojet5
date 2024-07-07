using ExpressVoituresV2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.ViewModel
{
	public class VehicleViewModel
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

		[Display(Name = "Date de disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
		public DateTime? AvailabilityDate { get; set; }

		[Display(Name = "Prix de vente")]
		[RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix de vente doit être un nombre")]
		public decimal? SalePrice { get; set; }

		[Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
		public DateTime? SaleDate { get; set; }

		[Display(Name = "Marque")]
		public int BrandId { get; set; }
		[Display(Name = "Ajouter une marque")]
		[NotMapped]
		public string? BrandAdd { get; set; }
		public virtual Brand Brand { get; set; }

		[Display(Name = "Modèle")]
		public int ModelId { get; set; }
		[Display(Name = "Ajouter un modèle")]
		[NotMapped]
		public string? ModelAdd { get; set; }
		public virtual Model Model { get; set; }

		[Display(Name = "Finition")]
		public int TrimLevelId { get; set; }
		[Display(Name = "Ajouter une finition")]
		[NotMapped]
		public string? TrimLevelAdd { get; set; }
		public virtual TrimLevel TrimLevel { get; set; }

		public ICollection<Repair>? Repairs { get; set; }

		[Display(Name = "Total des réparations")]
		[NotMapped]
		public decimal? TotalRepairCost { get; set; }
	}
}
