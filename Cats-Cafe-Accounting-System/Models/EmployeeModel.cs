using Cats_Cafe_Accounting_System.RegularClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeModel : ObservableObject, ICloneable, IEquatable<EmployeeModel>
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

        public static EmployeeModel Update(EmployeeModel oldEmployee, EmployeeModel newEmployee)
        {
            oldEmployee.LastName = newEmployee.LastName;
            oldEmployee.FirstName = newEmployee.FirstName;
            oldEmployee.Pathronymic = newEmployee.Pathronymic;
            oldEmployee.GenderId = newEmployee.GenderId;
            oldEmployee.Gender = newEmployee.Gender;
            oldEmployee.JobId = newEmployee.JobId;
            oldEmployee.Job = newEmployee.Job;
            oldEmployee.Birthday = newEmployee.Birthday;
            oldEmployee.ContractNumber = newEmployee.ContractNumber;
            oldEmployee.PhoneNumber = newEmployee.PhoneNumber;
            oldEmployee.Username = newEmployee.Username;
            oldEmployee.Password = newEmployee.Password;
            oldEmployee.Salt = newEmployee.Salt;
            return oldEmployee;
        }

        public object Clone()
        {
            return new EmployeeModel
            {
                Id = Id,
                LastName = LastName,
                FirstName = FirstName,
                Pathronymic = Pathronymic,
                GenderId = GenderId,
                Gender = Gender,
                JobId = JobId,
                Job = Job,
                Birthday = Birthday,
                ContractNumber = ContractNumber,
                PhoneNumber = PhoneNumber,
                Username = Username,
                Password = Password, 
                Salt = Salt
            };
        }

        public bool Equals(EmployeeModel? other)
        {
            return other?.LastName == LastName
                && other?.FirstName == FirstName
                && other?.Pathronymic == Pathronymic
                && other?.GenderId == GenderId
                && other?.Gender == Gender
                && other?.JobId == JobId
                && other?.Job == Job
                && other?.Birthday == Birthday
                && other?.ContractNumber == ContractNumber
                && other?.PhoneNumber == PhoneNumber
                && other?.Username == Username
                && other?.Password == Password;
        }
    }
}
