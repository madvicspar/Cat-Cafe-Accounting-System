using Cats_Cafe_Accounting_System.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
        public JobModel(int id)
        {
            DataRow row = DBContext.GetById("job", id);
            Id = Convert.ToInt32(row["id"]);
            Title = row["title"].ToString();
            Rate = (float)Convert.ToDouble(row["rate"]);
        }
    }
}
