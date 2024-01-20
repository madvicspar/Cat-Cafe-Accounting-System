using System;
using Cats_Cafe_Accounting_System.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public Gender(int _id, string _title)
        {
            Id = _id;
            Title = _title;
        }
        public Gender(int id)
        {
            DataRow row = DBContext.GetById("gender", id);
            Id = Convert.ToInt32(row["id"]);
            Title = row["title"].ToString();
        }
    }
}
