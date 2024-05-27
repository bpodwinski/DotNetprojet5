using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class SaleDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [JsonIgnore]
        public int vehicle_id { get; set; }

        [Required]
        public DateTime create_date { get; set; }

        [Required]
        public DateTime availability_date { get; set; }

        [Required]
        public DateTime sale_date { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal price { get; set; }

        [Required]
        [StringLength(100)]
        public string title { get; set; }

        [StringLength(500)]
        public string description { get; set; }
    }
}