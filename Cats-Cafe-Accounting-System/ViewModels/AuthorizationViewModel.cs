﻿using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class AuthorizationViewModel : ObservableObject
    {
        ApplicationDbContext _dbContext; // = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        private string? userName;
        private SecureString password = new SecureString();
        private string usernameError = "";
        private string passwordError = "";
        public string? UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }
        public SecureString Password
        {
            get => password;
            set
            {

                SetProperty(ref password, value);
                OnPropertyChanged(nameof(Password));
            }
        }
        public string? UsernameError
        {
            get => usernameError;
            set
            {
                usernameError = value;
                OnPropertyChanged(nameof(UsernameError));
            }
        }
        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged(nameof(PasswordError));
            }
        }
        public ICommand SignInCommand { get; set; }

        public AuthorizationViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            SignInCommand = new RelayCommand(ExecuteSignInCommand);
        }

        private void ExecuteSignInCommand()
        {
            var user = _dbContext.Employees.FirstOrDefault(u => u.Username == UserName);
            if (user is not null)
            {
                if (AuthenticateUser(user))
                {
                    Data.user = user;
                    Data.IsDirector = Data.user.JobId == 1;
                    var authorizationWindow = Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault();
                    authorizationWindow?.Close();
                    UsernameError = "";
                    PasswordError = "";
                }
                else
                {
                    UsernameError = "";
                    PasswordError = "(invalid password)";
                }
            }
            else
            {
                UsernameError = "(invalid username)";
            }
        }

        private bool AuthenticateUser(EmployeeModel user)
        {
            byte[] enteredPasswordHash = HashingHelper.ComputeHash(Password, user.Salt);

            byte[] storedPasswordHash = HashingHelper.ComputeHash(user.Password, user.Salt);

            if (HashingHelper.CompareByteArrays(enteredPasswordHash, storedPasswordHash))
            {
                return true;
            }
            return false;
        }
    }
}
