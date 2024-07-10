using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ExpressVoituresV2.Models
{
    public class Vehicle
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Code VIN")]
		[Vin(ErrorMessage = "Le code VIN doit être une chaîne de 17 caractères contenant uniquement des lettres majuscules (à l'exception de I, O, Q) et des chiffres.")]
		public string? Vin { get; set; }

		[Display(Name = "Année")]
		[Required(ErrorMessage = "Il manque l'année du véhicule")]
		[YearRange(1990, ErrorMessage = "L'année doit être entre 1990 et l'année en cours.")]
		public int Year { get; set; }

		[Display(Name = "Date d'achat")]
		[Required(ErrorMessage = "La date d'achat de la vehicle doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        public DateTime PurchaseDate { get; set; }

		[Display(Name = "Prix d'achat")]
		[Required(ErrorMessage = "Le prix d'achat de la vehicle doit être complétée")]
        public decimal PurchasePrice { get; set; }

        [Display(Name = "Disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        public DateTime? AvailabilityDate { get; set; }

        [Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        public DateTime? SaleDate { get; set; }

        [Display(Name = "Marque")]
		[Required(ErrorMessage = "Il manque la marque")]
		public int BrandId { get; set; }
        [NotMapped]
        public virtual Brand Brand { get; set; }

        [Display(Name = "Modèle")]
		[Required(ErrorMessage = "Il manque le modèle")]
		public int ModelId { get; set; }
        [NotMapped]
        public virtual Model Model { get; set; }

        [Display(Name = "Finition")]
		[Required(ErrorMessage = "Il manque la finition")]
		public int TrimLevelId { get; set; }
        [NotMapped]
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

        [Display(Name = "Coûts réparations")]
        public decimal? TotalRepairCost { get; set; }
        public ICollection<Repair>? Repairs { get; set; }
    }
}

public class VinAttribute : ValidationAttribute
{
	private const int VinLength = 17;
	private static readonly Regex VinRegex = new Regex(@"^[A-HJ-NPR-Z0-9]{17}$", RegexOptions.Compiled);

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is string vin)
		{
			if (vin.Length == VinLength && VinRegex.IsMatch(vin))
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult("Le code VIN doit être une chaîne de 17 caractères contenant uniquement des lettres majuscules (à l'exception de I, O, Q) et des chiffres.");
			}
		}
		return new ValidationResult("Valeur du VIN invalide.");
	}
}

public class YearRangeAttribute : ValidationAttribute
{
	private readonly int _minimumYear;

	public YearRangeAttribute(int minimumYear)
	{
		_minimumYear = minimumYear;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		if (value is int year)
		{
			int currentYear = DateTime.Now.Year;
			if (year >= _minimumYear && year <= currentYear)
			{
				return ValidationResult.Success;
			}
			else
			{
				return new ValidationResult($"L'année doit être entre {_minimumYear} et {currentYear}.");
			}
		}
		return new ValidationResult("Valeur invalide.");
	}
}
