using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cats_Cafe_Accounting_System.Models;
using System.Windows.Controls;
using DocumentFormat.OpenXml.ExtendedProperties;
using Cats_Cafe_Accounting_System.Utilities;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private bool isDirector;
        public bool IsDirector
        {
            get { return isDirector; }
            set
            {
                isDirector = value;
                OnPropertyChanged(nameof(IsDirector));
            }
        }
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
            VisitorsViewModel visitorsViewModel,
            PetsViewModel petsViewModel,
            JobsViewModel jobsViewModel,
            EmployeesViewModel employeesViewModel,
            TicketsViewModel ticketsViewModel,
            PetTransferLogViewModel petTransferLogViewModel,
            EmployeeShiftLogViewModel employeeShiftLogViewModel,
            VisitLogViewModel visitorsLogViewModel,
            PersonalAreaViewModel personalAreaViewModel,
            IncomesViewModel incomesViewModel,
            PopularPetsViewModel popularCatsViewModel)
        {
            NavigationOptions.Add(new() { Name = "Личный кабинет", Description = "", DestinationVM = personalAreaViewModel });
            NavigationOptions.Add(new() { Name = "Питомцы", Description = "", DestinationVM = petsViewModel });
            NavigationOptions.Add(new() { Name = "Посетители", Description = "", DestinationVM = visitorsViewModel });
            NavigationOptions.Add(new() { Name = "Должности", Description = "", DestinationVM = jobsViewModel });
            NavigationOptions.Add(new() { Name = "Сотрудники", Description = "", DestinationVM = employeesViewModel });
            NavigationOptions.Add(new() { Name = "Билеты", Description = "", DestinationVM = ticketsViewModel });
            NavigationOptions.Add(new() { Name = "Учета посещений", Description = "", DestinationVM = visitorsLogViewModel });
            NavigationOptions.Add(new() { Name = "Учета смен сотрудников", Description = "", DestinationVM = employeeShiftLogViewModel });
            NavigationOptions.Add(new() { Name = "Учета передач питомцев", Description = "", DestinationVM = petTransferLogViewModel });
            NavigationOptions.Add(new() { Name = "Доходы", Description = "", DestinationVM = incomesViewModel });
            NavigationOptions.Add(new() { Name = "Популярные питомцы", Description = "", DestinationVM = popularCatsViewModel });

            var message = new NavigationChangedRequestMessage(NavigationOptions[0]);
            WeakReferenceMessenger.Default.Send(message);
        }
    }
}
