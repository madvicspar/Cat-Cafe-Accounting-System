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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Text;
using System.Windows;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class EmployeesViewModel : ObservableObject
    {
        public bool IsEnabled { get; set; }
        public enum Fields
        {
            lastName, firstName, phoneNumber
        }

        public Fields field;
        public Fields Field
        {
            get { return field; }
            set
            {
                field = value;
                OnPropertyChanged(nameof(field));
            }
        }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string searchFirstName = "";
        public string SearchFirstName
        {
            get { return searchFirstName; }
            set
            {
                searchFirstName = value;
                OnPropertyChanged(nameof(SearchFirstName));
            }
        }

        private string searchLastName = "";
        public string SearchLastName
        {
            get { return searchLastName; }
            set
            {
                searchLastName = value;
                OnPropertyChanged(nameof(SearchLastName));
            }
        }

        private string searchGender = "";
        public string SearchGender
        {
            get { return searchGender; }
            set
            {
                searchGender = value;
                OnPropertyChanged(nameof(SearchGender));
            }
        }

        private string searchJob = "";
        public string SearchJob
        {
            get { return searchJob; }
            set
            {
                searchJob = value;
                OnPropertyChanged(nameof(SearchJob));
            }
        }

        private string searchPhoneNumber = "";
        public string SearchPhoneNumber
        {
            get { return searchPhoneNumber; }
            set
            {
                searchPhoneNumber = value;
                OnPropertyChanged(nameof(SearchPhoneNumber));
            }
        }

        private readonly ApplicationDbContext _dbContext;

        private ObservableCollection<Elem<EmployeeModel>> items = new ObservableCollection<Elem<EmployeeModel>>();
        public ObservableCollection<Elem<EmployeeModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<EmployeeModel>> filterItems = new ObservableCollection<Elem<EmployeeModel>>();
        public ObservableCollection<Elem<EmployeeModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> firstNames = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> FirstNames
        {
            get { return firstNames; }
            set
            {
                firstNames = value;
                OnPropertyChanged(nameof(FirstNames));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> lastNames = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> LastNames
        {
            get { return lastNames; }
            set
            {
                lastNames = value;
                OnPropertyChanged(nameof(LastNames));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> phoneNumbers = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> PhoneNumbers
        {
            get { return phoneNumbers; }
            set
            {
                phoneNumbers = value;
                OnPropertyChanged(nameof(PhoneNumbers));
            }
        }

        private ObservableCollection<FilterElem<Gender>> genders = new ObservableCollection<FilterElem<Gender>>();
        public ObservableCollection<FilterElem<Gender>> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                OnPropertyChanged(nameof(Genders));
            }
        }

        private ObservableCollection<FilterElem<JobModel>> jobs = new ObservableCollection<FilterElem<JobModel>>();
        public ObservableCollection<FilterElem<JobModel>> Jobs
        {
            get { return jobs; }
            set
            {
                jobs = value;
                OnPropertyChanged(nameof(Jobs));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> filterFirstNames = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> FilterFirstNames
        {
            get { return filterFirstNames; }
            set
            {
                filterFirstNames = value;
                OnPropertyChanged(nameof(FilterFirstNames));
            }
        }

        private ObservableCollection<FilterElem<Gender>> filterGenders = new ObservableCollection<FilterElem<Gender>>();
        public ObservableCollection<FilterElem<Gender>> FilterGenders
        {
            get { return filterGenders; }
            set
            {
                filterGenders = value;
                OnPropertyChanged(nameof(FilterGenders));
            }
        }

        private ObservableCollection<FilterElem<JobModel>> filterJobs = new ObservableCollection<FilterElem<JobModel>>();
        public ObservableCollection<FilterElem<JobModel>> FilterJobs
        {
            get { return filterJobs; }
            set
            {
                filterJobs = value;
                OnPropertyChanged(nameof(FilterJobs));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> filterLastNames = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> FilterLastNames
        {
            get { return filterLastNames; }
            set
            {
                filterLastNames = value;
                OnPropertyChanged(nameof(FilterLastNames));
            }
        }

        private ObservableCollection<FilterElem<EmployeeModel>> filterPhoneNumbers = new ObservableCollection<FilterElem<EmployeeModel>>();
        public ObservableCollection<FilterElem<EmployeeModel>> FilterPhoneNumbers
        {
            get { return filterPhoneNumbers; }
            set
            {
                filterPhoneNumbers = value;
                OnPropertyChanged(nameof(FilterPhoneNumbers));
            }
        }

        public ICommand AddEmployeeCommand { get; set; }
        public ICommand UpdateEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand DeleteManyEmployeeCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ChangeLastNameSelectionCommand { get; set; }
        public ICommand ChangeFirstNameSelectionCommand { get; set; }
        public ICommand ChangeGenderSelectionCommand { get; set; }
        public ICommand ChangeJobSelectionCommand { get; set; }
        public ICommand ChangeFieldCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchFirstNameCommand { get; set; }
        public ICommand SearchLastNameCommand { get; set; }
        public ICommand SearchGenderCommand { get; set; }
        public ICommand SearchJobCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxLastNameSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxFirstNameSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxGenderSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxJobSelectionCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }

        public EmployeesViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Employees.Include(p => p.Gender).Include(j => j.Job).ToList())
            {
                FilterItems.Add(new Elem<EmployeeModel>(item.Clone() as EmployeeModel));
                Items.Add(new Elem<EmployeeModel>(item.Clone() as EmployeeModel));
                if (!FirstNames.Any(p => p.Item.FirstName == item.FirstName))
                {
                    FirstNames.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                    FilterFirstNames.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                }
                if (!LastNames.Any(p => p.Item.LastName == item.LastName))
                {
                    LastNames.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                    FilterLastNames.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                }
                if (!PhoneNumbers.Any(p => p.Item.PhoneNumber == item.PhoneNumber))
                {
                    PhoneNumbers.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                    FilterPhoneNumbers.Add(new FilterElem<EmployeeModel>(item.Clone() as EmployeeModel));
                }
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                Genders.Add(new FilterElem<Gender>(item));
                FilterGenders.Add(new FilterElem<Gender>(item));
            }
            foreach (var item in _dbContext.Jobs.ToList())
            {
                Jobs.Add(new FilterElem<JobModel>(item));
                FilterJobs.Add(new FilterElem<JobModel>(item));
            }
            FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddEmployeeCommand = new RelayCommand(ExecuteAddEmployeeCommand);
            UpdateEmployeeCommand = new RelayCommand<EmployeeModel>(ExecuteUpdateEmployeeCommand);
            DeleteEmployeeCommand = new RelayCommand<EmployeeModel>(ExecuteDeleteEmployeeCommand);
            DeleteManyEmployeeCommand = new RelayCommand(ExecuteDeleteManyEmployeeCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ChangeLastNameSelectionCommand = new RelayCommand<bool>(ExecuteChangeLastNameSelectionCommand);
            ChangeFirstNameSelectionCommand = new RelayCommand<bool>(ExecuteChangeFirstNameSelectionCommand);
            ChangeGenderSelectionCommand = new RelayCommand<bool>(ExecuteChangeGenderSelectionCommand);
            ChangeJobSelectionCommand = new RelayCommand<bool>(ExecuteChangeJobSelectionCommand);
            ChangeFieldCommand = new RelayCommand<string>(ExecuteChangeFieldCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            SearchFirstNameCommand = new RelayCommand(ExecuteSearchFirstNameCommand);
            SearchLastNameCommand = new RelayCommand(ExecuteSearchLastNameCommand);
            SearchGenderCommand = new RelayCommand(ExecuteSearchGenderCommand);
            SearchJobCommand = new RelayCommand(ExecuteSearchJobCommand);
            UpdateCheckBoxLastNameSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxLastNameSelectionCommand);
            UpdateCheckBoxFirstNameSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxFirstNameSelectionCommand);
            UpdateCheckBoxGenderSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxGenderSelectionCommand);
            UpdateCheckBoxJobSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxJobSelectionCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<EmployeeModel>>(ExecuteElemUpdatedCommand);
        }

        public void ExecuteChangeGenderSelectionCommand(bool value)
        {
            foreach (var item in FilterGenders)
                item.IsSelected = !value;
        }

        private void ExecuteChangeJobSelectionCommand(bool value)
        {
            foreach (var item in FilterJobs)
                item.IsSelected = !value;
        }

        public void ExecuteElemUpdatedCommand(Elem<EmployeeModel> visitorUpdated)
        {
            if (_dbContext.Employees.FirstOrDefault(p => p.Equals(visitorUpdated.Item)) == null || !visitorUpdated.Item.Equals(Items.First(p => p.Item.Id == visitorUpdated.Item.Id).Item))
            {
                visitorUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddEmployeeCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastEmployee = FilterItems[FilterItems.Count - 1].Item;

            var salt = HashingHelper.GenerateSalt();

            EmployeeModel visitorToAdd = new EmployeeModel();
            try
            {
                visitorToAdd = new EmployeeModel
                {
                    FirstName = lastEmployee.FirstName,
                    LastName = lastEmployee.LastName,
                    Pathronymic = lastEmployee.Pathronymic,
                    GenderId = lastEmployee.Gender.Id,
                    Gender = lastEmployee.Gender,
                    PhoneNumber = lastEmployee.PhoneNumber,
                    Birthday = lastEmployee.Birthday,
                    ContractNumber = lastEmployee.ContractNumber,
                    JobId = lastEmployee.JobId,
                    Job = lastEmployee.Job,
                    Username = lastEmployee.PhoneNumber,
                    Salt = salt,
                    Password = Encoding.UTF8.GetBytes(lastEmployee.ContractNumber)
                };

                _dbContext.Employees.Add(visitorToAdd.Clone() as EmployeeModel);
                _dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введите все данные питомца в последней строке");
                return;
            }
            visitorToAdd.Id = _dbContext.Employees.First(p => p.LastName + p.FirstName + p.Pathronymic == visitorToAdd.LastName + visitorToAdd.FirstName + visitorToAdd.Pathronymic).Id;
            if (FirstNames.FirstOrDefault(p => p.Item.LastName + p.Item.FirstName + p.Item.Pathronymic == visitorToAdd.LastName + visitorToAdd.FirstName + visitorToAdd.Pathronymic) == null)
            {
                FirstNames.Add(new FilterElem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
                FilterFirstNames.Add(new FilterElem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
                LastNames.Add(new FilterElem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
                FilterLastNames.Add(new FilterElem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
                Items.Add(new Elem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
                FilterItems.Add(new Elem<EmployeeModel>(visitorToAdd.Clone() as EmployeeModel));
            }
            UpdateTable();
        }
        /// <summary>
        /// Обновление EmployeeModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="visitor"> Измененный EmployeeModel </param>
        public void ExecuteUpdateEmployeeCommand(EmployeeModel? visitor)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)
            EmployeeModel old = new EmployeeModel();
            string name = "";
            string lastName = "";
            try
            {
                old = _dbContext.Employees.First(p => p.Id == visitor.Id);
                name = old.FirstName;
                lastName = old.LastName;
                _dbContext.Employees.Update(EmployeeModel.Update(old, visitor));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.Employees.FirstOrDefault(p => p.Id == visitor.Id) != null)
                    MessageBox.Show("Введите все данные сотрудника");
                return;
            }
            if (visitor.FirstName != name)
            {
                FirstNames.First(p => p.Item.FirstName == name).Item.FirstName = visitor.FirstName;
                FilterFirstNames.First(p => p.Item.Id == visitor.Id).Item.FirstName = visitor.FirstName;
            }
            if (visitor.LastName != name)
            {
                LastNames.First(p => p.Item.LastName == name).Item.LastName = visitor.LastName;
                FilterLastNames.First(p => p.Item.Id == visitor.Id).Item.LastName = visitor.LastName;
            }
            FilterItems.First(p => p.Item.Id == visitor.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteEmployeeCommand(EmployeeModel? visitor)
        {
            if (FilterItems.Count > 1)
            {
                string name = "";
                try
                {
                    name = _dbContext.Employees.First(p => p == visitor).FirstName;
                    _dbContext.Employees.Remove(visitor);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    return;
                }

                if (_dbContext.Employees.FirstOrDefault(p => p.FirstName == name) == null)
                {
                    FirstNames.Remove(FirstNames.First(p => p.Item.FirstName == visitor.FirstName));
                    FilterFirstNames.Remove(FilterFirstNames.First(p => p.Item.FirstName == visitor.FirstName));
                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.FirstName = "";
                FilterItems[FilterItems.Count - 1].Item.PhoneNumber = "";
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyEmployeeCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<EmployeeModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    try
                    {
                        EmployeeModel visitor = _dbContext.Employees.First(p => p.Id == itemToDelete.Item.Id);
                        _dbContext.Employees.Remove(visitor);
                        _dbContext.SaveChanges();
                    }
                    catch
                    {
                        return;
                    }
                    if (_dbContext.Employees.FirstOrDefault(p => p.FirstName == itemToDelete.Item.FirstName) == null)
                    {
                        FirstNames.Remove(FirstNames.First(p => p.Item.FirstName == itemToDelete.Item.FirstName));
                        FilterFirstNames.Remove(FilterFirstNames.First(p => p.Item.FirstName == itemToDelete.Item.FirstName));
                    }
                    if (_dbContext.Employees.FirstOrDefault(p => p.LastName == itemToDelete.Item.LastName) == null)
                    {
                        LastNames.Remove(LastNames.First(p => p.Item.LastName == itemToDelete.Item.LastName));
                        FilterLastNames.Remove(FilterLastNames.First(p => p.Item.LastName == itemToDelete.Item.LastName));
                    }
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.FirstName = "";
                    FilterItems[FilterItems.Count - 1].Item.PhoneNumber = "";
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
            XWPFTable table = document.CreateTable(FilterItems.Count, 9);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Фамилия");
            table.GetRow(0).GetCell(1).SetText("Имя");
            table.GetRow(0).GetCell(2).SetText("Отчество");
            table.GetRow(0).GetCell(3).SetText("Пол");
            table.GetRow(0).GetCell(4).SetText("Дата рождения");
            table.GetRow(0).GetCell(6).SetText("Номер телефона");
            table.GetRow(0).GetCell(6).SetText("Должность");
            table.GetRow(0).GetCell(6).SetText("Табельный номер");
            table.GetRow(0).GetCell(6).SetText("Имя пользователя");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                EmployeeModel visitor = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(visitor.LastName);
                table.GetRow(i + 1).GetCell(1).SetText(visitor.FirstName);
                table.GetRow(i + 1).GetCell(2).SetText(visitor.Pathronymic);
                table.GetRow(i + 1).GetCell(3).SetText(visitor.Gender.Title);
                table.GetRow(i + 1).GetCell(4).SetText(visitor.Birthday.ToString());
                table.GetRow(i + 1).GetCell(5).SetText(visitor.PhoneNumber);
                table.GetRow(i + 1).GetCell(5).SetText(visitor.Job.Title);
                table.GetRow(i + 1).GetCell(5).SetText(visitor.ContractNumber);
                table.GetRow(i + 1).GetCell(5).SetText(visitor.Username);
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
                    var worksheet = workbook.Worksheets.Add("Employees");
                    List<string> headers = new List<string>() { "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения", "Номер телефона", "Должность", "Табельный номер", "Имя пользователя"};
                    List<List<string>> visitors = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        visitors.Add(new List<string>() { FilterItems[i].Item.LastName, FilterItems[i].Item.FirstName,
                            FilterItems[i].Item.Pathronymic, FilterItems[i].Item.Gender.Title, FilterItems[i].Item.Birthday.ToString("dd.MM.yyyy"),
                            FilterItems[i].Item.PhoneNumber, FilterItems[i].Item.Job.Title, FilterItems[i].Item.ContractNumber, FilterItems[i].Item.Username });
                    }
                    worksheet.Cell("A1").InsertTable(visitors);
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

        public void ExecuteChangeLastNameSelectionCommand(bool value)
        {
            foreach (var item in FilterLastNames)
                item.IsSelected = !value;
        }

        public void ExecuteChangeFirstNameSelectionCommand(bool value)
        {
            foreach (var item in FilterFirstNames)
                item.IsSelected = !value;
        }

        public void ExecuteChangeFieldCommand(string? value)
        {
            switch (value)
            {
                case "По имени":
                    Field = Fields.firstName;
                    break;
                case "По фамилии":
                    Field = Fields.lastName;
                    break;
                case "По номеру телефона":
                    Field = Fields.phoneNumber;
                    break;
                default:
                    Field = Fields.firstName;
                    break;
            }
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteFilterCommand()
        {
            foreach (var item in FirstNames)
            {
                var visitor = GetWithoutNameFilter().FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName);
                if (visitor is not null)
                {
                    item.IsSelected = visitor.IsSelected;
                }
            }
            foreach (var item in Genders)
            {
                var g = GetWithoutGenderFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (g is not null)
                {
                    item.IsSelected = g.IsSelected;
                }
            }
            foreach (var item in Jobs)
            {
                var g = GetWithoutJobFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (g is not null)
                {
                    item.IsSelected = g.IsSelected;
                }
            }
            UpdateTable();
        }

        public void UpdateTable()
        {
            var visitorNames = new ObservableCollection<string>(FirstNames.Where(p => p.IsSelected).Select(p => p.Item.FirstName));
            var visitorGenders = new ObservableCollection<Gender>(Genders.Where(p => p.IsSelected).Select(p => p.Item));
            var visitorJobs = new ObservableCollection<JobModel>(Jobs.Where(p => p.IsSelected).Select(p => p.Item));
            var visitorSearched = FilterItems.Select(p => p.Item);

            var filteredEmployees = _dbContext.Employees
                //.Where(p => visitorSearched.Contains(p))
                .Where(p => visitorNames.Contains(p.FirstName))
                .Where(p => visitorGenders.Contains(p.Gender))
                .Where(p => visitorJobs.Contains(p.Job))
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredEmployees)
                FilterItems.Add(new Elem<EmployeeModel>(item.Clone() as EmployeeModel));
            FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteUpdateCheckBoxLastNameSelectionCommand()
        {
            foreach (var item in FilterLastNames)
                LastNames.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteUpdateCheckBoxFirstNameSelectionCommand()
        {
            foreach (var item in FilterFirstNames)
                FirstNames.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteUpdateCheckBoxGenderSelectionCommand()
        {
            foreach (var item in FilterGenders)
                Genders.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteUpdateCheckBoxJobSelectionCommand()
        {
            foreach (var item in FilterJobs)
                Jobs.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteSearchFirstNameCommand()
        {
            if (SearchFirstName.Length > 5 && SearchFirstName[..5] == "Поиск")
            {
                FilterFirstNames.Clear();
                foreach (var item in FirstNames)
                {
                    if (!FilterFirstNames.Any(p => p.Item.FirstName == item.Item.FirstName))
                    {
                        FilterFirstNames.Add(item);
                        FilterFirstNames.Last().IsSelected = item.IsSelected;
                    }
                }
                return;
            }
            var visitorNames = new ObservableCollection<string>(FirstNames.Where(p => p.Item.FirstName.ToLower().Contains(SearchFirstName.ToLower())).Select(p => p.Item.FirstName));

            FilterFirstNames.Clear();
            foreach (var item in FirstNames.Where(p => visitorNames.Contains(p.Item.FirstName)))
            {
                if (!FilterFirstNames.Any(p => p.Item.FirstName == item.Item.FirstName))
                {
                    FilterFirstNames.Add(item);
                    FilterFirstNames.Last().IsSelected = item.IsSelected;
                }
            }
            // после удаления не обновляются значения фильтров
        }

        public void ExecuteSearchLastNameCommand()
        {
            if (SearchLastName.Length > 5 && SearchLastName[..5] == "Поиск")
            {
                FilterFirstNames.Clear();
                foreach (var item in FirstNames)
                {
                    if (!FilterFirstNames.Any(p => p.Item.LastName == item.Item.LastName))
                    {
                        FilterFirstNames.Add(item);
                        FilterFirstNames.Last().IsSelected = item.IsSelected;
                    }
                }
                return;
            }
            var visitorNames = new ObservableCollection<string>(FirstNames.Where(p => p.Item.LastName.ToLower().Contains(SearchLastName.ToLower())).Select(p => p.Item.LastName));

            FilterFirstNames.Clear();
            foreach (var item in FirstNames.Where(p => visitorNames.Contains(p.Item.LastName)))
            {
                if (!FilterFirstNames.Any(p => p.Item.LastName == item.Item.LastName))
                {
                    FilterFirstNames.Add(item);
                    FilterFirstNames.Last().IsSelected = item.IsSelected;
                }
            }
            // после удаления не обновляются значения фильтров
        }

        public void ExecuteSearchGenderCommand()
        {
            if (SearchGender.Length > 5 && SearchGender[..5] == "Поиск")
            {
                FilterGenders.Clear();
                foreach (var item in Genders)
                {
                    FilterGenders.Add(item);
                    FilterGenders.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var visitorGenders = new ObservableCollection<string>(Genders.Where(p => p.Item.Title.ToLower().Contains(SearchGender.ToLower())).Select(p => p.Item.Title));

            FilterGenders.Clear();
            foreach (var item in Genders.Where(p => visitorGenders.Contains(p.Item.Title)))
            {
                FilterGenders.Add(item);
                FilterGenders.Last().IsSelected = item.IsSelected;
            }
        }

        public void ExecuteSearchJobCommand()
        {
            if (SearchJob.Length > 5 && SearchJob[..5] == "Поиск")
            {
                FilterJobs.Clear();
                foreach (var item in Jobs)
                {
                    FilterJobs.Add(item);
                    FilterJobs.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var visitorJobs = new ObservableCollection<string>(Jobs.Where(p => p.Item.Title.ToLower().Contains(SearchJob.ToLower())).Select(p => p.Item.Title));

            FilterJobs.Clear();
            foreach (var item in Jobs.Where(p => visitorJobs.Contains(p.Item.Title)))
            {
                FilterJobs.Add(item);
                FilterJobs.Last().IsSelected = item.IsSelected;
            }
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<EmployeeModel>> GetWithoutNameFilter()
        {
            var collection = new ObservableCollection<FilterElem<EmployeeModel>>();
            foreach (var item in FirstNames)
            {
                if (!collection.Any(p => p.Item.FirstName == item.Item.FirstName))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<Gender>> GetWithoutGenderFilter()
        {
            var collection = new ObservableCollection<FilterElem<Gender>>();
            foreach (var item in Genders)
            {
                if (!collection.Any(p => p.Item.Title == item.Item.Title))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<JobModel>> GetWithoutJobFilter()
        {
            var collection = new ObservableCollection<FilterElem<JobModel>>();
            foreach (var item in Jobs)
            {
                if (!collection.Any(p => p.Item.Title == item.Item.Title))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        public void ExecuteSearchCommand()
        {
            if (SearchText.Length > 5 && SearchText[..5] == "Поиск")
            {
                FilterItems.Clear();
                foreach (var item in Items)
                {
                    if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                        && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                        && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                        && FilterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null)
                        FilterItems.Add(item);
                }
                FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
                return;
            }

            switch (Field)
            {
                case Fields.lastName:
                    var visitorLastNames = new ObservableCollection<string>(LastNames.Where(p => p.Item.LastName.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.LastName));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => visitorLastNames.Contains(p.Item.LastName)))
                    {
                        if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
                    break;
                case Fields.firstName:
                    var visitorFirstNames = new ObservableCollection<string>(FirstNames.Where(p => p.Item.FirstName.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.FirstName));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => visitorFirstNames.Contains(p.Item.FirstName)))
                    {
                        if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
                    break;
                case Fields.phoneNumber:
                    var visitorPhones = new ObservableCollection<string>(PhoneNumbers.Where(p => p.Item.PhoneNumber.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.PhoneNumber));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => visitorPhones.Contains(p.Item.PhoneNumber)))
                    {
                        if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<EmployeeModel>(new EmployeeModel() { Gender = Genders[0].Item, Birthday = DateTime.Today, Job = Jobs[0].Item }));
                    break;
            }
        }
    }
}
