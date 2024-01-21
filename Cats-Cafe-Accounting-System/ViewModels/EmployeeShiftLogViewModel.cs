using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class EmployeeShiftLogViewModel : ObservableObject
    {
        private ObservableCollection<EmployeeShiftLogEntryModel> employeeShiftLogEntries;
        public ObservableCollection<EmployeeShiftLogEntryModel> EmployeeShiftLogEntries
        {
            get { return employeeShiftLogEntries; }
            set
            {
                employeeShiftLogEntries = value;
                OnPropertyChanged(nameof(EmployeeShiftLogEntries));
            }
        }
        public EmployeeShiftLogViewModel()
        {
            // Инициализация коллекции питомцев
            EmployeeShiftLogEntries = GetEmployeeShiftLogEntriesFromTable("employee_shift_log_entries");
        }
        public static ObservableCollection<EmployeeShiftLogEntryModel> GetEmployeeShiftLogEntriesFromTable(string table)
        {
            ObservableCollection<EmployeeShiftLogEntryModel> employeeShiftLogEntries = new ObservableCollection<EmployeeShiftLogEntryModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                EmployeeShiftLogEntryModel employeeShiftLogEntry = new EmployeeShiftLogEntryModel(Convert.ToInt32(row["id"]), 
                    DateTime.Parse(row["date"].ToString()), Convert.ToInt32(row["employee_id"]), row["comments"].ToString());
                employeeShiftLogEntries.Add(employeeShiftLogEntry);
            }

            return employeeShiftLogEntries;
        }
    }
}
