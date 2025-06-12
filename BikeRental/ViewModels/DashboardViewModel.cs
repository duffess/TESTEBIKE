using System.ComponentModel;
using BikeRental.Models;
using System.Collections.ObjectModel;
using BikeRental.Services;
using System.Windows.Input;
using System;
using System.Windows;
using BikeRentalDashboard.Models;
using System.Linq;

namespace BikeRental.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _welcomeMessage = "Bem-vindo ao Sistema de Aluguel de Bicicletas!";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set { _welcomeMessage = value; OnPropertyChanged(nameof(WelcomeMessage)); }
        }

        public ObservableCollection<Bike> AvailableBikes { get; set; }


        private Bike _selectedBike;
        public Bike SelectedBike
        {
            get => _selectedBike;
            set
            {
                _selectedBike = value;
                OnPropertyChanged(nameof(SelectedBike));
                RentBikeCommand.RaiseCanExecuteChanged();
                ReturnBikeCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand RentBikeCommand { get; private set; }
        public RelayCommand ReturnBikeCommand { get; private set; }


        private User _authenticatedUser;

        public DashboardViewModel(User authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            AvailableBikes = new ObservableCollection<Bike>(BikeService.GetAllBikes());

            RentBikeCommand = new RelayCommand(RentBike, CanRentBike);
            ReturnBikeCommand = new RelayCommand(ReturnBike, CanReturnBike);
        }

        private bool CanRentBike(object parameter)
        {
            return SelectedBike != null && SelectedBike.IsAvailable && _authenticatedUser?.Role == "Client";
        }

        private void RentBike(object parameter)
        {
            if (SelectedBike != null && SelectedBike.IsAvailable)
            {
                SelectedBike.IsAvailable = false;
                BikeService.UpdateBike(SelectedBike);

                Rental newRental = new Rental
                {
                    BikeId = SelectedBike.Id,
                    UserId = _authenticatedUser.Id,
                    RentalDate = DateTime.Now,
                    ReturnDate = null,
                    TotalCost = 0 
                };
                RentalService.AddRental(newRental);

                MessageBox.Show($"Bicicleta {SelectedBike.Model} alugada com sucesso por {_authenticatedUser.Username}!");
                RefreshBikes();
            }
        }

        private bool CanReturnBike(object parameter)
        {
            // Check if the selected bike is currently rented by the authenticated user
            // This logic needs to be more robust, checking actual rental records
            return SelectedBike != null && !SelectedBike.IsAvailable && _authenticatedUser?.Role == "Client";
        }

        private void ReturnBike(object parameter)
        {
            if (SelectedBike != null && !SelectedBike.IsAvailable)
            {
                SelectedBike.IsAvailable = true;
                BikeService.UpdateBike(SelectedBike);

                // Find the active rental for this bike and user
                Rental activeRental = RentalService.GetAllRentals()
                                                .FirstOrDefault(r => r.BikeId == SelectedBike.Id &&
                                                                     r.UserId == _authenticatedUser.Id &&
                                                                     r.ReturnDate == null);
                if (activeRental != null)
                {
                    activeRental.ReturnDate = DateTime.Now;
                    // Calculate total cost (example: based on hours rented)
                    TimeSpan rentalDuration = activeRental.ReturnDate.Value - activeRental.RentalDate;
                    activeRental.TotalCost = (decimal)rentalDuration.TotalHours * SelectedBike.PricePerHour;
                    RentalService.UpdateRental(activeRental);
                    MessageBox.Show($"Bicicleta {SelectedBike.Model} devolvida com sucesso! Custo total: {activeRental.TotalCost:C}");
                }
                else
                {
                    MessageBox.Show("Não foi possível encontrar um aluguel ativo para esta bicicleta.");
                }
                RefreshBikes();
            }
        }

        private void RefreshBikes()
        {
            AvailableBikes.Clear();
            foreach (var bike in BikeService.GetAllBikes())
            {
                AvailableBikes.Add(bike);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


