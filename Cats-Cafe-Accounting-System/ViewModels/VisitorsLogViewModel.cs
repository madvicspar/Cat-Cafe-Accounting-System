using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

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
            VisitorsLogEntries = GetVisitorsLogEntriesFromTable("visitor_log");
        }
        public static ObservableCollection<VisitorLogEntryModel> GetVisitorsLogEntriesFromTable(string table)
        {
            ObservableCollection<VisitorLogEntryModel> visitorsLogEntries = new ObservableCollection<VisitorLogEntryModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                VisitorLogEntryModel visitorsLogEntry = new VisitorLogEntryModel(Convert.ToInt32(row["id_entry"]), DateTime.Parse(row["date"].ToString()),
                    DateTime.Parse(row["start_time"].ToString()), DateTime.Parse(row["end_time"].ToString()),
                    Convert.ToInt32(row["visitor_id"]), Convert.ToInt32(row["ticket_id"]),
                    Convert.ToInt32(row["tickets_count"]), new VisitorModel(Convert.ToInt32(row["visitor_id"])), new TicketModel(Convert.ToInt32(row["ticket_id"])));
                visitorsLogEntries.Add(visitorsLogEntry);
            }

            return visitorsLogEntries;
        }
    }
}
