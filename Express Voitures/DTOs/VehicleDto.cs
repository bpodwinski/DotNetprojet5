namespace Express_Voitures.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Vin { get; set; }

        public string Year { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string TrimLevel { get; set; }

        public PurchaseDto Purchase { get; set; }
    }
}