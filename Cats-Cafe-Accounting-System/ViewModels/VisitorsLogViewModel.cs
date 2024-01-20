using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitorsLogViewModel : ObservableObject
    {
        private ObservableCollection<VisitorLogEntryModel> visitorsLogEntries;
        public ObservableCollection<VisitorLogEntryModel> VisitorsLogEntries
        {
            get { return visitorsLogEntries; }
            set
            {
                visitorsLogEntries = value;
                OnPropertyChanged(nameof(VisitorsLogEntries));
            }
        }
        public VisitorsLogViewModel()
        {
            // Инициализация коллекции питомцев
            VisitorsLogEntries = new ObservableCollection<VisitorLogEntryModel>();
            VisitorsLogEntries.Add(new VisitorLogEntryModel { VisitorId = 1, TicketId = 2, TicketsCount = 1, Date = new DateOnly(2023, 05, 23), TimeStart = new TimeOnly(12,0,0) });
            VisitorsLogEntries.Add(new VisitorLogEntryModel { VisitorId = 3, TicketId = 8, TicketsCount = 1, Date = new DateOnly(2023, 02, 01), TimeStart = new TimeOnly(9, 07, 56) });
            VisitorsLogEntries.Add(new VisitorLogEntryModel { VisitorId = 4, TicketId = 1, TicketsCount = 2, Date = new DateOnly(2023, 11, 19), TimeStart = new TimeOnly(18, 59, 01) });

        }
    }
}
