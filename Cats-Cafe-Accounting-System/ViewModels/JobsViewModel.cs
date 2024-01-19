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
    public class JobsViewModel : ObservableObject
    {
        private ObservableCollection<JobModel> jobs;
        public ObservableCollection<JobModel> Jobs
        {
            get { return jobs; }
            set
            {
                jobs = value;
                OnPropertyChanged(nameof(Jobs));
            }
        }
        public JobsViewModel()
        {
            // Инициализация коллекции питомцев
            Jobs = new ObservableCollection<JobModel>();
            Jobs.Add(new JobModel { Title = "уборщик", Rate = 4.56f });
            Jobs.Add(new JobModel { Title = "менеджер",  Rate = 9.87f});
            Jobs.Add(new JobModel { Title = "директор", Rate = 14.92f });
        }
    }
}
