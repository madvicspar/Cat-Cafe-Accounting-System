using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class EmployeeShiftLogViewModel : ObservableObject
    {
        private ObservableCollection<EmployeeShiftLogEntryModel> employeeShiftLogEntries;
        public ObservableCollection<EmployeeShiftLogEntryModel> EmployeeShiftLogEntries
        {
            get { return employeeShiftLogEntries; }
            set
            {
                employeeShiftLogEntries = value;
                OnPropertyChanged(nameof(EmployeeShiftLogEntries));
            }
        }
        public EmployeeShiftLogViewModel()
        {

        }
    }
}
