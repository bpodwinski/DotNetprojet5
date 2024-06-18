using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ExpressVoituresApi.Models.Entities
{
    public class Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int vehicle_id { get; set; }

        public string title { get; set; }

        [ForeignKey("vehicle_id")]
        [JsonIgnore]
        public virtual Vehicle vehicle { get; set; }
    }
}