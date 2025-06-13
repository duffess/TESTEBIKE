using BikeRental.ViewModels;
using BikeRental.Models;
using System.Windows;

namespace BikeRental
{
    public partial class MainWindow : Window
    {
        public MainWindow(User authenticatedUser)
        {
            InitializeComponent();

            this.DataContext = new MainViewModel(authenticatedUser);
        }
    }
}