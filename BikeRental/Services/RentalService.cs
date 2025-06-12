using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using BikeRental.Models;
using System;

namespace BikeRental.Services
{
    public static class RentalService
    {
        private static List<Rental> rentals;
        private static readonly string RentalsFilePath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Data", "rentals.json");

        static RentalService()
        {
            LoadRentals();
        }

        private static void LoadRentals()
        {
            if (File.Exists(RentalsFilePath))
            {
                string json = File.ReadAllText(RentalsFilePath);
                rentals = JsonConvert.DeserializeObject<List<Rental>>(json);
            }
            else
            {
                rentals = new List<Rental>();
                SaveRentals();
            }
        }

        private static void SaveRentals()
        {
            string json = JsonConvert.SerializeObject(rentals, Formatting.Indented);
            File.WriteAllText(RentalsFilePath, json);
        }

        public static void AddRental(Rental rental)
        {
            if (rental.Id == 0)
            {
                rental.Id = rentals.Any() ? rentals.Max(r => r.Id) + 1 : 1;
            }
            rentals.Add(rental);
            SaveRentals();
        }

        public static List<Rental> GetAllRentals()
        {
            return rentals.ToList();
        }

        public static void UpdateRental(Rental updatedRental)
        {
            var index = rentals.FindIndex(r => r.Id == updatedRental.Id);
            if (index != -1)
            {
                rentals[index] = updatedRental;
                SaveRentals();
            }
        }

        public static void DeleteRental(int id)
        {
            var rental = rentals.FirstOrDefault(r => r.Id == id);
            if (rental != null)
            {
                rentals.Remove(rental);
                SaveRentals();
            }
        }
    }
}


