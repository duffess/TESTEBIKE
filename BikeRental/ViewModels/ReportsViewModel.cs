using System.ComponentModel;
using BikeRental.Models;
using BikeRental.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Windows.Input;

namespace BikeRental.ViewModels
{
    public class ReportsViewModel : INotifyPropertyChanged
    {
        private string _reportData;
        public string ReportData
        {
            get => _reportData;
            set { _reportData = value; OnPropertyChanged(nameof(ReportData)); }
        }

        private User _authenticatedUser;

        // Relatórios
        public ObservableCollection<Rental> AllRentals { get; set; }
        public ObservableCollection<Bike> AllBikes { get; set; }
        public ObservableCollection<User> AllUsers { get; set; }

        public int RentalsToday { get; set; }
        public int RentalsThisWeek { get; set; }
        public int RentalsThisMonth { get; set; }
        public ObservableCollection<Tuple<string, int>> TopBikes { get; set; }
        public ObservableCollection<Tuple<string, int>> TopUsers { get; set; }
        public ObservableCollection<Rental> CurrentRentals { get; set; }
        public decimal TotalRevenue { get; set; }

        public ICommand GenerateReportsCommand { get; private set; }

        public ReportsViewModel(User authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            AllRentals = new ObservableCollection<Rental>();
            AllBikes = new ObservableCollection<Bike>();
            AllUsers = new ObservableCollection<User>();
            TopBikes = new ObservableCollection<Tuple<string, int>>();
            TopUsers = new ObservableCollection<Tuple<string, int>>();
            CurrentRentals = new ObservableCollection<Rental>();

            GenerateReportsCommand = new RelayCommand(GenerateReports, param => IsAdmin());

            if (IsAdmin())
            {
                GenerateReports();
            }
            else
            {
                ReportData = "Acesso negado. Apenas administradores podem visualizar relatórios.";
            }
        }

        private bool IsAdmin()
        {
            return _authenticatedUser?.Role == "Administrator";
        }

        private void GenerateReports(object parameter = null)
        {
            AllRentals = new ObservableCollection<Rental>(RentalService.GetAllRentals());
            AllBikes = new ObservableCollection<Bike>(BikeService.GetAllBikes());
            AllUsers = new ObservableCollection<User>(UserService.GetAllUsers());

            // Quantidade de aluguéis por dia, semana ou mês
            RentalsToday = AllRentals.Count(r => r.RentalDate.Date == DateTime.Today);
            RentalsThisWeek = AllRentals.Count(r => r.RentalDate >= GetStartOfWeek(DateTime.Today) && r.RentalDate <= DateTime.Today);
            RentalsThisMonth = AllRentals.Count(r => r.RentalDate.Month == DateTime.Today.Month && r.RentalDate.Year == DateTime.Today.Year);

            // Bicicletas mais alugadas
            TopBikes.Clear();
            AllRentals.GroupBy(r => r.BikeId)
                      .Select(g => new { BikeId = g.Key, Count = g.Count() })
                      .OrderByDescending(x => x.Count)
                      .Take(5)
                      .ToList()
                      .ForEach(x => TopBikes.Add(new Tuple<string, int>(AllBikes.FirstOrDefault(b => b.Id == x.BikeId)?.Model ?? "Desconhecida", x.Count)));

            // Usuários que mais alugaram
            TopUsers.Clear();
            AllRentals.GroupBy(r => r.UserId)
                      .Select(g => new { UserId = g.Key, Count = g.Count() })
                      .OrderByDescending(x => x.Count)
                      .Take(5)
                      .ToList()
                      .ForEach(x => TopUsers.Add(new Tuple<string, int>(AllUsers.FirstOrDefault(u => u.Id == x.UserId)?.Username ?? "Desconhecido", x.Count)));

            // Bicicletas atualmente alugadas
            CurrentRentals.Clear();
            AllRentals.Where(r => r.ReturnDate == null).ToList().ForEach(r => CurrentRentals.Add(r));

            // Receita total gerada no período
            TotalRevenue = AllRentals.Where(r => r.ReturnDate != null).Sum(r => r.TotalCost);

            OnPropertyChanged(nameof(RentalsToday));
            OnPropertyChanged(nameof(RentalsThisWeek));
            OnPropertyChanged(nameof(RentalsThisMonth));
            OnPropertyChanged(nameof(TopBikes));
            OnPropertyChanged(nameof(TopUsers));
            OnPropertyChanged(nameof(CurrentRentals));
            OnPropertyChanged(nameof(TotalRevenue));
        }

        private DateTime GetStartOfWeek(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            int days = day - DayOfWeek.Monday;
            if (days < 0) days += 7;
            return date.AddDays(-days).Date;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}


