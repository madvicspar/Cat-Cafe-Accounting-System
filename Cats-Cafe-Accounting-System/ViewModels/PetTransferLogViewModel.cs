using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PetTransferLogViewModel : ObservableObject
    {
        private ObservableCollection<PetTransferLogEntryModel> petTransferLogEntries;
        public ObservableCollection<PetTransferLogEntryModel> PetTransferLogEntries
        {
            get { return petTransferLogEntries; }
            set
            {
                petTransferLogEntries = value;
                OnPropertyChanged(nameof(PetTransferLogEntries));
            }
        }
        public PetTransferLogViewModel()
        {
            // Инициализация коллекции питомцев
            PetTransferLogEntries = GetPetTransferLogEntriesFromTable("owner_log");
        }
        public static ObservableCollection<PetTransferLogEntryModel> GetPetTransferLogEntriesFromTable(string table)
        {
            ObservableCollection<PetTransferLogEntryModel> petTransferLogEntries = new ObservableCollection<PetTransferLogEntryModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                PetTransferLogEntryModel petTransferLogEntry = new PetTransferLogEntryModel(Convert.ToInt32(row["id_entry"]), DateTime.Parse(row["date"].ToString()),
                    Convert.ToInt32(row["visitor_id"]), Convert.ToInt32(row["pet_id"]),
                    new VisitorModel(Convert.ToInt32(row["visitor_id"])), new PetModel(Convert.ToInt32(row["pet_id"])));
                petTransferLogEntries.Add(petTransferLogEntry);
            }

            return petTransferLogEntries;
        }
    }
}
