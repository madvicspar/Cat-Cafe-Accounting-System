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
            Visitors = new ObservableCollection<VisitorModel>();
            Visitors.Add(new VisitorModel { FirstName = "loh", LastName="lohov", Pathronymic = "lohovich", Gender = new Gender(0, "Мужской"), Birthday = new DateOnly(2023, 05, 23) });
            Visitors.Add(new VisitorModel { FirstName = "да", LastName = "дада", Pathronymic = "дадада", Gender = new Gender(1, "Женский"), Birthday = new DateOnly(2023, 02, 01)});
            Visitors.Add(new VisitorModel { FirstName = "чел", LastName = ", ", Pathronymic = "ты..", Gender = new Gender(0, "Мужской"), Birthday = new DateOnly(2023, 11, 19)});

        }
    }
}
