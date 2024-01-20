using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeShiftLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Сomments { get; set; }
    }
}
