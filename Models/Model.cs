using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
	public class Model
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Le nom du modèle est obligatoire.")]
		[StringLength(100, ErrorMessage = "Le nom du modèle ne doit pas dépasser 100 caractères.")]
		[Display(Name = "Modèle")]
		public string Name { get; set; }

		[ForeignKey("Brand")]
		public int BrandId { get; set; }
		public virtual Brand Brand { get; set; }
	}
}
