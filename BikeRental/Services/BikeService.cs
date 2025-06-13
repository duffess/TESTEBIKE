using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System;

namespace BikeRental.Services
{
    public static class BikeService
    {
        private static List<BikeRental.Models.Bike> bikes;
        private static readonly string BikesFilePath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Data", "bikes.json");

        static BikeService()
        {
            LoadBikes();
        }

        private static void LoadBikes()
        {
            if (File.Exists(BikesFilePath))
            {
                string json = File.ReadAllText(BikesFilePath);
                bikes = JsonConvert.DeserializeObject<List<BikeRental.Models.Bike>>(json);
            }
            else
            {
                bikes = new List<BikeRental.Models.Bike>();
                SaveBikes();
            }
        }

        private static void SaveBikes()
        {
            string json = JsonConvert.SerializeObject(bikes, Formatting.Indented);
            File.WriteAllText(BikesFilePath, json);
        }

        public static void AddBike(BikeRental.Models.Bike bike)
        {
            if (bike.Id == 0)
            {
                bike.Id = bikes.Any() ? bikes.Max(b => b.Id) + 1 : 1;
            }
            bikes.Add(bike);
            SaveBikes();
        }

        public static List<BikeRental.Models.Bike> GetAllBikes()
        {
            return bikes.ToList();
        }

        public static void UpdateBike(BikeRental.Models.Bike updatedBike)
        {
            var index = bikes.FindIndex(b => b.Id == updatedBike.Id);
            if (index != -1)
            {
                bikes[index] = updatedBike;
                SaveBikes();
            }
        }

        public static void DeleteBike(int id)
        {
            var bike = bikes.FirstOrDefault(b => b.Id == id);
            if (bike != null)
            {
                bikes.Remove(bike);
                SaveBikes();
            }
        }
    }
}


