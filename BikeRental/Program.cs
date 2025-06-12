//using System;
//using BikeRental.ViewModels;
//using BikeRental.Models;

//namespace BikeRental
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            // LOGIN TEST
//            var loginVM = new LoginViewModel();
//            loginVM.Email = "admin@bikerental.com";
//            loginVM.Password = "1234";

//            bool loginResult = loginVM.LoginCommand.CanExecute(null);
//            Console.WriteLine($"Login successful? {loginResult}");

//            // BIKE TEST
//            var bikeVM = new BikeManagementViewModel();
//            bikeVM.NewBike.Model = "Mountain Bike";
//            bikeVM.NewBike.PricePerHour = 10.5m;
//            bikeVM.NewBike.IsAvailable = true;

//            bikeVM.AddBikeCommand.Execute(null);
//            Console.WriteLine($"Bikes: {bikeVM.Bikes.Count}");

//            var bike = bikeVM.Bikes[0];
//            bikeVM.SelectedBike = bike;
//            bikeVM.AlterarStatusCommand.Execute(null);
//            Console.WriteLine($"Bike status: {bike.Status}");

//            // USER TEST
//            var userVM = new UserManagementViewModel();
//            userVM.NewUser.Username = "John Doe";
//            userVM.NewUser.Email = "john@bikerental.com";
//            userVM.NewUser.Profile = "Admin";

//            userVM.AddUserCommand.Execute(null);
//            Console.WriteLine($"Users: {userVM.Users.Count}");

//            var user = userVM.Users[0];
//            userVM.SelectedUser = user;
//            userVM.DeleteUserCommand.Execute(null);
//            Console.WriteLine($"Users after delete: {userVM.Users.Count}");

//            Console.ReadKey();
//        }
//    }
//}
