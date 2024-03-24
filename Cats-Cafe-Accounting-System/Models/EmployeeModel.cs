using Cats_Cafe_Accounting_System.RegularClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeModel : ObservableObject
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
        public DateTime Birthday { get; set; }
        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual JobModel Job { get; set; }
        public string ContractNumber { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }

        public EmployeeModel()
        {
            Gender = new Gender();
            Job = new JobModel();
        }
    }
}
