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
            
        }
    }
}
