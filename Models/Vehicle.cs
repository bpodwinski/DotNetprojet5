using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Display(Name = "Code VIN")]
		public string? Vin { get; set; }

        public class AnneeValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                int? annee = value as int?;

                if (annee == null || (annee >= 1990 && annee <= DateTime.Now.Year))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date d'achat doit être postérieure à 1990 et inférieur ou égale à l'année en cours");
            }
        }
        [Display(Name = "Année")]
		[Required(ErrorMessage = "L'année de la vehicle doit être complétée")]
        [RegularExpression("^\\d+$", ErrorMessage = "L'année doit être un entier")]
        [AnneeValidation]
        public int Year { get; set; }

		[Display(Name = "Marque")]
		public int BrandId { get; set; }
		[ForeignKey("BrandId")]
		public virtual Brand Brand { get; set; }

		public class DateAchatValidationAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
            {
                DateTime? date = value as DateTime?;
                
                var voiture = (Vehicle)validationContext.ObjectInstance;

                if (date == null || (date >= new DateTime(1990, 1, 1) && date <= DateTime.Now) && date.Value.Year >= voiture.Year)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("La date d'achat doit être postérieure à l'Année");
            }
        }
        [Display(Name = "Date d'achat")]
		[Required(ErrorMessage = "La date d'achat de la vehicle doit être complétée")]
        [DataType(DataType.Date, ErrorMessage = "La date d'achat doit être une date.")]
        [DateAchatValidation]
        public DateTime PurchaseDate { get; set; }

        public class PrixAchatValidationAttribute : ValidationAttribute
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
		[Display(Name = "Prix d'achat")]
		[Required(ErrorMessage = "Le prix d'achat de la vehicle doit être complétée")]
        [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix d'achat doit être un nombre")]
        [PrixAchatValidation]
        public float PurchasePrice { get; set; }

        public class DateDisponibiliteDeVenteValidationAttribute : ValidationAttribute
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
        [Display(Name = "Date de disponibilité")]
		[DataType(DataType.Date, ErrorMessage = "La date de disponibilité de vente doit être une date.")]
        [DateDisponibiliteDeVenteValidation]
        public DateTime? AvailabilityDate { get; set; }

        public class PrixDeVenteValidationAttribute : ValidationAttribute
        {
            private readonly float _marge;

            public PrixDeVenteValidationAttribute(float marge)
            {
                _marge = marge;
            }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                float? PrixDeVente = value as float?;
                var voiture = (Vehicle)validationContext.ObjectInstance;

                if (PrixDeVente == null)
                {
                    return new ValidationResult("Le prix de vente doit être complété avec le prix d'achat + marge qui est de 500 € minimum");
                }
                else if (PrixDeVente >= voiture.PurchasePrice + _marge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Il n'est pas possible de vendre une vehicle en dessous du prix d'achat + la marge qui est de 500 € minimum");
                }
            }
        }

        [Display(Name = "Prix de vente")]
		[RegularExpression(@"^[0-9]+(\,[0-9]{1,2})?$", ErrorMessage = "Le prix de vente doit être un nombre")]
        [PrixDeVenteValidation(500)]
        public float? SalePrice { get; set; }

        public class DateDeVenteValidationAttribute : ValidationAttribute
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

        [Display(Name = "Date de vente")]
		[DataType(DataType.Date, ErrorMessage = "La date de vente doit être une date.")]
        [DateDeVenteValidation]
        public DateTime? SaleDate { get; set; }

		//public ICollection<Repair>? Repairs { get; set; }
    }
}
