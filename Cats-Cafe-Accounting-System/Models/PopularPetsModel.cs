using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PopularPetsModel : ObservableObject
    {
        public int Place { get; set; }
        public int Score { get; set; }
        public PetModel Pet { get; set; }
        public List<VisitLogEntryModel> PetTransferLogEntryModels { get; set; }

        [ExcludeFromCodeCoverage]
        public PopularPetsModel()
        {
            Pet = new PetModel();
            PetTransferLogEntryModels = new List<VisitLogEntryModel>();
        }
    }
}
