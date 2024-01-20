using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security;
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public DateOnly Birthday { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public JobModel Job { get; set; }
        public string ContractNumber { get; set; }
        public SecureString Pass { get; set; }
        public string Salt { get; set; }
    }
}
