using System;

namespace BikeRental.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int BikeId { get; set; }
        public int UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}


