using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing.Drawing2D;

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

        public virtual Brand brand { get; set; }

        public virtual Model model { get; set; }

        public virtual Trim_Level trim_level { get; set; }

        public decimal purchase_price { get; set; }

        public DateTime purchase_date { get; set; }

        public decimal sale_price { get; set; }

        public DateTime availability_date { get; set; }

        public DateTime sale_date { get; set; }

        public virtual ICollection<Repair> repair { get; set; }
    }
}
