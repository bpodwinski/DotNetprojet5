using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpressVoituresApi.Models.Dtos
{
    public class PurchaseDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [JsonIgnore]
        public int vehicle_id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [SwaggerSchema(Description = "The date of the purchase.")]
        [CustomValidation(typeof(PurchaseDto), nameof(ValidateDate))]
        public DateTime date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        [SwaggerSchema(Description = "The price of the purchase.")]
        public decimal price { get; set; }

        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the future.", new[] { nameof(date) });
            }
            return ValidationResult.Success;
        }
    }
}