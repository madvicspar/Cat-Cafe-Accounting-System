using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }
        //public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public JobModel Job { get; set; }
        public string ContractNumber { get; set; }
        public SecureString Pass { get; set; }
        public string Salt { get; set; }
        public EmployeeModel(int id, string lastName, string firstName, string pathronymic, int genderId, string phone, DateTime birthday, int jobId, string contractNumber, Gender gender, JobModel job)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Pathronymic = pathronymic;
            GenderId = genderId;
            Phone = phone;
            Birthday = birthday;
            Gender = gender;
            JobId = jobId;
            ContractNumber = contractNumber;
            Job = job;
        }
        public EmployeeModel(int id)
        {
            DataRow row = DBContext.GetById("employee", id);
            Id = Convert.ToInt32(row["id"]);
            FirstName = row["first_name"].ToString();
            LastName = row["last_name"].ToString();
            Pathronymic = row["pathronymic"].ToString();
            GenderId = Convert.ToInt32(row["gender_id"]);
            Phone = row["phone_number"].ToString();
            Birthday = DateTime.Parse(row["birthday"].ToString());
            JobId = Convert.ToInt32(row["job_id"]); ;
            ContractNumber = row["contract_number"].ToString();
            Gender = new Gender(GenderId);
            Job = new JobModel(JobId);
        }
    }
}
