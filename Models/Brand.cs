using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.Models
{
	public class Brand
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Le nom de la marque est obligatoire.")]
		[StringLength(100, ErrorMessage = "Le nom de la marque ne doit pas dépasser 100 caractères.")]
		[Display(Name = "Marque")]
		public string Name { get; set; }
	}
}
