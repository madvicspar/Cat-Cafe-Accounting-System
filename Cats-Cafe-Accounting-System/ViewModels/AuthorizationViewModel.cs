using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Net;
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class AuthorizationViewModel : ObservableObject
    {
        private string? userName;
        private SecureString password;
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
            set => SetProperty(ref password, value);
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

        public AuthorizationViewModel()
        {
            SignInCommand = new RelayCommand(ExecuteSignInCommand);
        }

        private void ExecuteSignInCommand()
        {
            //Data.user = 
            //Data.breedsList = Breed.GetBreedsFromTable();
            //Data.gendersList = Gender.GetGendersFromTable();
            //Data.statusesList = Status.GetStatusesFromTable();
            //var authorizationWindow = Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault();
            //authorizationWindow?.Close();
            if (CanExecuteSignInCommand())
            {
                if (DBContext.AuthenticateUser(new NetworkCredential(UserName, Password)))
                {
                    Data.breedsList = Breed.GetBreedsFromTable();
                    Data.gendersList = Gender.GetGendersFromTable();
                    Data.statusesList = Status.GetStatusesFromTable();
                    var authorizationWindow = Application.Current.Windows.OfType<AuthorizationView>().FirstOrDefault();
                    authorizationWindow?.Close();
                    UsernameError = "";
                    PasswordError = "";
                }
                else
                {
                    if (DBContext.UsernameIsExist(new NetworkCredential(UserName, "")))
                    {
                        UsernameError = "";
                        PasswordError = "(invalid password)";
                    }
                    else
                    {
                        UsernameError = "(invalid username)";
                    }
                }
            }
        }

        private bool CanExecuteSignInCommand()
        {
            return true;
            //bool validData = true;
            //if (string.IsNullOrWhiteSpace(UserName) || UserName.Length < 3 ||
            //    Password == null || Password.Length < 3)
            //    validData = false;
            //return validData;
        }
    }
}
