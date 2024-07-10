using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
	[Display(Name = "Modèle")]
	public class Model
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Modèle")]
		public string Name { get; set; } = string.Empty;

		[NotMapped]
		[Display(Name = "Marque")]
		public int BrandId { get; set; }
		public virtual Brand? Brand { get; set; }

		[NotMapped]
		public virtual ICollection<TrimLevel>? TrimLevels { get; set; }
    }
}
