using Swashbuckle.AspNetCore.Annotations;

namespace Express_Voitures.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}