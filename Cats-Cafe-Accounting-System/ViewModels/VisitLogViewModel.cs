using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Office2010.Excel;
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
    public class VisitLogViewModel : ObservableObject
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
        private ObservableCollection<Elem<VisitLogEntryModel>> items = new ObservableCollection<Elem<VisitLogEntryModel>>();
        public ObservableCollection<Elem<VisitLogEntryModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<VisitLogEntryModel>> filterItems = new ObservableCollection<Elem<VisitLogEntryModel>>();
        public ObservableCollection<Elem<VisitLogEntryModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        private ObservableCollection<FilterElem<VisitLogEntryModel>> visitors = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
        public ObservableCollection<FilterElem<VisitLogEntryModel>> Visitors
        {
            get { return visitors; }
            set
            {
                visitors = value;
                OnPropertyChanged(nameof(Visitors));
            }
        }

        private ObservableCollection<FilterElem<VisitLogEntryModel>> tickets = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
        public ObservableCollection<FilterElem<VisitLogEntryModel>> Tickets
        {
            get { return tickets; }
            set
            {
                tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }

        private ObservableCollection<FilterElem<VisitLogEntryModel>> filterVisitors = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
        public ObservableCollection<FilterElem<VisitLogEntryModel>> FilterVisitors
        {
            get { return filterVisitors; }
            set
            {
                filterVisitors = value;
                OnPropertyChanged(nameof(FilterVisitors));
            }
        }

        private ObservableCollection<FilterElem<VisitLogEntryModel>> filterTickets = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
        public ObservableCollection<FilterElem<VisitLogEntryModel>> FilterTickets
        {
            get { return filterTickets; }
            set
            {
                filterTickets = value;
                OnPropertyChanged(nameof(FilterTickets));
            }
        }

        public ICommand AddEntryCommand { get; set; }
        public ICommand UpdateEntryCommand { get; set; }
        public ICommand DeleteEntryCommand { get; set; }
        public ICommand DeleteManyEntryCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ChangeVisitorSelectionCommand { get; set; }
        public ICommand ChangeTicketSelectionCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxVisitorSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxTicketSelectionCommand { get; set; }
        public ICommand DeleteDateFiltersCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }
        public VisitLogViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.VisitLogEntries.Include(p => p.Visitor).Include(t => t.Ticket).ToList())
            {
                FilterItems.Add(new Elem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                Items.Add(new Elem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                Visitors.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                Tickets.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                if (FilterVisitors.Count > 0)
                {
                    if (!FilterVisitors.Any(p => p.Item.Visitor.Equals(item.Visitor)))
                    {
                        FilterVisitors.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                    }
                    if (!FilterTickets.Any(p => p.Item.Ticket.Equals(item.Ticket)))
                    {
                        FilterTickets.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                    }
                }
                else
                {
                    FilterVisitors.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                    FilterTickets.Add(new FilterElem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
                }
            }
            FilterItems.Add(new Elem<VisitLogEntryModel>(new VisitLogEntryModel() { Date = DateTime.Today, StartTime = TimeOnly.FromDateTime(DateTime.Now) }));
            ExecuteDeleteDateFiltersCommand();
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddEntryCommand = new RelayCommand(ExecuteAddEntryCommand);
            UpdateEntryCommand = new RelayCommand<VisitLogEntryModel>(ExecuteUpdateEntryCommand);
            DeleteEntryCommand = new RelayCommand<VisitLogEntryModel>(ExecuteDeleteEntryCommand);
            DeleteManyEntryCommand = new RelayCommand(ExecuteDeleteManyEntryCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ChangeVisitorSelectionCommand = new RelayCommand<bool>(ExecuteChangeVisitorSelectionCommand);
            ChangeTicketSelectionCommand = new RelayCommand<bool>(ExecuteChangeTicketSelectionCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            UpdateCheckBoxSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxSelectionCommand);
            UpdateCheckBoxVisitorSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxVisitorSelectionCommand);
            UpdateCheckBoxTicketSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxTicketSelectionCommand);
            DeleteDateFiltersCommand = new RelayCommand(ExecuteDeleteDateFiltersCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<VisitLogEntryModel>>(ExecuteElemUpdatedCommand);
        }
        public void ExecuteDeleteDateFiltersCommand()
        {
            DateTime birtdayMin = FilterItems.MinBy(item => item.Item.Date).Item.Date;
            DateTime birtdayMax = FilterItems.MaxBy(item => item.Item.Date).Item.Date;
            StartDate = birtdayMin;
            EndDate = birtdayMax;
        }

        public void ExecuteElemUpdatedCommand(Elem<VisitLogEntryModel> entryUpdated)
        {
            if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Equals(entryUpdated.Item)) == null || !entryUpdated.Item.Equals(Items.First(p => p.Item.Id == entryUpdated.Item.Id).Item))
            {
                entryUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddEntryCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastEntry = FilterItems[FilterItems.Count - 1].Item;
            var entryToAdd = new VisitLogEntryModel();
            try
            {
                entryToAdd = new VisitLogEntryModel
                {
                    Date = lastEntry.Date,
                    StartTime = lastEntry.StartTime,
                    VisitorId = lastEntry.Visitor.Id,
                    Visitor = lastEntry.Visitor,
                    TicketId = lastEntry.Ticket.Id,
                    Ticket = lastEntry.Ticket,
                    TicketsCount = lastEntry.TicketsCount
                };

                _dbContext.VisitLogEntries.Add(entryToAdd.Clone() as VisitLogEntryModel);
                _dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введите все данные посещения");
                return;
            }
            entryToAdd.Id = _dbContext.VisitLogEntries.First(p => p.Visitor == entryToAdd.Visitor).Id;
            if (Visitors.FirstOrDefault(p => p.Item.Visitor == entryToAdd.Visitor) == null)
            {
                Visitors.Add(new FilterElem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
                FilterVisitors.Add(new FilterElem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
                Tickets.Add(new FilterElem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
                FilterTickets.Add(new FilterElem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
                Items.Add(new Elem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
                FilterItems.Add(new Elem<VisitLogEntryModel>(entryToAdd.Clone() as VisitLogEntryModel));
            }
            UpdateTable();
        }
        /// <summary>
        /// Обновление VisitLogEntryModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="entry"> Измененный VisitLogEntryModel </param>
        public void ExecuteUpdateEntryCommand(VisitLogEntryModel? entry)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)
            string name = "";
            try
            {
                VisitLogEntryModel old = _dbContext.VisitLogEntries.First(p => p.Id == entry.Id);
                name = old.Visitor.LastName;
                _dbContext.VisitLogEntries.Update(VisitLogEntryModel.Update(old, entry));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Id == entry.Id) != null)
                    MessageBox.Show("Введите все данные посещения");
                return;
            }
            if (entry.Visitor.LastName != name)
            {
                Visitors.First(p => p.Item.Visitor.LastName == name).Item.Visitor = entry.Visitor;
                FilterVisitors.First(p => p.Item.Id == entry.Id).Item.Visitor = entry.Visitor;
                Tickets.Add(new FilterElem<VisitLogEntryModel>(entry.Clone() as VisitLogEntryModel));
                FilterTickets.Add(new FilterElem<VisitLogEntryModel>(entry.Clone() as VisitLogEntryModel));
            }
            FilterItems.First(p => p.Item.Id == entry.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteEntryCommand(VisitLogEntryModel? entry)
        {
            if (FilterItems.Count > 1)
            {
                string name = "";
                int id = 0;
                try
                {
                    name = _dbContext.VisitLogEntries.First(p => p == entry).Visitor.LastName;
                    id = _dbContext.VisitLogEntries.First(p => p == entry).TicketId;
                    _dbContext.VisitLogEntries.Remove(entry);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    MessageBox.Show("Введите все данные записи");
                    return;
                }
                if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Visitor.LastName == name) == null)
                {
                    Visitors.Remove(Visitors.First(p => p.Item.Visitor == entry.Visitor));
                    FilterVisitors.Remove(FilterVisitors.First(p => p.Item.Visitor == entry.Visitor));

                }
                if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Ticket.Id == id) == null)
                {
                    Tickets.Remove(Visitors.First(p => p.Item.Ticket == entry.Ticket));
                    FilterTickets.Remove(FilterVisitors.First(p => p.Item.Ticket == entry.Ticket));

                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.TicketsCount = 1;
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyEntryCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<VisitLogEntryModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    VisitLogEntryModel entry = _dbContext.VisitLogEntries.First(p => p.Id == itemToDelete.Item.Id);
                    _dbContext.VisitLogEntries.Remove(entry);
                    _dbContext.SaveChanges();
                    if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Visitor == itemToDelete.Item.Visitor) == null)
                    {
                        Visitors.Remove(Visitors.First(p => p.Item.Visitor == itemToDelete.Item.Visitor));
                        FilterVisitors.Remove(FilterVisitors.First(p => p.Item.Visitor == itemToDelete.Item.Visitor));
                    }
                    if (_dbContext.VisitLogEntries.FirstOrDefault(p => p.Ticket == itemToDelete.Item.Ticket) == null)
                    {
                        Tickets.Remove(Visitors.First(p => p.Item.Ticket == itemToDelete.Item.Ticket));
                        FilterTickets.Remove(FilterVisitors.First(p => p.Item.Ticket == itemToDelete.Item.Ticket));
                    }
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.TicketsCount = 1;
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
            table.GetRow(0).GetCell(1).SetText("Время начала");
            table.GetRow(0).GetCell(2).SetText("ФИО посетителя");
            table.GetRow(0).GetCell(3).SetText("Комментарий к билету");
            table.GetRow(0).GetCell(4).SetText("Количествр билетов");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                VisitLogEntryModel entry = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(entry.Date.ToString());
                table.GetRow(i + 1).GetCell(1).SetText(entry.StartTime.ToString());
                table.GetRow(i + 1).GetCell(2).SetText(entry.Visitor.LastName + entry.Visitor.FirstName + entry.Visitor.Pathronymic);
                table.GetRow(i + 1).GetCell(3).SetText(entry.Ticket.Comments);
                table.GetRow(i + 1).GetCell(4).SetText(entry.TicketsCount.ToString());
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
                    List<string> headers = new List<string>() { "Дата", "Время начала", "ФИО посетителя", "Комментарий к билету", "Количество билетов" };
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Date.ToString(), FilterItems[i].Item.StartTime.ToString(), FilterItems[i].Item.Visitor.LastName + FilterItems[i].Item.Visitor.FirstName + FilterItems[i].Item.Visitor.Pathronymic, FilterItems[i].Item.Ticket.Comments, FilterItems[i].Item.TicketsCount.ToString() });
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

        public void ExecuteChangeVisitorSelectionCommand(bool value)
        {
            foreach (var item in FilterVisitors)
                item.IsSelected = !value;
        }

        public void ExecuteChangeTicketSelectionCommand(bool value)
        {
            foreach (var item in FilterTickets)
                item.IsSelected = !value;
        }

        public void ExecuteFilterCommand()
        {
            foreach (var item in Visitors)
            {
                var visitor = GetWithoutVisitorFilter().FirstOrDefault(p => p.Item.Visitor == item.Item.Visitor);
                if (visitor is not null)
                {
                    item.IsSelected = visitor.IsSelected;
                }
            }
            foreach (var item in Tickets)
            {
                var ticket = GetWithoutTicketFilter().FirstOrDefault(p => p.Item.Ticket == item.Item.Ticket);
                if (ticket is not null)
                {
                    item.IsSelected = ticket.IsSelected;
                }
            }
            UpdateTable();
        }

        public void UpdateTable()
        {
            var visitors = new ObservableCollection<VisitLogEntryModel>(Visitors.Where(p => p.IsSelected).Select(p => p.Item));
            var tickets = new ObservableCollection<VisitLogEntryModel>(Tickets.Where(p => p.IsSelected).Select(p => p.Item));
            var petSearched = FilterItems.Select(p => p.Item);

            var filteredPets = _dbContext.VisitLogEntries
                //.Where(p => petSearched.Contains(p))
                .Where(p => visitors.Contains(p))
                .Where(p => tickets.Contains(p))
                .Where(p => p.Date >= StartDate && p.Date <= EndDate)
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredPets)
                FilterItems.Add(new Elem<VisitLogEntryModel>(item.Clone() as VisitLogEntryModel));
            FilterItems.Add(new Elem<VisitLogEntryModel>(new VisitLogEntryModel() { Date = DateTime.Today, StartTime = TimeOnly.FromDateTime(DateTime.Now) }));
        }

        public void ExecuteUpdateCheckBoxSelectionCommand()
        {
            foreach (var item in FilterVisitors)
                Visitors.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxVisitorSelectionCommand()
        {
            foreach (var item in FilterVisitors)
                Visitors.First(p => p.Item.Visitor == item.Item.Visitor).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxTicketSelectionCommand()
        {
            foreach (var item in FilterTickets)
                Tickets.First(p => p.Item.Ticket == item.Item.Ticket).IsSelected = item.IsSelected;
        }

        public ObservableCollection<FilterElem<VisitLogEntryModel>> GetWithoutVisitorFilter()
        {
            var collection = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
            foreach (var item in Visitors)
            {
                if (!collection.Any(p => p.Item.Visitor == item.Item.Visitor))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        public ObservableCollection<FilterElem<VisitLogEntryModel>> GetWithoutTicketFilter()
        {
            var collection = new ObservableCollection<FilterElem<VisitLogEntryModel>>();
            foreach (var item in Tickets)
            {
                if (!collection.Any(p => p.Item.Ticket == item.Item.Ticket))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }
    }
}
