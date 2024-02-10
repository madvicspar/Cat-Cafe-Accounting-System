using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public JobModel Job { get; set; }
        public string ContractNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public EmployeeModel(int id, string lastName, string firstName, string pathronymic, int genderId, string phone, DateTime birthday, int jobId, string contractNumber, string username)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pathronymic = pathronymic;
            GenderId = genderId;
            PhoneNumber = phone;
            Birthday = birthday;
            Gender = new Gender(GenderId);
            JobId = jobId;
            ContractNumber = contractNumber;
            Username = username;
            Job = new JobModel(JobId);
        }
        public EmployeeModel(int id)
        {
            DataRow row = DBContext.GetById("employees", id);
            Id = Convert.ToInt32(row[nameof(Id)]);
            FirstName = row[nameof(FirstName)].ToString();
            LastName = row[nameof(LastName)].ToString();
            Pathronymic = row[nameof(Pathronymic)].ToString();
            GenderId = Convert.ToInt32(row[nameof(GenderId)]);
            PhoneNumber = row[nameof(PhoneNumber)].ToString();
            Birthday = DateTime.Parse(row[nameof(Birthday)].ToString());
            JobId = Convert.ToInt32(row[nameof(JobId)]); ;
            ContractNumber = row[nameof(ContractNumber)].ToString();
            Username = row[nameof(Username)].ToString();
            Gender = new Gender(GenderId);
            Job = new JobModel(JobId);
        }
    }
}
