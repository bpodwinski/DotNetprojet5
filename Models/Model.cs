using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
	public class Model
	{
		public int Id { get; set; }

		[Display(Name = "Modèle")]
		public string Name { get; set; }

		[ForeignKey("Brand")]
		public int BrandId { get; set; }
		public virtual Brand Brand { get; set; }
	}
}
