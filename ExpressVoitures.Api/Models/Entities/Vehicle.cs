using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [SwaggerSchema(ReadOnly = true)]
        public DateTime create_date { get; set; }

        public string vin { get; set; }

        public int year { get; set; }

        public string brand { get; set; }

        public string model { get; set; }

        public string trim_level { get; set; }

        public virtual Purchase purchase { get; set; }

        public virtual Sale sale { get; set; }

        public virtual ICollection<Repair> repair { get; set; }
    }
}