using System.Windows.Input;
using System.ComponentModel;
using System;
using BikeRental.Models;
using BikeRental.Services;
using System.Windows;
using BikeRentalDashboard.ViewModels;
using BikeRentalDashboard.Models;

namespace BikeRental.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public void Login(object parameter)
        {
            User authenticatedUser = UserService.Authenticate(Email, Password);

            if (authenticatedUser != null)
            {
                // Login bem-sucedido
                MessageBox.Show($"Bem-vindo, {authenticatedUser.Username}! Seu perfil é: {authenticatedUser.Role}");
                
                // Abre a janela principal e passa o usuário autenticado
                MainWindow main = new MainWindow(authenticatedUser);
                main.Show();

                // Fecha a janela de login
                Application.Current.Windows[0].Close();
            }
            else
            {
                MessageBox.Show("Email ou senha inválidos.");
            }
        }
    }
}


