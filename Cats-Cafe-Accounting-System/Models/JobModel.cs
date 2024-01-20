using Cats_Cafe_Accounting_System.RegularClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class JobModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rate { get; set; }
        public JobModel(string title, float rate)
        {
            Title = title;
            Rate = rate;
        }
        public JobModel(int id, string title, float rate)
        {
            Id = id;
            Title = title;
            Rate = rate;
        }
    }
}
