using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public static class Data
    {
        public static EmployeeModel user = new EmployeeModel(Convert.ToInt32(DBContext.GetById("employees", 6.ToString())["Id"]));
        public static List<Gender> gendersList = new List<Gender>();
        public static List<Breed> breedsList = new List<Breed>();
        public static List<Status> statusesList = new List<Status>();
        public static DateTime currentDate = DateTime.Now;
    }
}
