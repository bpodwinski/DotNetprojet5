using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
    public class Vehicle
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Code VIN")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le code VIN doit être une chaîne de 17 caractères")]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Le code VIN doit contenir uniquement des lettres majuscules (à l'exception de I, O, Q) et des chiffres.")]
        public string? Vin { get; set; }

		[Display(Name = "Année*")]
		[Required(ErrorMessage = "Il manque l'année du véhicule")]
		[ValidYear(1990, ErrorMessage = "L'année doit être entre 1990 et l'année en cours.")]
		public int Year { get; set; }

		[Display(Name = "Date d'achat*")]
		[Required(ErrorMessage = "La date d'achat du vehicle doit être complétée.")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
		[ValidPurchaseDate]
		public DateTime PurchaseDate { get; set; }

		[Display(Name = "Prix d'achat*")]
		[Required(ErrorMessage = "Le prix d'achat du vehicle doit être complétée.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le prix d'achat doit être supérieur à 0.")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        public DateTime? AvailabilityDate { get; set; }

        [Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        public DateTime? SaleDate { get; set; }

        [Display(Name = "Marque*")]
        [Required(ErrorMessage = "Il manque la marque")]
        public int BrandId { get; set; }
        [NotMapped]
        public virtual Brand Brand { get; set; }

        [Display(Name = "Modèle*")]
        [Required(ErrorMessage = "Il manque le modèle")]
        public int ModelId { get; set; }
        [NotMapped]
        public virtual Model Model { get; set; }

        [Display(Name = "Finition*")]
        [Required(ErrorMessage = "Il manque la finition")]
        public int TrimLevelId { get; set; }
        [NotMapped]
        public virtual TrimLevel TrimLevel { get; set; }

		[Display(Name = "Description")]
        [StringLength(150, ErrorMessage = "La description ne peut pas dépasser 150 caractères.")]
        public string? Description { get; set; }

		[Display(Name = "Image")]
		[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.png|.gif)$", ErrorMessage = "Seules les images avec l'extension .jpg, .jpeg, .png, .gif sont autorisées.")]
		public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Détails du véhicule")]
        public string VehicleTitle => $"{Year} - {Brand?.Name} {Model?.Name} {TrimLevel?.Name}";

		public ICollection<Repair>? Repairs { get; set; }
		[NotMapped]
		[Display(Name = "Coûts réparations")]
		public decimal? TotalRepairCost => Repairs?.Sum(r => r.Cost) ?? 0;

		[NotMapped]
        [Display(Name = "Prix de vente")]
        public decimal? SalePrice => PurchasePrice + (TotalRepairCost ?? 0) + 500;
	}
}

public class ValidYearAttribute : ValidationAttribute
{
	private readonly int _minYear;

	public ValidYearAttribute(int minYear)
	{
		_minYear = minYear;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is int year)
		{
			int currentYear = DateTime.Now.Year;
			if (year >= _minYear && year <= currentYear)
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult($"L'année doit être entre {_minYear} et l'année en cours.");
			}
		}
		return new ValidationResult("L'année n'est pas valide.");
	}
}

public class ValidPurchaseDateAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is DateTime purchaseDate)
		{
			if (purchaseDate > DateTime.Now)
			{
				return new ValidationResult("La date d'achat ne peut pas être dans le futur.");
			}
		}
		return ValidationResult.Success;
	}
}
