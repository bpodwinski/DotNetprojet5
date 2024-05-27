using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class Repair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [JsonIgnore]
        public int vehicle_id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        public string description { get; set; }

        public decimal cost { get; set; }

        [ForeignKey("vehicle_id")]
        [JsonIgnore]
        public virtual Vehicle vehicle { get; set; }
    }
}