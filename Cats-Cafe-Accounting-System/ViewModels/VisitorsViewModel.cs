using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitorsViewModel : ObservableObject
    {
        private ObservableCollection<VisitorModel> visitors;
        public ObservableCollection<VisitorModel> Visitors
        {
            get { return visitors; }
            set
            {
                visitors = value;
                OnPropertyChanged(nameof(Visitors));
            }
        }
        public VisitorsViewModel()
        {

        }
    }
}
