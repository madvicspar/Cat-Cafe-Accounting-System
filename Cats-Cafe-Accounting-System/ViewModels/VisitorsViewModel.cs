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

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class VisitorsViewModel : ObservableObject
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
        private ObservableCollection<Elem<VisitorModel>> items = new ObservableCollection<Elem<VisitorModel>>();
        public ObservableCollection<Elem<VisitorModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<VisitorModel>> filterItems = new ObservableCollection<Elem<VisitorModel>>();
        public ObservableCollection<Elem<VisitorModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        private ObservableCollection<FilterElem<VisitorModel>> firstNames = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> FirstNames
        {
            get { return firstNames; }
            set
            {
                firstNames = value;
                OnPropertyChanged(nameof(FirstNames));
            }
        }

        private ObservableCollection<FilterElem<VisitorModel>> lastNames = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> LastNames
        {
            get { return lastNames; }
            set
            {
                lastNames = value;
                OnPropertyChanged(nameof(LastNames));
            }
        }

        private ObservableCollection<FilterElem<VisitorModel>> phoneNumbers = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> PhoneNumbers
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

        private ObservableCollection<FilterElem<VisitorModel>> filterFirstNames = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> FilterFirstNames
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

        private ObservableCollection<FilterElem<VisitorModel>> filterLastNames = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> FilterLastNames
        {
            get { return filterLastNames; }
            set
            {
                filterLastNames = value;
                OnPropertyChanged(nameof(FilterLastNames));
            }
        }

        private ObservableCollection<FilterElem<VisitorModel>> filterPhoneNumbers = new ObservableCollection<FilterElem<VisitorModel>>();
        public ObservableCollection<FilterElem<VisitorModel>> FilterPhoneNumbers
        {
            get { return filterPhoneNumbers; }
            set
            {
                filterPhoneNumbers = value;
                OnPropertyChanged(nameof(FilterPhoneNumbers));
            }
        }

        public ICommand AddVisitorCommand { get; set; }
        public ICommand UpdateVisitorCommand { get; set; }
        public ICommand DeleteVisitorCommand { get; set; }
        public ICommand DeleteManyVisitorCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ChangeLastNameSelectionCommand { get; set; }
        public ICommand ChangeFirstNameSelectionCommand { get; set; }
        public ICommand ChangeGenderSelectionCommand { get; set; }
        public ICommand ChangeFieldCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchFirstNameCommand { get; set; }
        public ICommand SearchLastNameCommand { get; set; }
        public ICommand SearchGenderCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxGenderSelectionCommand { get; set; }
        public ICommand DeleteDateFiltersCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }

        public VisitorsViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Visitors.Include(p => p.Gender).ToList())
            {
                FilterItems.Add(new Elem<VisitorModel>(item.Clone() as VisitorModel));
                Items.Add(new Elem<VisitorModel>(item.Clone() as VisitorModel));
                if (!FirstNames.Any(p => p.Item.FirstName == item.FirstName))
                {
                    FirstNames.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                    FilterFirstNames.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                }
                if (!LastNames.Any(p => p.Item.LastName == item.LastName))
                {
                    LastNames.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                    FilterLastNames.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                }
                if (!PhoneNumbers.Any(p => p.Item.PhoneNumber == item.PhoneNumber))
                {
                    PhoneNumbers.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                    FilterPhoneNumbers.Add(new FilterElem<VisitorModel>(item.Clone() as VisitorModel));
                }
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                Genders.Add(new FilterElem<Gender>(item));
                FilterGenders.Add(new FilterElem<Gender>(item));
            }
            FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
            ExecuteDeleteDateFiltersCommand();
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddVisitorCommand = new RelayCommand(ExecuteAddVisitorCommand);
            UpdateVisitorCommand = new RelayCommand<VisitorModel>(ExecuteUpdateVisitorCommand);
            DeleteVisitorCommand = new RelayCommand<VisitorModel>(ExecuteDeleteVisitorCommand);
            DeleteManyVisitorCommand = new RelayCommand(ExecuteDeleteManyVisitorCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ChangeLastNameSelectionCommand = new RelayCommand<bool>(ExecuteChangeLastNameSelectionCommand);
            ChangeFirstNameSelectionCommand = new RelayCommand<bool>(ExecuteChangeFirstNameSelectionCommand);
            ChangeGenderSelectionCommand = new RelayCommand<bool>(ExecuteChangeGenderSelectionCommand);
            ChangeFieldCommand = new RelayCommand<string>(ExecuteChangeFieldCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            SearchFirstNameCommand = new RelayCommand(ExecuteSearchFirstNameCommand);
            SearchLastNameCommand = new RelayCommand(ExecuteSearchLastNameCommand);
            SearchGenderCommand = new RelayCommand(ExecuteSearchGenderCommand);
            UpdateCheckBoxSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxSelectionCommand);
            UpdateCheckBoxGenderSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxGenderSelectionCommand);
            DeleteDateFiltersCommand = new RelayCommand(ExecuteDeleteDateFiltersCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<VisitorModel>>(ExecuteElemUpdatedCommand);
        }

        public void ExecuteChangeGenderSelectionCommand(bool value)
        {
            foreach (var item in FilterGenders)
                item.IsSelected = !value;
        }

        public void ExecuteDeleteDateFiltersCommand()
        {
            DateTime birtdayMin = FilterItems.MinBy(item => item.Item.Birthday).Item.Birthday;
            DateTime birtdayMax = FilterItems.MaxBy(item => item.Item.Birthday).Item.Birthday;
            StartDate = birtdayMin;
            EndDate = birtdayMax;
        }

        public void ExecuteElemUpdatedCommand(Elem<VisitorModel> visitorUpdated)
        {
            if (_dbContext.Visitors.FirstOrDefault(p => p.Equals(visitorUpdated.Item)) == null || !visitorUpdated.Item.Equals(Items.First(p => p.Item.Id == visitorUpdated.Item.Id).Item))
            {
                visitorUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddVisitorCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastVisitor = FilterItems[FilterItems.Count - 1].Item;

            var visitorToAdd = new VisitorModel
            {
                FirstName = lastVisitor.FirstName,
                LastName = lastVisitor.LastName,
                Pathronymic = lastVisitor.Pathronymic,
                GenderId = lastVisitor.Gender.Id,
                Gender = lastVisitor.Gender,
                PhoneNumber = lastVisitor.PhoneNumber,
                Birthday = lastVisitor.Birthday
            };

            _dbContext.Visitors.Add(visitorToAdd.Clone() as VisitorModel);
            _dbContext.SaveChanges();
            visitorToAdd.Id = _dbContext.Visitors.First(p => p.LastName + p.FirstName + p.Pathronymic == visitorToAdd.LastName + visitorToAdd.FirstName + visitorToAdd.Pathronymic).Id;
            if (FirstNames.FirstOrDefault(p => p.Item.LastName + p.Item.FirstName + p.Item.Pathronymic == visitorToAdd.LastName + visitorToAdd.FirstName + visitorToAdd.Pathronymic) == null)
            {
                FirstNames.Add(new FilterElem<VisitorModel>(visitorToAdd.Clone() as VisitorModel));
                FilterFirstNames.Add(new FilterElem<VisitorModel>(visitorToAdd.Clone() as VisitorModel));
                Items.Add(new Elem<VisitorModel>(visitorToAdd.Clone() as VisitorModel));
                FilterItems.Add(new Elem<VisitorModel>(visitorToAdd.Clone() as VisitorModel));
            }
            UpdateTable();
        }
        /// <summary>
        /// Обновление VisitorModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="visitor"> Измененный VisitorModel </param>
        public void ExecuteUpdateVisitorCommand(VisitorModel? visitor)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)

            VisitorModel old = _dbContext.Visitors.First(p => p.Id == visitor.Id);
            string name = old.FirstName;
            _dbContext.Visitors.Update(VisitorModel.Update(old, visitor));
            _dbContext.SaveChanges();
            if (visitor.FirstName != name)
            {
                FirstNames.First(p => p.Item.FirstName == name).Item.FirstName = visitor.FirstName;
                FilterFirstNames.First(p => p.Item.Id == visitor.Id).Item.FirstName = visitor.FirstName;
            }
            if (visitor.FirstName != name)
            {
                FirstNames.First(p => p.Item.FirstName == name).Item.FirstName = visitor.FirstName;
                FilterFirstNames.First(p => p.Item.Id == visitor.Id).Item.FirstName = visitor.FirstName;
            }
            FilterItems.First(p => p.Item.Id == visitor.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteVisitorCommand(VisitorModel? visitor)
        {
            if (FilterItems.Count > 1)
            {
                string name = _dbContext.Visitors.First(p => p == visitor).FirstName;
                _dbContext.Visitors.Remove(visitor);
                _dbContext.SaveChanges();
                if (_dbContext.Visitors.FirstOrDefault(p => p.FirstName == name) == null)
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

        public void ExecuteDeleteManyVisitorCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<VisitorModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    VisitorModel visitor = _dbContext.Visitors.First(p => p.Id == itemToDelete.Item.Id);
                    _dbContext.Visitors.Remove(visitor);
                    _dbContext.SaveChanges();
                    if (_dbContext.Visitors.FirstOrDefault(p => p.FirstName == itemToDelete.Item.FirstName) == null)
                    {
                        FirstNames.Remove(FirstNames.First(p => p.Item.FirstName == itemToDelete.Item.FirstName));
                        FilterFirstNames.Remove(FilterFirstNames.First(p => p.Item.FirstName == itemToDelete.Item.FirstName));
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
            XWPFTable table = document.CreateTable(FilterItems.Count, 6);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Фамилия");
            table.GetRow(0).GetCell(1).SetText("Имя");
            table.GetRow(0).GetCell(2).SetText("Отчество");
            table.GetRow(0).GetCell(3).SetText("Пол");
            table.GetRow(0).GetCell(4).SetText("Дата рождения");
            table.GetRow(0).GetCell(6).SetText("Номер телефона");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                VisitorModel visitor = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(visitor.LastName);
                table.GetRow(i + 1).GetCell(1).SetText(visitor.FirstName);
                table.GetRow(i + 1).GetCell(2).SetText(visitor.Pathronymic);
                table.GetRow(i + 1).GetCell(3).SetText(visitor.Gender.Title);
                table.GetRow(i + 1).GetCell(4).SetText(visitor.Birthday.ToString());
                table.GetRow(i + 1).GetCell(5).SetText(visitor.PhoneNumber);
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
                    var worksheet = workbook.Worksheets.Add("Visitors");
                    List<string> headers = new List<string>() { "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения", "Номер телефона" };
                    List<List<string>> visitors = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        visitors.Add(new List<string>() { FilterItems[i].Item.LastName, FilterItems[i].Item.FirstName,
                            FilterItems[i].Item.Pathronymic, FilterItems[i].Item.Gender.Title, FilterItems[i].Item.Birthday.ToString("dd.MM.yyyy"),
                            FilterItems[i].Item.PhoneNumber });
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
            UpdateTable();
        }

        public void UpdateTable()
        {
            var visitorNames = new ObservableCollection<string>(FirstNames.Where(p => p.IsSelected).Select(p => p.Item.FirstName));
            var visitorGenders = new ObservableCollection<Gender>(Genders.Where(p => p.IsSelected).Select(p => p.Item));
            var visitorSearched = FilterItems.Select(p => p.Item);

            var filteredVisitors = _dbContext.Visitors
                //.Where(p => visitorSearched.Contains(p))
                .Where(p => visitorNames.Contains(p.FirstName))
                .Where(p => visitorGenders.Contains(p.Gender))
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredVisitors)
                FilterItems.Add(new Elem<VisitorModel>(item.Clone() as VisitorModel));
            FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
        }

        public void ExecuteUpdateCheckBoxSelectionCommand()
        {
            foreach (var item in FilterFirstNames)
                FirstNames.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxGenderSelectionCommand()
        {
            foreach (var item in FilterGenders)
                Genders.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
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

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<VisitorModel>> GetWithoutNameFilter()
        {
            var collection = new ObservableCollection<FilterElem<VisitorModel>>();
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
                        && FilterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null
                        && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate)
                        FilterItems.Add(item);
                }
                FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
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
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null
                            && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
                    break;
                case Fields.firstName:
                    var visitorFirstNames = new ObservableCollection<string>(FirstNames.Where(p => p.Item.FirstName.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.FirstName));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => visitorFirstNames.Contains(p.Item.FirstName)))
                    {
                        if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null
                            && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
                    break;
                case Fields.phoneNumber:
                    var visitorPhones = new ObservableCollection<string>(PhoneNumbers.Where(p => p.Item.PhoneNumber.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.PhoneNumber));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => visitorPhones.Contains(p.Item.PhoneNumber)))
                    {
                        if (FilterLastNames.FirstOrDefault(p => p.Item.LastName == item.Item.LastName && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterFirstNames.FirstOrDefault(p => p.Item.FirstName == item.Item.FirstName && p.IsSelected == true) is not null
                            && filterPhoneNumbers.FirstOrDefault(p => p.Item.PhoneNumber == item.Item.PhoneNumber && p.IsSelected == true) is not null
                            && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<VisitorModel>(new VisitorModel() { Gender = Genders[0].Item, Birthday = DateTime.Today }));
                    break;
            }
        }
    }
}
