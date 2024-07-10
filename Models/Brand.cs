using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
	[Display(Name = "Marque")]
	public class Brand
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Marque")]
		public string Name { get; set; } = string.Empty;

		[NotMapped]
		[Display(Name = "Modèle")]
		public virtual ICollection<Model>? Models { get; set; }
	}
}
