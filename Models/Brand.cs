using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.Models
{
	[Display(Name = "Marque")]
	public class Brand
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Marque")]
		public string Name { get; set; }

		[Display(Name = "Modèle")]
		public virtual ICollection<Model> Models { get; set; }
	}
}
