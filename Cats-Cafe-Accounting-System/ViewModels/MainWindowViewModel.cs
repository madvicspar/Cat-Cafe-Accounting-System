using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Cats_Cafe_Accounting_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableObject? NavigationVM { get; set; }
        private ObservableObject? currentVM;
        public ObservableObject? CurrentVM
        {
            get => currentVM;
            set => SetProperty(ref currentVM, value);
        }
        public MainWindowViewModel(NavigationViewModel navVM)
        {
            WeakReferenceMessenger.Default.Register<OpenMainWindowMessage>(this, OpenMainWindow);
            NavigationVM = navVM;
            WeakReferenceMessenger.Default.Register<NavigationChangedRequestMessage>(this, NavigateTo);
        }

        private void NavigateTo(object recipient, NavigationChangedRequestMessage message)
        {
            if (message.Value is NavigationModel navModel)
                CurrentVM = navModel.DestinationVM;
        }
        private void OpenMainWindow(object recipient, OpenMainWindowMessage message)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
    public class OpenMainWindowMessage
    {

    }
}
