using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Инициализация коллекции питомцев
            Employees = new ObservableCollection<EmployeeModel>();
            //Employees.Add(new EmployeeModel { FirstName = "loh", LastName = "lohov", Pathronymic = "lohovich", Gender = new Gender(0, "Мужской"), Birthday = new DateOnly(2023, 05, 23), Job = new JobModel { Title = "директор", Rate = 14.92f }, ContractNumber = "samayabadbitch #1" });
            //Employees.Add(new EmployeeModel { FirstName = "да", LastName = "дада", Pathronymic = "дадада", Gender = new Gender(1, "Женский"), Birthday = new DateOnly(2023, 02, 01), Job = new JobModel { Title = "менеджер", Rate = 9.87f }, ContractNumber = "456" });
            //Employees.Add(new EmployeeModel { FirstName = "чел", LastName = ", ", Pathronymic = "ты..", Gender = new Gender(0, "Мужской"), Birthday = new DateOnly(2023, 11, 19), Job = new JobModel{ Title = "уборщик", Rate = 4.56f }, ContractNumber = "123" });

        }
    }
}
