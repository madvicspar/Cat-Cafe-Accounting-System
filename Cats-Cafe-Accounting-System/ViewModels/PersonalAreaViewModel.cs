using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PersonalAreaViewModel : ObservableObject
    {
        public static EmployeeModel employee = Data.user;
        public EmployeeModel Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }
        public PersonalAreaViewModel()
        {

        }
    }
}
