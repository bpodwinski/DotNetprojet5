using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressVoituresApi.Models.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Vin { get; set; }

        public int Year { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string TrimLevel { get; set; }

        public Purchase Purchase { get; set; }
    }
}