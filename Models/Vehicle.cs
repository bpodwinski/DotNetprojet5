using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
    public class Vehicle : IValidatableObject
	{
        public int Id { get; set; }

        [Display(Name = "Code VIN")]
		public string? Vin { get; set; }

		[YearValidation(1990)]
		[Display(Name = "Année")]
		[Required(ErrorMessage = "Il manque l'année du véhicule")]
		[RegularExpression("^[0-9]+$", ErrorMessage = "L'année doit être un nombre")]
		public int Year { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (Year < 1990 || Year > DateTime.Now.Year)
			{
				yield return new ValidationResult(
					$"L'année doit être entre 1990 et {DateTime.Now.Year}.",
					new[] { nameof(Year) });
			}
		}

		[Display(Name = "Marque")]
		public int BrandId { get; set; }
		[ForeignKey("BrandId")]
		public virtual Brand Brand { get; set; }

        [Display(Name = "Date d'achat")]
		[Required(ErrorMessage = "La date d'achat de la vehicle doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        [PurchaseDateValidation]
        public DateTime PurchaseDate { get; set; }

		[Display(Name = "Prix d'achat")]
		[Required(ErrorMessage = "Le prix d'achat de la vehicle doit être complétée")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix d'achat doit être un nombre")]
        [PurchasePriceValidation]
        public float PurchasePrice { get; set; }

        [Display(Name = "Date de disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        [AvailabilityDateValidation]
        public DateTime? AvailabilityDate { get; set; }

        [Display(Name = "Prix de vente")]
		[RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix de vente doit être un nombre")]
        [SalePriceValidation(500)]
        public float? SalePrice { get; set; }

        [Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        [SaleDateValidation]
        public DateTime? SaleDate { get; set; }

		//public ICollection<Repair>? Repairs { get; set; }
    }

    public class YearValidationAttribute : ValidationAttribute
    {
	    private readonly int _minYear;
	    public YearValidationAttribute(int minYear)
	    {
		    _minYear = minYear;
		    ErrorMessage = "L'année doit être supérieure à " + minYear;
	    }

	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
		    if (value is int year)
		    {
			    if (year > _minYear)
			    {
				    return ValidationResult.Success;
			    }
		    }
		    return new ValidationResult(ErrorMessage);
	    }
    }

	public class PurchaseDateValidationAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	    {
		    DateTime? date = value as DateTime?;

		    var vehicle = (Vehicle)validationContext.ObjectInstance;

		    if (date == null || (date >= new DateTime(1990, 1, 1) && date <= DateTime.Now) && date.Value.Year >= vehicle.Year)
		    {
			    return ValidationResult.Success;
		    }
		    return new ValidationResult("La date d'achat doit être postérieure à l'Année");
	    }
    }

    public class PurchasePriceValidationAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	    {
		    float? PrixAchat = value as float?;

		    if (PrixAchat != null && (PrixAchat >= 500))
		    {
			    return ValidationResult.Success;
		    }
		    return new ValidationResult("Le prix d'achat doit être de 500 € minimum");
	    }
    }

    public class AvailabilityDateValidationAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
		    DateTime? date = value as DateTime?;
		    if (validationContext == null)
		    {
			    throw new ArgumentNullException(nameof(validationContext));
		    }

		    var dateAchat = (DateTime)validationContext.ObjectInstance.GetType().GetProperty("PurchaseDate").GetValue(validationContext.ObjectInstance);

		    if (date == null || (date >= dateAchat && date <= DateTime.Now.AddYears(1)))
		    {
			    return ValidationResult.Success;
		    }
		    return new ValidationResult("La date de disponibilité de vente doit être postérieure ou égale à la date d'achat et inférieur ou égale à la date du jour + 1 an");
	    }
    }

    public class SalePriceValidationAttribute : ValidationAttribute
    {
	    private readonly float _marge;

	    public SalePriceValidationAttribute(float marge)
	    {
		    _marge = marge;
	    }
	    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	    {
		    float? SalePrice = value as float?;
		    var voiture = (Vehicle)validationContext.ObjectInstance;

		    if (SalePrice == null)
		    {
			    return new ValidationResult("Le prix de vente doit être complété avec le prix d'achat + marge qui est de 500 € minimum");
		    }
		    else if (SalePrice >= voiture.PurchasePrice + _marge)
		    {
			    return ValidationResult.Success;
		    }
		    else
		    {
			    return new ValidationResult("Il n'est pas possible de vendre une vehicle en dessous du prix d'achat + la marge qui est de 500 € minimum");
		    }
	    }
    }

    public class SaleDateValidationAttribute : ValidationAttribute
    {
	    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
	    {
		    DateTime? dateDeVente = value as DateTime?;
		    if (validationContext == null)
		    {
			    throw new ArgumentNullException(nameof(validationContext));
		    }

		    var dateDisponibiliteDeVente = (DateTime?)validationContext.ObjectInstance.GetType().GetProperty("AvailabilityDate").GetValue(validationContext.ObjectInstance);

		    if (dateDeVente != null && dateDisponibiliteDeVente == null)
		    {
			    return new ValidationResult("Veuillez remplir la Date disponibilité de Vente");
		    }
		    else if (dateDeVente == null || (dateDeVente >= dateDisponibiliteDeVente && dateDeVente <= DateTime.Now))
		    {
			    return ValidationResult.Success;
		    }
		    return new ValidationResult("La date de vente doit être supérieure à la date de disponibilité de vente et inférieure ou égale à la date du jour");
	    }
    }
}
