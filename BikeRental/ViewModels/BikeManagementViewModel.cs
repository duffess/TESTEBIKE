using BikeRental.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using BikeRental.Services;
using BikeRentalDashboard.Models;

namespace BikeRental.ViewModels
{
    public class BikeManagementViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Bike> Bikes { get; set; }
        public Bike NewBike { get; set; }
        private Bike _selectedBike;
        public Bike SelectedBike
        {
            get => _selectedBike;
            set
            {
                _selectedBike = value;
                OnPropertyChanged();
                AddBikeCommand.RaiseCanExecuteChanged();
                EditBikeCommand.RaiseCanExecuteChanged();
                DeleteBikeCommand.RaiseCanExecuteChanged();
                AlterarStatusCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddBikeCommand { get; }
        public RelayCommand EditBikeCommand { get; }
        public RelayCommand DeleteBikeCommand { get; }
        public RelayCommand AlterarStatusCommand { get; }

        private User _authenticatedUser;

        public BikeManagementViewModel(User authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            Bikes = new ObservableCollection<Bike>(BikeService.GetAllBikes());
            NewBike = new Bike();

            AddBikeCommand = new RelayCommand(param => AddBike(), param => IsAdmin());
            EditBikeCommand = new RelayCommand(param => EditBike(), param => SelectedBike != null && IsAdmin());
            DeleteBikeCommand = new RelayCommand(param => DeleteBike(), param => SelectedBike != null && IsAdmin());
            AlterarStatusCommand = new RelayCommand(param => AlterarStatus(), param => SelectedBike != null && IsAdmin());
        }

        private bool IsAdmin()
        {
            return _authenticatedUser?.Role == "Administrator";
        }

        private void AddBike()
        {
            try
            {
                BikeService.AddBike(NewBike);
                Bikes.Add(NewBike);
                NewBike = new Bike();
                OnPropertyChanged(nameof(NewBike));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar bike: {ex.Message}");
            }
        }

        private void EditBike()
        {
            if (SelectedBike == null) return;

            BikeService.UpdateBike(SelectedBike);
            OnPropertyChanged(nameof(Bikes));
        }

        private void DeleteBike()
        {
            if (SelectedBike != null)
            {
                BikeService.DeleteBike(SelectedBike.Id);
                Bikes.Remove(SelectedBike);
            }
        }

        private void AlterarStatus()
        {
            if (SelectedBike != null)
            {
                SelectedBike.IsAvailable = !SelectedBike.IsAvailable;
                BikeService.UpdateBike(SelectedBike);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


