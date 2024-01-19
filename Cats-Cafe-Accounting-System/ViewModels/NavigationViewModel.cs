using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        public List<NavigationModel> NavigationOptions { get; set; } = new();

        public ICommand SelectionChangedCommand { get; set; } = new RelayCommand<object>((o) =>
        {
            if (o is SelectionChangedEventArgs selectionChanged)
            {
                if (selectionChanged.AddedItems.Count == 0)
                    return;
                if (selectionChanged.AddedItems[0] is NavigationModel navModel)
                {
                    var message = new NavigationChangedRequestMessage(navModel);
                    WeakReferenceMessenger.Default.Send(message);
                }
            }
        });
        public NavigationViewModel(
            AuthorizationViewModel loginView)
        {
            NavigationOptions.Add(new() { Name = "Login", Description = "User can login with this page. the demo shows how to request data from other ViewModels", DestinationVM = loginView });

            var message = new NavigationChangedRequestMessage(NavigationOptions[0]);
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}
