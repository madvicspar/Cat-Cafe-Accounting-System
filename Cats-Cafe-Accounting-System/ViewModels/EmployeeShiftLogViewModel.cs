using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class EmployeeShiftLogViewModel : ObservableObject
    {
        private string searchNumber;
        public string SearchNumber
        {
            get { return searchNumber; }
            set
            {
                searchNumber = value;
                OnPropertyChanged(nameof(SearchNumber));
            }
        }
        public bool IsEnabled { get; set; }

        private DateTime startDate = new DateTime();
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        private DateTime endDate = new DateTime();
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private ObservableCollection<Elem<EmployeeShiftLogEntryModel>> items = new ObservableCollection<Elem<EmployeeShiftLogEntryModel>>();
        public ObservableCollection<Elem<EmployeeShiftLogEntryModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<EmployeeShiftLogEntryModel>> filterItems = new ObservableCollection<Elem<EmployeeShiftLogEntryModel>>();
        public ObservableCollection<Elem<EmployeeShiftLogEntryModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        private ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>> numbers = new ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>>();
        public ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>> Numbers
        {
            get { return numbers; }
            set
            {
                numbers = value;
                OnPropertyChanged(nameof(Numbers));
            }
        }

        private ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>> filterNumbers = new ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>>();
        public ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>> FilterNumbers
        {
            get { return filterNumbers; }
            set
            {
                filterNumbers = value;
                OnPropertyChanged(nameof(FilterNumbers));
            }
        }

        public ICommand AddEntryCommand { get; set; }
        public ICommand UpdateEntryCommand { get; set; }
        public ICommand DeleteEntryCommand { get; set; }
        public ICommand DeleteManyEntryCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ChangeNumberSelectionCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchNumberCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxNumberSelectionCommand { get; set; }
        public ICommand DeleteDateFiltersCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }
        public EmployeeShiftLogViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.EmployeeShiftLogEntries.Include(p => p.Employee).ToList())
            {
                FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
                Items.Add(new Elem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
                Numbers.Add(new FilterElem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
                if (FilterNumbers.Count > 0)
                {
                    if (!FilterNumbers.Any(p => p.Item.Employee.ContractNumber == item.Employee.ContractNumber))
                    {
                        FilterNumbers.Add(new FilterElem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
                    }
                }
                else
                {
                    FilterNumbers.Add(new FilterElem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
                }
            }
            FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(new EmployeeShiftLogEntryModel() { Date = DateTime.Today }));
            ExecuteDeleteDateFiltersCommand();
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddEntryCommand = new RelayCommand(ExecuteAddEntryCommand);
            UpdateEntryCommand = new RelayCommand<EmployeeShiftLogEntryModel>(ExecuteUpdateEntryCommand);
            DeleteEntryCommand = new RelayCommand<EmployeeShiftLogEntryModel>(ExecuteDeleteEntryCommand);
            DeleteManyEntryCommand = new RelayCommand(ExecuteDeleteManyEntryCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ChangeNumberSelectionCommand = new RelayCommand<bool>(ExecuteChangeNumberSelectionCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            SearchNumberCommand = new RelayCommand(ExecuteSearchNumberCommand);
            UpdateCheckBoxSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxSelectionCommand);
            UpdateCheckBoxNumberSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxNumberSelectionCommand);
            DeleteDateFiltersCommand = new RelayCommand(ExecuteDeleteDateFiltersCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<EmployeeShiftLogEntryModel>>(ExecuteElemUpdatedCommand);
        }
        public void ExecuteDeleteDateFiltersCommand()
        {
            DateTime birtdayMin = FilterItems.MinBy(item => item.Item.Date).Item.Date;
            DateTime birtdayMax = FilterItems.MaxBy(item => item.Item.Date).Item.Date;
            StartDate = birtdayMin;
            EndDate = birtdayMax;
        }

        public void ExecuteElemUpdatedCommand(Elem<EmployeeShiftLogEntryModel> entryUpdated)
        {
            if (_dbContext.EmployeeShiftLogEntries.FirstOrDefault(p => p.Equals(entryUpdated.Item)) == null || !entryUpdated.Item.Equals(Items.First(p => p.Item.Id == entryUpdated.Item.Id).Item))
            {
                entryUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddEntryCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastEntry = FilterItems[FilterItems.Count - 1].Item;
            EmployeeShiftLogEntryModel entryToAdd = new EmployeeShiftLogEntryModel();
            try
            {
                entryToAdd = new EmployeeShiftLogEntryModel
                {
                    Date = lastEntry.Date,
                    EmployeeId = lastEntry.Employee.Id,
                    Employee = lastEntry.Employee,
                    Comments = lastEntry.Comments
                };

                _dbContext.EmployeeShiftLogEntries.Add(entryToAdd.Clone() as EmployeeShiftLogEntryModel);
                _dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введите все данные записи");
                return;
            }
            entryToAdd.Id = _dbContext.EmployeeShiftLogEntries.First(p => p.Employee.ContractNumber == entryToAdd.Employee.ContractNumber).Id;
            if (Numbers.FirstOrDefault(p => p.Item.Employee.ContractNumber == entryToAdd.Employee.ContractNumber) == null)
            {
                Numbers.Add(new FilterElem<EmployeeShiftLogEntryModel>(entryToAdd.Clone() as EmployeeShiftLogEntryModel));
                FilterNumbers.Add(new FilterElem<EmployeeShiftLogEntryModel>(entryToAdd.Clone() as EmployeeShiftLogEntryModel));
                Items.Add(new Elem<EmployeeShiftLogEntryModel>(entryToAdd.Clone() as EmployeeShiftLogEntryModel));
                FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(entryToAdd.Clone() as EmployeeShiftLogEntryModel));
            }
            UpdateTable();
        }
        /// <summary>
        /// Обновление EmployeeShiftLogEntryModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="entry"> Измененный EmployeeShiftLogEntryModel </param>
        public void ExecuteUpdateEntryCommand(EmployeeShiftLogEntryModel? entry)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)
            string name = "";
            try
            {
                EmployeeShiftLogEntryModel old = _dbContext.EmployeeShiftLogEntries.First(p => p.Id == entry.Id);
                name = old.Employee.ContractNumber;
                _dbContext.EmployeeShiftLogEntries.Update(EmployeeShiftLogEntryModel.Update(old, entry));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.EmployeeShiftLogEntries.FirstOrDefault(p => p.Id == entry.Id) != null)
                    MessageBox.Show("Введите все данные записи");
                return;
            }
            if (entry.Employee.ContractNumber != name)
            {
                Numbers.First(p => p.Item.Employee.ContractNumber == name).Item.Employee.ContractNumber = entry.Employee.ContractNumber;
                FilterNumbers.First(p => p.Item.Id == entry.Id).Item.Employee.ContractNumber = entry.Employee.ContractNumber;
            }
            FilterItems.First(p => p.Item.Id == entry.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteEntryCommand(EmployeeShiftLogEntryModel? entry)
        {
            if (FilterItems.Count > 1)
            {
                string name = "";
                try
                {
                    name = _dbContext.EmployeeShiftLogEntries.First(p => p == entry).Employee.ContractNumber;
                    _dbContext.EmployeeShiftLogEntries.Remove(entry);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Введите все данные записи");
                    return;
                }
                if (_dbContext.EmployeeShiftLogEntries.FirstOrDefault(p => p.Employee.ContractNumber == name) == null)
                {
                    Numbers.Remove(Numbers.First(p => p.Item.Employee.ContractNumber == entry.Employee.ContractNumber));
                    FilterNumbers.Remove(FilterNumbers.First(p => p.Item.Employee.ContractNumber == entry.Employee.ContractNumber));
                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.Comments = "";
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyEntryCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<EmployeeShiftLogEntryModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    try
                    {
                        EmployeeShiftLogEntryModel entry = _dbContext.EmployeeShiftLogEntries.First(p => p.Id == itemToDelete.Item.Id);
                        _dbContext.EmployeeShiftLogEntries.Remove(entry);
                        _dbContext.SaveChanges();

                        if (_dbContext.EmployeeShiftLogEntries.FirstOrDefault(p => p.Employee.ContractNumber == itemToDelete.Item.Employee.ContractNumber) == null)
                        {
                            Numbers.Remove(Numbers.First(p => p.Item.Employee.ContractNumber == itemToDelete.Item.Employee.ContractNumber));
                            FilterNumbers.Remove(FilterNumbers.First(p => p.Item.Employee.ContractNumber == itemToDelete.Item.Employee.ContractNumber));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Введите все данные записи");
                        return;
                    }
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.Comments = "";
                }
            }
            UpdateTable();
        }

        [ExcludeFromCodeCoverage]
        private void ExecuteWordExportCommand()
        {
            // сделать кнопку неактивной, если в коллекции 0 элементов (фильтры учитываются)
            // Создание нового документа Word
            XWPFDocument document = new XWPFDocument();

            // Создание таблицы
            XWPFTable table = document.CreateTable(FilterItems.Count, 3);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Дата");
            table.GetRow(0).GetCell(1).SetText("Табельный номер сотрудника");
            table.GetRow(0).GetCell(2).SetText("Комментарий");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                EmployeeShiftLogEntryModel entry = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(entry.Date.ToString());
                table.GetRow(i + 1).GetCell(1).SetText(entry.Employee.ContractNumber);
                table.GetRow(i + 1).GetCell(2).SetText(entry.Comments);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word Documents|*.docx";
            saveFileDialog.DefaultExt = ".docx";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                {

                    document.Write(stream);
                }
                Process.Start(new ProcessStartInfo
                {
                    FileName = saveFileDialog.FileName,
                    UseShellExecute = true
                });
            }
        }
        [ExcludeFromCodeCoverage]
        private void ExecuteExcelExportCommand()
        {
            using (var workbook = new XLWorkbook())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = ".xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    var worksheet = workbook.Worksheets.Add("Pets");
                    List<string> headers = new List<string>() { "Дата", "Табельный номер сотрудника", "Комментарий" };
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Date.ToString(), FilterItems[i].Item.Employee.ContractNumber, FilterItems[i].Item.Comments });
                    }
                    worksheet.Cell("A1").InsertTable(pets);
                    for (int i = 0; i < headers.Count; i++)
                        worksheet.Column(i + 1).Cell(1).Value = headers[i];
                    worksheet.Columns().AdjustToContents();
                    string path = saveFileDialog.FileName;
                    workbook.SaveAs(path);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
            }
        }

        public void ExecuteChangeSelectionCommand(bool value)
        {
            foreach (var item in FilterItems)
                item.IsSelected = value;
        }

        public void ExecuteChangeNumberSelectionCommand(bool value)
        {
            foreach (var item in FilterNumbers)
                item.IsSelected = !value;
        }

        public void ExecuteFilterCommand()
        {
            foreach (var item in Numbers)
            {
                var pet = GetWithoutNumberFilter().FirstOrDefault(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber);
                if (pet is not null)
                {
                    item.IsSelected = pet.IsSelected;
                }
            }
            UpdateTable();
        }

        public void UpdateTable()
        {
            var petNumbers = new ObservableCollection<EmployeeShiftLogEntryModel>(Numbers.Where(p => p.IsSelected).Select(p => p.Item));
            var petSearched = FilterItems.Select(p => p.Item);

            var filteredPets = _dbContext.EmployeeShiftLogEntries
                //.Where(p => petSearched.Contains(p))
                .Where(p => petNumbers.Contains(p))
                .Where(p => p.Date >= StartDate && p.Date <= EndDate)
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredPets)
                FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(item.Clone() as EmployeeShiftLogEntryModel));
            FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(new EmployeeShiftLogEntryModel() { Date = DateTime.Today }));
        }

        public void ExecuteUpdateCheckBoxSelectionCommand()
        {
            foreach (var item in FilterNumbers)
                Numbers.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxNumberSelectionCommand()
        {
            foreach (var item in FilterNumbers)
                Numbers.First(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber).IsSelected = item.IsSelected;
        }

        public void ExecuteSearchNumberCommand()
        {
            if (SearchNumber.Length > 5 && SearchNumber[..5] == "Поиск")
            {
                FilterNumbers.Clear();
                foreach (var item in Numbers)
                {
                    if (!FilterNumbers.Any(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber))
                    {
                        FilterNumbers.Add(item);
                        FilterNumbers.Last().IsSelected = item.IsSelected;
                    }
                }
                return;
            }
            var petNames = new ObservableCollection<string>(Numbers.Where(p => p.Item.Employee.ContractNumber.ToLower().Contains(SearchNumber.ToLower())).Select(p => p.Item.Employee.ContractNumber));

            FilterNumbers.Clear();
            foreach (var item in Numbers.Where(p => petNames.Contains(p.Item.Employee.ContractNumber)))
            {
                if (!FilterNumbers.Any(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber))
                {
                    FilterNumbers.Add(item);
                    FilterNumbers.Last().IsSelected = item.IsSelected;
                }
            }
            // после удаления не обновляются значения фильтров
        }

        public ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>> GetWithoutNumberFilter()
        {
            var collection = new ObservableCollection<FilterElem<EmployeeShiftLogEntryModel>>();
            foreach (var item in Numbers)
            {
                if (!collection.Any(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        public void ExecuteSearchCommand()
        {
            if (SearchNumber.Length > 5 && SearchNumber[..5] == "Поиск")
            {
                FilterItems.Clear();
                foreach (var item in Items)
                {
                    if (FilterNumbers.FirstOrDefault(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber && p.IsSelected == true) is not null
                        && item.Item.Date >= StartDate && item.Item.Date <= EndDate)
                        FilterItems.Add(item);
                }
                FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(new EmployeeShiftLogEntryModel() { Date = DateTime.Today }));
                return;
            }

            var numbers = new ObservableCollection<string>(Numbers.Where(p => p.Item.Employee.ContractNumber.ToLower().Contains(SearchNumber.ToLower())).Select(p => p.Item.Employee.ContractNumber));

            FilterItems.Clear();
            foreach (var item in Items.Where(p => numbers.Contains(p.Item.Employee.ContractNumber)))
            {
                if (FilterNumbers.FirstOrDefault(p => p.Item.Employee.ContractNumber == item.Item.Employee.ContractNumber && p.IsSelected == true) is not null
                    && item.Item.Date >= StartDate && item.Item.Date <= EndDate)
                    FilterItems.Add(item);
            }
            FilterItems.Add(new Elem<EmployeeShiftLogEntryModel>(new EmployeeShiftLogEntryModel() { Date = DateTime.Today }));
        }
    }
}
