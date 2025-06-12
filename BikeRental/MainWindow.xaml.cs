using BikeRental.ViewModels;
using System.Windows;
using BikeRental.Models;
using BikeRentalDashboard.Models;

namespace BikeRental
{
    /// <summary>
    /// Interação lógica para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(User authenticatedUser)
        {
            InitializeComponent();
            DataContext = new MainViewModel(authenticatedUser);
        }
    }
}


