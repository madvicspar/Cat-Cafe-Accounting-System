using Cats_Cafe_Accounting_System.RegularClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PopularPetsModel : ObservableObject
    {
        public int Place { get; set; }
        public int Score { get; set; }
        public PetModel Pet { get; set; }
        public List<VisitLogEntryModel> PetTransferLogEntryModels { get; set; }
        public PopularPetsModel()
        {
            Pet = new PetModel();
            PetTransferLogEntryModels = new List<VisitLogEntryModel>();
        }
    }
}
