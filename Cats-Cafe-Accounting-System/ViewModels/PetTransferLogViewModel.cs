using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

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
            PetTransferLogEntries = new ObservableCollection<PetTransferLogEntryModel>();
            PetTransferLogEntries.Add(new PetTransferLogEntryModel { VisitorId = 1, PetId = 2, Date = new DateOnly(2023, 05, 23) });
            PetTransferLogEntries.Add(new PetTransferLogEntryModel { VisitorId = 3, PetId = 8, Date = new DateOnly(2023, 02, 01) });
            PetTransferLogEntries.Add(new PetTransferLogEntryModel { VisitorId = 4, PetId = 1, Date = new DateOnly(2023, 11, 19) });

        }
    }
}
