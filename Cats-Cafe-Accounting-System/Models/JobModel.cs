using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class JobModel : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public float Rate { get; set; }
        public JobModel(int id)
        {
            DataRow row = DBContext.GetById("jobs", id);
            Id = Convert.ToInt32(row["id"]);
            Title = row["title"].ToString();
            Rate = (float)Convert.ToDouble(row["rate"]);
        }
        public JobModel()
        {

        }
    }
}
