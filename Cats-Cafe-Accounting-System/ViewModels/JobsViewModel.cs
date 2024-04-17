using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class JobsViewModel : ObservableObject
    {
        private string searchText = "";
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string searchTitle = "";
        public string SearchTitle
        {
            get { return searchTitle; }
            set
            {
                searchTitle = value;
                OnPropertyChanged(nameof(SearchTitle));
            }
        }

        public bool IsEnabled { get; set; }

        private readonly ApplicationDbContext _dbContext;

        private ObservableCollection<Elem<JobModel>> items = new ObservableCollection<Elem<JobModel>>();
        public ObservableCollection<Elem<JobModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<JobModel>> filterItems = new ObservableCollection<Elem<JobModel>>();
        public ObservableCollection<Elem<JobModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        private ObservableCollection<FilterElem<JobModel>> titles = new ObservableCollection<FilterElem<JobModel>>();
        public ObservableCollection<FilterElem<JobModel>> Titles
        {
            get { return titles; }
            set
            {
                titles = value;
                OnPropertyChanged(nameof(Titles));
            }
        }

        private ObservableCollection<FilterElem<JobModel>> filterTitles = new ObservableCollection<FilterElem<JobModel>>();
        public ObservableCollection<FilterElem<JobModel>> FilterTitles
        {
            get { return filterTitles; }
            set
            {
                filterTitles = value;
                OnPropertyChanged(nameof(FilterTitles));
            }
        }

        public ICommand AddJobCommand { get; set; }
        public ICommand UpdateJobCommand { get; set; }
        public ICommand DeleteJobCommand { get; set; }
        public ICommand DeleteManyJobCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand SearchTitleCommand { get; set; }
        public ICommand ChangeTitleSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxTitleSelectionCommand { get; set; }

        public JobsViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Jobs)
            {
                Items.Add(new Elem<JobModel>(item.Clone() as JobModel));
                FilterItems.Add(new Elem<JobModel>(item.Clone() as JobModel));
                Titles.Add(new FilterElem<JobModel>(item.Clone() as JobModel));
                FilterTitles.Add(new FilterElem<JobModel>(item.Clone() as JobModel));
            }
            FilterItems.Add(new Elem<JobModel>(new JobModel()));
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddJobCommand = new RelayCommand(ExecuteAddJobCommand);
            UpdateJobCommand = new RelayCommand<JobModel>(ExecuteUpdateJobCommand);
            DeleteJobCommand = new RelayCommand<JobModel>(ExecuteDeleteJobCommand);
            DeleteManyJobCommand = new RelayCommand(ExecuteDeleteManyJobsCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            SearchTitleCommand = new RelayCommand(ExecuteSearchTitleCommand);
            ChangeTitleSelectionCommand = new RelayCommand<bool>(ExecuteChangeTitleSelectionCommand);
            UpdateCheckBoxTitleSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxTitleSelectionCommand);
        }

        public void ExecuteAddJobCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastJob = FilterItems[FilterItems.Count - 1].Item;

            var jobToAdd = new JobModel
            {
                Title = lastJob.Title,
                Rate = lastJob.Rate,
            };

            _dbContext.Jobs.Add(jobToAdd.Clone() as JobModel);
            _dbContext.SaveChanges();
            jobToAdd.Id = _dbContext.Jobs.First(p => p.Title == jobToAdd.Title).Id;
            if (Titles.FirstOrDefault(p => p.Item.Title == jobToAdd.Title) == null)
            {
                Titles.Add(new FilterElem<JobModel>(jobToAdd.Clone() as JobModel));
                FilterTitles.Add(new FilterElem<JobModel>(jobToAdd.Clone() as JobModel));
                Items.Add(new Elem<JobModel>(jobToAdd.Clone() as JobModel));
                FilterItems.Add(new Elem<JobModel>(jobToAdd.Clone() as JobModel));
            }
            UpdateTable();
        }

        public void ExecuteUpdateJobCommand(JobModel? job)
        {
            JobModel old = _dbContext.Jobs.First(p => p.Id == job.Id);
            string name = old.Title;
            _dbContext.Jobs.Update(JobModel.Update(old, job));
            _dbContext.SaveChanges();
            if (job.Title != name)
            {
                Titles.First(p => p.Item.Title == name).Item.Title = job.Title;
                FilterTitles.First(p => p.Item.Id == job.Id).Item.Title = job.Title;
            }
            FilterItems.First(p => p.Item.Id == job.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteJobCommand(JobModel? job)
        {
            if (FilterItems.Count > 1)
            {
                string name = _dbContext.Jobs.First(p => p == job).Title;
                _dbContext.Jobs.Remove(job);
                _dbContext.SaveChanges();
                if (_dbContext.Jobs.FirstOrDefault(p => p.Title == name) == null)
                {
                    Titles.Remove(Titles.First(p => p.Item.Title == job.Title));
                    FilterTitles.Remove(FilterTitles.First(p => p.Item.Title == job.Title));
                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.Title = "";
                FilterItems[FilterItems.Count - 1].Item.Rate = 0;
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyJobsCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<JobModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    JobModel job = _dbContext.Jobs.First(p => p.Id == itemToDelete.Item.Id);
                    _dbContext.Jobs.Remove(job);
                    _dbContext.SaveChanges();
                    if (_dbContext.Jobs.FirstOrDefault(p => p.Title == itemToDelete.Item.Title) == null)
                    {
                        Titles.Remove(Titles.First(p => p.Item.Title == itemToDelete.Item.Title));
                        FilterTitles.Remove(FilterTitles.First(p => p.Item.Title == itemToDelete.Item.Title));
                    }
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.Title = "";
                    FilterItems[FilterItems.Count - 1].Item.Rate = 0;
                }
            }
            UpdateTable();
        }

        [ExcludeFromCodeCoverage]
        private void ExecuteWordExportCommand()
        {
            // Создание нового документа Word
            XWPFDocument document = new XWPFDocument();

            // Создание таблицы
            XWPFTable table = document.CreateTable(FilterItems.Count + 1, 2);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Title");
            table.GetRow(0).GetCell(1).SetText("Rate");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count; i++)
            {
                JobModel job = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(job.Title);
                table.GetRow(i + 1).GetCell(1).SetText(job.Rate.ToString());
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
                    var worksheet = workbook.Worksheets.Add("Должности");
                    List<string> headers = new List<string>() { "Название", "Ставка"};
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Title, FilterItems[i].Item.Rate.ToString() });
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

        public void ExecuteChangeTitleSelectionCommand(bool value)
        {
            foreach (var item in FilterTitles)
                item.IsSelected = !value;
        }

        public void ExecuteChangeSelectionCommand(bool value)
        {
            foreach (var item in FilterItems)
                item.IsSelected = value;
        }

        public void ExecuteUpdateCheckBoxTitleSelectionCommand()
        {
            foreach (var item in FilterTitles)
                Titles.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void UpdateTable()
        {
            var jobTitles = new ObservableCollection<string>(FilterTitles.Where(p => p.IsSelected).Select(p => p.Item.Title));

            var filteredJobs = _dbContext.Jobs
                .Where(p => jobTitles.Contains(p.Title))
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredJobs)
                FilterItems.Add(new Elem<JobModel>(item.Clone() as JobModel));
            FilterItems.Add(new Elem<JobModel>(new JobModel()));
        }

        public void ExecuteFilterCommand()
        {
            foreach (var item in Titles)
            {
                var pet = GetWithoutTitleFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (pet is not null)
                {
                    item.IsSelected = pet.IsSelected;
                }
            }
            UpdateTable();
        }

        public ObservableCollection<FilterElem<JobModel>> GetWithoutTitleFilter()
        {
            var collection = new ObservableCollection<FilterElem<JobModel>>();
            foreach (var item in Titles)
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
                    if (FilterTitles.FirstOrDefault(p => p.Item.Title == item.Item.Title && p.IsSelected == true) is not null)
                        FilterItems.Add(item);
                }
                FilterItems.Add(new Elem<JobModel>(new JobModel() { Title = "", Rate = 0 }));
                return;
            }

            var jobTitles = new ObservableCollection<string>(Titles.Where(p => p.Item.Title.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.Title));

            FilterItems.Clear();
            foreach (var item in Items.Where(p => jobTitles.Contains(p.Item.Title)))
            {
                if (FilterTitles.FirstOrDefault(p => p.Item.Title == item.Item.Title && p.IsSelected == true) is not null)
                    FilterItems.Add(item);
            }
            FilterItems.Add(new Elem<JobModel>(new JobModel() { Title = "", Rate = 0 }));
        }

        public void ExecuteSearchTitleCommand()
        {
            if (SearchTitle.Length > 5 && SearchTitle[..5] == "Поиск")
            {
                FilterTitles.Clear();
                foreach (var item in Titles)
                {
                    FilterTitles.Add(item);
                    FilterTitles.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var petGenders = new ObservableCollection<string>(Titles.Where(p => p.Item.Title.ToLower().Contains(SearchTitle.ToLower())).Select(p => p.Item.Title));

            FilterTitles.Clear();
            foreach (var item in Titles.Where(p => petGenders.Contains(p.Item.Title)))
            {
                FilterTitles.Add(item);
                FilterTitles.Last().IsSelected = item.IsSelected;
            }
        }
    }
}
