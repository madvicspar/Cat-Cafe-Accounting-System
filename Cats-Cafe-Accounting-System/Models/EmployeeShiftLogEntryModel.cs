using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Cats_Cafe_Accounting_System.RegularClasses;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Cats_Cafe_Accounting_System.Models
{
    public class EmployeeShiftLogEntryModel : ObservableObject, ICloneable, IEquatable<EmployeeShiftLogEntryModel>
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual EmployeeModel Employee { get; set; }
        public string? Comments { get; set; }

        public EmployeeShiftLogEntryModel()
        {
            Employee = new EmployeeModel();
        }

        public static EmployeeShiftLogEntryModel Update(EmployeeShiftLogEntryModel oldEmployeeShiftLogEntryModel, EmployeeShiftLogEntryModel newEmployeeShiftLogEntryModel)
        {
            oldEmployeeShiftLogEntryModel.Date = newEmployeeShiftLogEntryModel.Date;
            oldEmployeeShiftLogEntryModel.Employee = newEmployeeShiftLogEntryModel.Employee;
            oldEmployeeShiftLogEntryModel.EmployeeId = newEmployeeShiftLogEntryModel.EmployeeId;
            oldEmployeeShiftLogEntryModel.Comments = newEmployeeShiftLogEntryModel.Comments;
            return oldEmployeeShiftLogEntryModel;
        }

        public object Clone()
        {
            return new EmployeeShiftLogEntryModel
            {
                Id = Id,
                Date = Date,
                EmployeeId = EmployeeId,
                Employee = Employee,
                Comments = Comments
            };
        }

        public bool Equals(EmployeeShiftLogEntryModel? other)
        {
            return other?.Date == Date
                && other?.EmployeeId == EmployeeId
                && other?.Employee == Employee
                && other?.Comments == Comments;
        }
    }
}
