using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            Employees = GetEmployeesFromTable("employees");
        }
        public static ObservableCollection<EmployeeModel> GetEmployeesFromTable(string table)
        {
            ObservableCollection<EmployeeModel> emplyees = new ObservableCollection<EmployeeModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                EmployeeModel employee = new EmployeeModel(Convert.ToInt32(row["id"]), row["lastname"].ToString(), 
                    row["firstname"].ToString(), row["pathronymic"].ToString(), Convert.ToInt32(row["genderid"]),
                    row["phonenumber"].ToString(), DateTime.Parse(row["birthday"].ToString()),
                    Convert.ToInt32(row["jobid"]), row["contractnumber"].ToString(), row["username"].ToString());
                emplyees.Add(employee);
            }

            return emplyees;
        }
    }
}
