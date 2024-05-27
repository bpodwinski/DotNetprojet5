using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int vehicle_id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        public DateTime availability_date { get; set; }

        public DateTime sale_date { get; set; }

        public decimal price { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        [ForeignKey("vehicle_id")]
        [JsonIgnore]
        public virtual Vehicle vehicle { get; set; }
    }
}