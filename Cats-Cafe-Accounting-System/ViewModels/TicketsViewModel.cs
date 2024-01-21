using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

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
            Tickets = GetTicketsFromTable("ticket");
        }
        public static ObservableCollection<TicketModel> GetTicketsFromTable(string table)
        {
            ObservableCollection<TicketModel> tickets = new ObservableCollection<TicketModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                TicketModel ticket = new TicketModel(Convert.ToInt32(row["id"]), (float)Convert.ToDouble(row["price"]),
                    Convert.ToInt32(row["pet_id"]), row["comments"].ToString());
                tickets.Add(ticket);
            }

            return tickets;
        }
    }
}
