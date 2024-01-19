using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Security;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class AuthorizationViewModel : ObservableObject
    {
        private string? userName;
        private SecureString password;
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
        public ICommand SignInCommand { get; set; }
        public IRelayCommand OpenMainWindowCommand { get; set; }

        public AuthorizationViewModel()
        {
            SignInCommand = new RelayCommand(ExecuteSignInCommand, CanExecuteSignInCommand);
            OpenMainWindowCommand = new RelayCommand(OpenMainWindow);
        }

        private void ExecuteSignInCommand()
        {
            // проверка данных
            OpenMainWindowCommand.Execute(null);
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

        private void OpenMainWindow()
        {
            WeakReferenceMessenger.Default.Send(new OpenMainWindowMessage());
        }
    }
}
