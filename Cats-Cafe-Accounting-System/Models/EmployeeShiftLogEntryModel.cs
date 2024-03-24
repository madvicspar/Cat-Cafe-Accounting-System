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
        public int EmployeeId { get; set; }
        [ForeignKey("Employee")]
        public virtual EmployeeModel Employee { get; set; }
        public string Сomments { get; set; }
    }
}
