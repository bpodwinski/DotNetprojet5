using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoitures.Models
{
	[Display(Name = "Finition")]
	public class TrimLevel
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Finition")]
		public string Name { get; set; } = string.Empty;

		[NotMapped]
		[Display(Name = "Modèle")]
		public int ModelId { get; set; }

		[NotMapped]
		public virtual Model? Model { get; set; }
    }
}
