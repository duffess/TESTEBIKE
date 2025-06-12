namespace BikeRental.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; } // Adicionado
        public int Year { get; set; }     // Adicionado
        public bool IsAvailable { get; set; }
        public decimal PricePerHour { get; set; }
        public string Status => IsAvailable ? "Available" : "Unavailable";
    }
}
