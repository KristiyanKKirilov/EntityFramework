using CarDealer.Models;

namespace CarDealer.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public long TraveledDistance { get; set; }
        public List<int> PartsId { get; set; }        
    }
}
