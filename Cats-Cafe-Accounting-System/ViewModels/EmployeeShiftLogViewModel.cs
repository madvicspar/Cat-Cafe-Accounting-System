using Cats_Cafe_Accounting_System.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

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
            EmployeeShiftLogEntries = new ObservableCollection<EmployeeShiftLogEntryModel>();
            EmployeeShiftLogEntries.Add(new EmployeeShiftLogEntryModel { EmployeeId = 1, Сomments = "lohov", Date = new DateOnly(2023, 05, 23) });
            EmployeeShiftLogEntries.Add(new EmployeeShiftLogEntryModel { EmployeeId = 3, Сomments = "дада", Date = new DateOnly(2023, 02, 01) }) ;
            EmployeeShiftLogEntries.Add(new EmployeeShiftLogEntryModel { EmployeeId = 4, Сomments = "чел", Date = new DateOnly(2023, 11, 19) }) ;

        }
    }
}
