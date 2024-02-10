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
            // Инициализация коллекции питомцев
            Visitors = GetVisitorsFromTable("visitors");
        }
        public static ObservableCollection<VisitorModel> GetVisitorsFromTable(string table)
        {
            ObservableCollection<VisitorModel> visitors = new ObservableCollection<VisitorModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                VisitorModel visitor = new VisitorModel(Convert.ToInt32(row["id"]), row["lastname"].ToString(), 
                    row["firstname"].ToString(), row["pathronymic"].ToString(),Convert.ToInt32(row["genderid"]),
                    row["phonenumber"].ToString(), DateTime.Parse(row["birthday"].ToString()));
                visitors.Add(visitor);
            }

            return visitors;
        }
    }
}
