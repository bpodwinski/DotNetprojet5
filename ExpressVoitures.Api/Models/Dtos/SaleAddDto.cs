using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class SaleAddDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int id { get; set; }

        [ForeignKey("Vehicle")]
        [SwaggerSchema(ReadOnly = true)]
        public int vehicle_id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        [Required]
        public DateTime availability_date { get; set; }

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