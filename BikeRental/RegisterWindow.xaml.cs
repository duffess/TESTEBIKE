using System.Windows;
using BikeRental.Models;
using BikeRental.Services;
using BikeRentalDashboard.Models;

namespace BikeRental.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Registrar_Click(object sender, RoutedEventArgs e)
        {
            string nome = NomeBox.Text;
            string email = EmailBox.Text;
            string senha = SenhaBox.Password;

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("Todos os campos são obrigatórios.");
                return;
            }


            var novoUsuario = new User { Username = nome, Email = email, Password = senha };
            UserService.Add(novoUsuario);

            MessageBox.Show("Usuário registrado com sucesso!");

            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
