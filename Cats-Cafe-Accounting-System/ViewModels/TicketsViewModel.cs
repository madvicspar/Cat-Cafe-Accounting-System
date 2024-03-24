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

        }
    }
}
