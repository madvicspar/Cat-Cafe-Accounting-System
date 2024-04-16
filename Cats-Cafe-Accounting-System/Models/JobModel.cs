using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class JobModel : ObservableObject, ICloneable, IEquatable<JobModel>
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rate { get; set; }
        public JobModel() { }

        public static JobModel Update(JobModel oldJob, JobModel newJob)
        {
            oldJob.Title = newJob.Title;
            oldJob.Rate = newJob.Rate;
            return oldJob;
        }

        public object Clone()
        {
            return new JobModel
            {
                Id = Id,
                Title = Title,
                Rate = Rate
            };
        }

        public bool Equals(JobModel? other)
        {
            return other?.Title == Title
                && other?.Rate == Rate;
        }
    }
}
