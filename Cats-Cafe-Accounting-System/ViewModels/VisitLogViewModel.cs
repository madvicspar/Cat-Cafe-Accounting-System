using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitLogViewModel : ObservableObject
    {
        private ObservableCollection<VisitLogEntryModel> visitorsLogEntries;
        public ObservableCollection<VisitLogEntryModel> VisitorsLogEntries
        {
            get { return visitorsLogEntries; }
            set
            {
                visitorsLogEntries = value;
                OnPropertyChanged(nameof(VisitorsLogEntries));
            }
        }
        public VisitLogViewModel()
        {
            // Инициализация коллекции питомцев
            VisitorsLogEntries = GetVisitorsLogEntriesFromTable("visit_log_entries");
        }
        public static ObservableCollection<VisitLogEntryModel> GetVisitorsLogEntriesFromTable(string table)
        {
            ObservableCollection<VisitLogEntryModel> visitorsLogEntries = new ObservableCollection<VisitLogEntryModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                VisitLogEntryModel visitorsLogEntry = new VisitLogEntryModel(Convert.ToInt32(row["id"]), DateTime.Parse(row["date"].ToString()),
                    DateTime.Parse(row["starttime"].ToString()),
                    Convert.ToInt32(row["visitorid"]), Convert.ToInt32(row["ticketid"]),Convert.ToInt32(row["ticketscount"]));
                visitorsLogEntries.Add(visitorsLogEntry);
            }

            return visitorsLogEntries;
        }
    }
}
