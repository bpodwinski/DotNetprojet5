using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpressVoituresV2.ViewModel
{
	public class VehicleViewModel
	{
		public string Vin { get; set; }
		public int Year { get; set; }
		public DateTime PurchaseDate { get; set; }
		public float PurchasePrice { get; set; }
		public DateTime? AvailabilityDate { get; set; }
		public DateTime? SaleDate { get; set; }
		public float? SalePrice { get; set; }
		public int BrandId { get; set; }
		public string? BrandAdd { get; set; }
		public string? BrandList { get; set; }
	}
}
