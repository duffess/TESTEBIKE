using System.Collections.Generic;
using System.Linq;
using BikeRentalDashboard.Models;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace BikeRental.Services
{
    public static class UserService
    {
        private static List<User> users;
        private static readonly string UsersFilePath = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "Data", "users.json");

        static UserService()
        {
            LoadUsers();
        }

        private static void LoadUsers()
        {
            try
            {
                if (File.Exists(UsersFilePath))
                {
                    string json = File.ReadAllText(UsersFilePath);
                    users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
                }
                else
                {
                    users = new List<User>();
                    Add(new User { Username = "admin", Password = "admin123", Role = "Administrator", Email = "admin@example.com" });
                    Add(new User { Username = "user", Password = "user123", Role = "Client", Email = "user@example.com" });
                    SaveUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuários: " + ex.Message);
                users = new List<User>();
            }
        }

        private static void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(UsersFilePath, json);
        }

        public static void Add(User user)
        {
            if (user.Id == 0)
            {
                user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            }
            users.Add(user);
            SaveUsers();
        }

        public static bool EmailExists(string email)
        {
            return users.Any(u => u.Email == email);
        }

        public static User Authenticate(string email, string password)
        {
            Console.WriteLine($"EMAIL = {email}, SENHA = {password}");
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public static List<User> GetAllUsers()
        {
            return users.ToList();
        }

        public static void Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SaveUsers();
            }
        }

        public static void Update(User updatedUser)
        {
            var index = users.FindIndex(u => u.Id == updatedUser.Id);
            if (index != -1)
            {
                users[index] = updatedUser;
                SaveUsers();
            }
        }
    }
}


