using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableObject? NavigationVM { get; set; }
        private ObservableObject? currentVM;
        ApplicationDbContext _dbContext { get; set; }
        public ObservableObject? CurrentVM
        {
            get => currentVM;
            set => SetProperty(ref currentVM, value);
        }
        public ICommand SignOutCommand { get; set; }
        public MainWindowViewModel(NavigationViewModel navVM, ApplicationDbContext context)
        {
            _dbContext = context;
            NavigationVM = navVM;
            CurrentVM = new PetsViewModel(_dbContext);
            WeakReferenceMessenger.Default.Register<NavigationChangedRequestMessage>(this, NavigateTo);
            SignOutCommand = new RelayCommand(ExecuteSignOutCommand);
        }

        private void NavigateTo(object recipient, NavigationChangedRequestMessage message)
        {
            if (message.Value is NavigationModel navModel)
                CurrentVM = navModel.DestinationVM;
        }

        public void ExecuteSignOutCommand()
        {
            Data.user = null;
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).Close();
            App.InitializeApplication();
        }
    }
}
