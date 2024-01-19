using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;

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
    }
}
