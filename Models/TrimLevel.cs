using System.ComponentModel.DataAnnotations;

namespace ExpressVoituresV2.Models
{
	[Display(Name = "Finition")]
	public class TrimLevel
	{
        [Key]
        public int Id { get; set; }

		[Display(Name = "Finition")]
		public string Name { get; set; }

		[Display(Name = "Modèle")]
		public int ModelId { get; set; }
		public virtual Model Model { get; set; }
    }
}
