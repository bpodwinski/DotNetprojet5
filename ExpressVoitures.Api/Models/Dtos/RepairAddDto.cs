using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Dtos
{
    public class RepairAddDto
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
        [StringLength(200, ErrorMessage = "Repair description cannot exceed 200 characters")]
        public string description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be a positive value")]
        public decimal cost { get; set; }
    }
}