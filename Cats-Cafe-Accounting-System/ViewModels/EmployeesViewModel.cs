using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class EmployeesViewModel : ObservableObject
    {
        private ObservableCollection<EmployeeModel> employees;
        public ObservableCollection<EmployeeModel> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }
        public EmployeesViewModel()
        {

        }
    }
}
