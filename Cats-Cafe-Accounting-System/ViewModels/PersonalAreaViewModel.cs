using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PersonalAreaViewModel : ObservableObject
    {
        public EmployeeModel employee {  get; set; }
        public PersonalAreaViewModel()
        {
            employee = Data.user;
        }
    }
}
