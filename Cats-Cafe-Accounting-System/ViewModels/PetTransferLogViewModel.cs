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
    public class PetTransferLogViewModel : ObservableObject
    {
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
        private ObservableCollection<Elem<PetTransferLogEntryModel>> items = new ObservableCollection<Elem<PetTransferLogEntryModel>>();
        public ObservableCollection<Elem<PetTransferLogEntryModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<PetTransferLogEntryModel>> filterItems = new ObservableCollection<Elem<PetTransferLogEntryModel>>();
        public ObservableCollection<Elem<PetTransferLogEntryModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        public ICommand AddEntryCommand { get; set; }
        public ICommand UpdateEntryCommand { get; set; }
        public ICommand DeleteEntryCommand { get; set; }
        public ICommand DeleteManyEntryCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand DeleteDateFiltersCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }
        public ICommand FilterCommand { get; set; }

        public PetTransferLogViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.PetTransferLogEntries.Include(p => p.Pet).Include(v => v.Visitor).ToList())
            {
                FilterItems.Add(new Elem<PetTransferLogEntryModel>(item.Clone() as PetTransferLogEntryModel));
                Items.Add(new Elem<PetTransferLogEntryModel>(item.Clone() as PetTransferLogEntryModel));
            }
            FilterItems.Add(new Elem<PetTransferLogEntryModel>(new PetTransferLogEntryModel() { Date = DateTime.Today }));
            ExecuteDeleteDateFiltersCommand();
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddEntryCommand = new RelayCommand(ExecuteAddEntryCommand);
            UpdateEntryCommand = new RelayCommand<PetTransferLogEntryModel>(ExecuteUpdateEntryCommand);
            DeleteEntryCommand = new RelayCommand<PetTransferLogEntryModel>(ExecuteDeleteEntryCommand);
            DeleteManyEntryCommand = new RelayCommand(ExecuteDeleteManyEntryCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            DeleteDateFiltersCommand = new RelayCommand(ExecuteDeleteDateFiltersCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<PetTransferLogEntryModel>>(ExecuteElemUpdatedCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
        }

        private void ExecuteFilterCommand()
        {
            UpdateTable();
        }

        public void ExecuteDeleteDateFiltersCommand()
        {
            DateTime birtdayMin = FilterItems.MinBy(item => item.Item.Date).Item.Date;
            DateTime birtdayMax = FilterItems.MaxBy(item => item.Item.Date).Item.Date;
            StartDate = birtdayMin;
            EndDate = birtdayMax;
        }

        public void ExecuteElemUpdatedCommand(Elem<PetTransferLogEntryModel> entryUpdated)
        {
            if (_dbContext.PetTransferLogEntries.FirstOrDefault(p => p.Equals(entryUpdated.Item)) == null || !entryUpdated.Item.Equals(Items.First(p => p.Item.Id == entryUpdated.Item.Id).Item))
            {
                entryUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddEntryCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastEntry = FilterItems[FilterItems.Count - 1].Item;
            var entryToAdd = new PetTransferLogEntryModel();
            try
            {
                entryToAdd = new PetTransferLogEntryModel
                {
                    Date = lastEntry.Date,
                    PetId = lastEntry.Pet.Id,
                    Pet = lastEntry.Pet,
                    VisitorId = lastEntry.VisitorId,
                    Visitor = lastEntry.Visitor
                };

                _dbContext.PetTransferLogEntries.Add(entryToAdd.Clone() as PetTransferLogEntryModel);
                _dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введите все данные записи");
                return;
            }
            entryToAdd.Id = _dbContext.PetTransferLogEntries.First(p => p.Pet.Name == entryToAdd.Pet.Name).Id;
            Items.Add(new Elem<PetTransferLogEntryModel>(entryToAdd.Clone() as PetTransferLogEntryModel));
            FilterItems.Add(new Elem<PetTransferLogEntryModel>(entryToAdd.Clone() as PetTransferLogEntryModel));
            UpdateTable();
        }
        /// <summary>
        /// Обновление PetTransferLogEntryModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="entry"> Измененный PetTransferLogEntryModel </param>
        public void ExecuteUpdateEntryCommand(PetTransferLogEntryModel? entry)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)
            try
            {
                PetTransferLogEntryModel old = _dbContext.PetTransferLogEntries.First(p => p.Id == entry.Id);
                _dbContext.PetTransferLogEntries.Update(PetTransferLogEntryModel.Update(old, entry));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.PetTransferLogEntries.FirstOrDefault(p => p.Id == entry.Id) != null)
                    MessageBox.Show("Введите все данные записи");
                return;
            }
            FilterItems.First(p => p.Item.Id == entry.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteEntryCommand(PetTransferLogEntryModel? entry)
        {
            if (FilterItems.Count > 1)
            {
                _dbContext.PetTransferLogEntries.Remove(entry);
                _dbContext.SaveChanges();
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyEntryCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<PetTransferLogEntryModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    PetTransferLogEntryModel entry = _dbContext.PetTransferLogEntries.First(p => p.Id == itemToDelete.Item.Id);
                    _dbContext.PetTransferLogEntries.Remove(entry);
                    _dbContext.SaveChanges();
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
            table.GetRow(0).GetCell(1).SetText("Кличка питомца");
            table.GetRow(0).GetCell(2).SetText("Фамилия посетителя");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                PetTransferLogEntryModel entry = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(entry.Date.ToString());
                table.GetRow(i + 1).GetCell(1).SetText(entry.Pet.Name);
                table.GetRow(i + 1).GetCell(2).SetText(entry.Visitor.LastName);
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
                    var worksheet = workbook.Worksheets.Add("Передачи питомцев");
                    List<string> headers = new List<string>() { "Дата", "Кличка питомца", "Фамилия посетителя" };
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Date.ToString(), FilterItems[i].Item.Pet.Name, FilterItems[i].Item.Visitor.LastName });
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

        public void UpdateTable()
        {
            var petSearched = FilterItems.Select(p => p.Item);

            var filteredPets = _dbContext.PetTransferLogEntries
                //.Where(p => petSearched.Contains(p))
                .Where(p => p.Date >= StartDate && p.Date <= EndDate)
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredPets)
                FilterItems.Add(new Elem<PetTransferLogEntryModel>(item.Clone() as PetTransferLogEntryModel));
            FilterItems.Add(new Elem<PetTransferLogEntryModel>(new PetTransferLogEntryModel() { Date = DateTime.Today }));
        }
    }
}
