
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressVoituresV2.Models
{
	public class TrimLevel
	{
		public int Id { get; set; }

		[Display(Name = "Finition")]
		public string Name { get; set; }

		[ForeignKey("Model")]
		public int ModelId { get; set; }
		public virtual Model Model { get; set; }
	}
}
