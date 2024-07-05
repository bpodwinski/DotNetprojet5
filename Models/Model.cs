using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.Models
{
	[Display(Name = "Modèle")]
	public class Model
	{
		public int Id { get; set; }


		[Display(Name = "Modèle")]
		public string Name { get; set; }


		[Display(Name = "Marque")]
		public int BrandId { get; set; }
		public virtual Brand Brand { get; set; }

	}
}
