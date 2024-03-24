using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class JobsViewModel : ObservableObject
    {
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

        private ObservableCollection<FilterElem<JobModel>> selectedTitles = new ObservableCollection<FilterElem<JobModel>>();
        public ObservableCollection<FilterElem<JobModel>> SelectedTitles
        {
            get { return selectedTitles; }
            set
            {
                selectedTitles = value;
                OnPropertyChanged(nameof(SelectedTitles));
            }
        }

        public ICommand AddJobCommand { get; set; }
        public ICommand UpdateJobCommand { get; set; }
        public ICommand DeleteJobCommand { get; set; }
        public ICommand DeleteManyJobsCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchTitleCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public JobsViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            foreach (var item in _dbContext.Jobs)
            {
                Items.Add(new Elem<JobModel>(item));
                SelectedTitles.Add(new FilterElem<JobModel>(item));
            }
            Items.Add(new Elem<JobModel>(new JobModel()));
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddJobCommand = new RelayCommand(ExecuteAddJobCommand);
            UpdateJobCommand = new RelayCommand<JobModel>(ExecuteUpdateJobCommand);
            DeleteJobCommand = new RelayCommand<JobModel>(ExecuteDeleteJobCommand);
            DeleteManyJobsCommand = new RelayCommand(ExecuteDeleteManyJobsCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            //SearchNameCommand = new RelayCommand(ExecuteSearchNameCommand);
        }

        public void ExecuteAddJobCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastJob = Items[Items.Count - 1].Item;

            var jobToAdd = new JobModel
            {
                Title = lastJob.Title,
                Rate = lastJob.Rate,
            };

            _dbContext.Jobs.Add(jobToAdd);
            _dbContext.SaveChanges();
            SelectedTitles.Add(new FilterElem<JobModel>(jobToAdd));
            UpdateTable();
        }

        private void ExecuteUpdateJobCommand(JobModel? job)
        {
            _dbContext.Jobs.Update(job);
            _dbContext.SaveChanges();
            // при удалении, изменении и тд нужно менять значения внутри коллекций
            UpdateTable();
        }

        private void ExecuteDeleteJobCommand(JobModel? job)
        {
            _dbContext.Jobs.Remove(job);
            _dbContext.SaveChanges();
            UpdateTable();
        }

        private void ExecuteDeleteManyJobsCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<JobModel>>(Items.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                _dbContext.Jobs.Remove(itemToDelete.Item);
            }
            _dbContext.SaveChanges();
            UpdateTable();

        }

        private void ExecuteWordExportCommand()
        {
            // Создание нового документа Word
            XWPFDocument document = new XWPFDocument();

            // Создание таблицы
            XWPFTable table = document.CreateTable(Items.Count + 1, 2);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Title");
            table.GetRow(0).GetCell(1).SetText("Rate");

            // Заполнение данных о питомцах
            for (int i = 0; i < Items.Count; i++)
            {
                JobModel job = Items[i].Item;

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

        private void ExecuteExcelExportCommand()
        {
            using (var workbook = new XLWorkbook())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.ShowDialog();
                saveFileDialog.DefaultExt = ".xlsx";
                var worksheet = workbook.Worksheets.Add("Pets");
                worksheet.Cell("A1").InsertTable(Items);
                string path = saveFileDialog.FileName + ".xlsx";
                workbook.SaveAs(path);
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }

        public void ExecuteChangeSelectionCommand(bool value)
        {
            foreach (var item in Items)
                item.IsSelected = value;
        }

        public void ExecuteFilterCommand()
        {
            UpdateTable();
        }

        public void UpdateTable()
        {
            var jobTitles = new ObservableCollection<string>(SelectedTitles.Where(p => p.IsSelected).Select(p => p.Item.Title));

            var filteredJobs = _dbContext.Jobs
                .Where(p => jobTitles.Contains(p.Title))
                .ToList();

            Items.Clear();
            foreach (var item in filteredJobs)
                Items.Add(new Elem<JobModel>(item));
            Items.Add(new Elem<JobModel>(new JobModel()));
        }

        public void ExecuteSearchNameCommand()
        {
            var jobTitles = new ObservableCollection<string>(SelectedTitles.Where(p => p.Item.Title.Contains(SearchTitle)).Select(p => p.Item.Title));
            // нужно доп коллекцию или что-то подобное для поиска
        }
    }
}
