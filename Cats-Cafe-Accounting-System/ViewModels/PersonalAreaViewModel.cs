using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PersonalAreaViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        private EmployeeModel employee;
        private static readonly string path = "../Images/UsersPhoto/";
        private string photoAddress;

        public string PhotoAddress
        {
            get { return photoAddress; }
            set { SetProperty(ref photoAddress, path + value); }
        }

        public EmployeeModel Employee
        {
            get { return employee; }
            set
            {
                SetProperty(ref employee, value);
                OnPropertyChanged(nameof(Employee));
                UpdatePhotoAddress();
            }
        }
        public ICommand UpdateEmployeeCommand { get; set; }
        public PersonalAreaViewModel()
        {
            Employee = null;
            PhotoAddress = "cat_default_icon.jpg";
            UpdateEmployeeCommand = new RelayCommand(ExecuteUpdateEmployeeCommand);
        }

        private void UpdatePhotoAddress()
        {
            if (Employee != null && !string.IsNullOrEmpty(Employee.PhotoAddress))
            {
                PhotoAddress = Employee.PhotoAddress;
            }
            else
            {
                PhotoAddress = "cat_default_icon.jpg";
            }
        }
        public void ExecuteUpdateEmployeeCommand()
        {
            _dbContext.Employees.Update(Employee);
            _dbContext.SaveChanges();
        }
    }
}