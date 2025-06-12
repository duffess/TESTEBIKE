using System;
using System.ComponentModel;
using System.Windows.Input;
using BikeRental.Models;
using BikeRentalDashboard.Models;

namespace BikeRental.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentView;
        private User authenticatedUser;

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
        }

        public User AuthenticatedUser { get; private set; }
        public bool IsAdmin => AuthenticatedUser?.Role == "Administrator";
        public bool IsClient => AuthenticatedUser?.Role == "Client";

        public LoginViewModel LoginVM { get; }
        public DashboardViewModel DashboardVM { get; }
        public UserManagementViewModel UserManagementVM { get; }
        public BikeManagementViewModel BikeManagementVM { get; }
        public ReportsViewModel ReportsVM { get; }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowBikesCommand { get; }
        public ICommand ShowReportsCommand { get; }

        public MainViewModel(User authenticatedUser)
        {
            AuthenticatedUser = authenticatedUser;

            LoginVM = new LoginViewModel(); 
            DashboardVM = new DashboardViewModel(authenticatedUser);
            UserManagementVM = new UserManagementViewModel(authenticatedUser);
            BikeManagementVM = new BikeManagementViewModel(authenticatedUser);
            ReportsVM = new ReportsViewModel(authenticatedUser);

            ShowDashboardCommand = new RelayCommand(o => CurrentView = DashboardVM);
            ShowUsersCommand = new RelayCommand(o => CurrentView = UserManagementVM, o => IsAdmin);
            ShowBikesCommand = new RelayCommand(o => CurrentView = BikeManagementVM, o => IsAdmin);
            ShowReportsCommand = new RelayCommand(o => CurrentView = ReportsVM, o => IsAdmin);

            CurrentView = DashboardVM;
            this.authenticatedUser = authenticatedUser;

        }

        //public MainViewModel(User authenticatedUser)
        //{
        //    this.authenticatedUser = authenticatedUser;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


