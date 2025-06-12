using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BikeRental.Models;
using BikeRental.Services;
using System.Linq;
using System;
using BikeRentalDashboard.Models;
using BikeRentalDashboard.ViewModels;

namespace BikeRental.ViewModels
{
    public class UserManagementViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public User SelectedUser { get; set; }
        public User NewUser { get; set; }

        public RelayCommand AddUserCommand { get; }
        public RelayCommand EditUserCommand { get; }
        public RelayCommand DeleteUserCommand { get; }

        private User _authenticatedUser;

        public UserManagementViewModel(User authenticatedUser)
        {
            _authenticatedUser = authenticatedUser;
            Users = new ObservableCollection<User>(UserService.GetAllUsers());
            NewUser = new User();

            AddUserCommand = new RelayCommand(param => AddUser(), param => IsAdmin());
            EditUserCommand = new RelayCommand(param => EditUser(), param => SelectedUser != null && IsAdmin());
            DeleteUserCommand = new RelayCommand(param => DeleteUser(), param => SelectedUser != null && IsAdmin());
        }

        private bool IsAdmin()
        {
            return _authenticatedUser?.Role == "Administrator";
        }

        private void AddUser()
        {
            if (NewUser != null && !string.IsNullOrEmpty(NewUser.Username) && !string.IsNullOrEmpty(NewUser.Email))
            {
                UserService.Add(NewUser);
                Users.Add(NewUser);
                NewUser = new User(); // Clear form
                OnPropertyChanged(nameof(NewUser));
            }
        }

        private new void    OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        private void EditUser()
        {
            if (SelectedUser != null)
            {
                UserService.Update(SelectedUser);
                OnPropertyChanged(nameof(Users)); // Notify UI of changes
            }
        }

        private void DeleteUser()
        {
            if (SelectedUser != null)
            {
                UserService.Delete(SelectedUser.Id);
                Users.Remove(SelectedUser);
            }
        }
    }
}


