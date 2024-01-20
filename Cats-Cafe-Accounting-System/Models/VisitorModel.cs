using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
using System.Xml.Linq;

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
        public VisitorModel(int id, string lastName, string firstName, string pathronymic, int genderId, string phone, DateTime birthday, Gender gender)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pathronymic = pathronymic;
            GenderId = genderId;
            Phone = phone;
            Birthday = birthday;
            Gender = gender;
        }
    }
}
