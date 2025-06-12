using System.Windows;
using BikeRental.ViewModels;
using BikeRental.Models;

namespace BikeRental.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
                vm.LoginCommand.Execute(null);
            }
        }


        private void AbrirRegistro_Click(object sender, RoutedEventArgs e)
        {
            var register = new RegisterWindow(); // Assuming RegisterWindow exists
            register.Show();
            this.Close();
        }
    }
}


