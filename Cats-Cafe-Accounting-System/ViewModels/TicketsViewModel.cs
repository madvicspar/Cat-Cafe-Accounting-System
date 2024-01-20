using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class TicketsViewModel : ObservableObject
    {
        private ObservableCollection<TicketModel> tickets;
        public ObservableCollection<TicketModel> Tickets
        {
            get { return tickets; }
            set
            {
                tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }
        public TicketsViewModel()
        {
            // Инициализация коллекции питомцев
            Tickets = new ObservableCollection<TicketModel>();
            Tickets.Add(new TicketModel { Comments = "уборщик", Price = 4.56f, PetId=1 });
            Tickets.Add(new TicketModel { Comments = "менеджер", Price = 9.87f, PetId=2 });
            Tickets.Add(new TicketModel { Comments = "директор", Price = 14.92f });
        }
    }
}
