using System;
using Cats_Cafe_Accounting_System.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.RegularClasses
{
    public class Gender : ObservableObject
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
            DataRow row = DBContext.GetById("genders", id);
            Id = Convert.ToInt32(row["id"]);
            Title = row["title"].ToString();
        }

        public Gender() { }
        public static List<Gender> GetGendersFromTable()
        {
            List<Gender> genders = new List<Gender>();

            DataTable dataTable = DBContext.GetTable("genders");

            foreach (DataRow row in dataTable.Rows)
            {
                Gender gender = new Gender(Convert.ToInt32(row["id"]), row["title"].ToString());
                genders.Add(gender);
            }

            return genders;
        }
    }
}
