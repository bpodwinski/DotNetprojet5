using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class PurchaseDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [SwaggerSchema(Description = "The date of the purchase.")]
        [CustomValidation(typeof(PurchaseDto), nameof(ValidateDate))]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [SwaggerSchema(Description = "The price of the purchase.")]
        public decimal Price { get; set; }

        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the future.", new[] { nameof(Date) });
            }
            return ValidationResult.Success;
        }
    }
}