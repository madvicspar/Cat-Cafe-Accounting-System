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
            Jobs = GetJobsFromTable("jobs");
        }
        public static ObservableCollection<JobModel> GetJobsFromTable(string table)
        {
            ObservableCollection<JobModel> jobs = new ObservableCollection<JobModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                JobModel job = new JobModel(Convert.ToInt32(row["id"]), row["title"].ToString(),
                    (float)Convert.ToDouble(row["rate"]));
                jobs.Add(job);
            }

            return jobs;
        }
    }
}
