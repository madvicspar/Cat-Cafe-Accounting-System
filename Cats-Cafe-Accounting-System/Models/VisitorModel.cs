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
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly Birthday { get; set; }
        public VisitorModel(int id, string lastName, string firstName, string pathronymic, int genderId, string phone, DateOnly birthday)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pathronymic = pathronymic;
            GenderId = genderId;
            PhoneNumber = phone;
            Birthday = birthday;
            Gender = new Gender(GenderId);
        }
        public VisitorModel(int id)
        {
            DataRow row = DBContext.GetById("visitors", id);
            Id = Convert.ToInt32(row["id"]);
            FirstName = row["firstname"].ToString();
            LastName = row["lastname"].ToString();
            Pathronymic = row["pathronymic"].ToString();
            GenderId = Convert.ToInt32(row["genderid"]);
            PhoneNumber = row["phonenumber"].ToString();
            Birthday = DateOnly.Parse(row["birthday"].ToString());
            Gender = new Gender(GenderId);
        }
        public VisitorModel()
        {
            Gender = new Gender();
        }
    }
}
