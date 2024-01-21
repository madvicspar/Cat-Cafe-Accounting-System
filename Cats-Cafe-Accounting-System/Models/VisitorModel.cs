using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class VisitorModel
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public VisitorModel(int id, string lastName, string firstName, string pathronymic, int genderId, string phone, DateTime birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pathronymic = pathronymic;
            GenderId = genderId;
            Phone = phone;
            Birthday = birthday;
            Gender = new Gender(GenderId);
        }
        public VisitorModel(int id)
        {
            DataRow row = DBContext.GetById("visitors", id);
            Id = Convert.ToInt32(row["id"]);
            FirstName = row["first_name"].ToString();
            LastName = row["last_name"].ToString();
            Pathronymic = row["pathronymic"].ToString();
            GenderId = Convert.ToInt32(row["gender_id"]);
            Phone = row["phone_number"].ToString();
            Birthday = DateTime.Parse(row["birthday"].ToString());
            Gender = new Gender(GenderId);
        }
    }
}
