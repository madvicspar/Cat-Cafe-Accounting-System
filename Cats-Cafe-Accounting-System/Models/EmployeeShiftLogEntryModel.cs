using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeShiftLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public string Сomments { get; set; }
        public EmployeeModel Employee { get; set; }

        public EmployeeShiftLogEntryModel(int id, DateTime date, int employeeId, string comments)
        {
            Id = id;
            Date = date;
            EmployeeId = employeeId;
            Сomments = comments;
            Employee = new EmployeeModel(EmployeeId);
        }
        public EmployeeShiftLogEntryModel(int id)
        {
            DataRow row = DBContext.GetById("employee_shift_log", id);
            Id = id;
            Date = DateTime.Parse(row["date"].ToString());
            EmployeeId = Convert.ToInt32(row["employee_id"]);
            Сomments = row["comments"].ToString();
            Employee = new EmployeeModel(EmployeeId);
        }
    }
}
