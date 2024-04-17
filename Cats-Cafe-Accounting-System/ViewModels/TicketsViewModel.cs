using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class TicketsViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        public bool IsEnabled { get; set; }

        private ObservableCollection<ElemTicket> items = new ObservableCollection<ElemTicket>();
        public ObservableCollection<ElemTicket> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<ElemTicket> filterItems = new ObservableCollection<ElemTicket>();
        public ObservableCollection<ElemTicket> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }

        public ICommand AddTicketCommand { get; set; }
        public ICommand UpdateTicketCommand { get; set; }
        public ICommand DeleteTicketCommand { get; set; }
        public ICommand DeleteManyTicketCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }

        public TicketsViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Tickets.Include(p => p.Pet).ToList())
            {
                Items.Add(new ElemTicket(item.Clone() as TicketModel));
                FilterItems.Add(new ElemTicket(item.Clone() as TicketModel));
            }
            FilterItems.Add(new ElemTicket(new TicketModel() { Price = 0, PetId = null, Pet = null, Comments = "" }));
            AddTicketCommand = new RelayCommand(ExecuteAddTicketCommand);
            UpdateTicketCommand = new RelayCommand<TicketModel>(ExecuteUpdateTicketCommand);
            DeleteTicketCommand = new RelayCommand<TicketModel>(ExecuteDeleteTicketCommand);
            DeleteManyTicketCommand = new RelayCommand(ExecuteDeleteManyTicketCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            ElemUpdatedCommand = new RelayCommand<ElemTicket>(ExecuteElemUpdatedCommand);
        }

        public void ExecuteAddTicketCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastTicket = FilterItems[FilterItems.Count - 1].Item;
            var ticketToAdd = new TicketModel();
            try
            {
                ticketToAdd = new TicketModel
                {
                    Price = lastTicket.Price,
                    Pet = null,
                    PetId = null,
                    Comments = lastTicket.Comments,
                };

                _dbContext.Tickets.Add(ticketToAdd.Clone() as TicketModel);
                _dbContext.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Введите все данные билета");
                return;
            }
            ticketToAdd.Id = _dbContext.Tickets.First(p => p.Comments == ticketToAdd.Comments).Id;
            Items.Add(new ElemTicket(ticketToAdd.Clone() as TicketModel));
            FilterItems.First(x => x.Item.Comments == ticketToAdd.Comments).Item.Id = ticketToAdd.Id;
            UpdateTable();
        }

        public void ExecuteUpdateTicketCommand(TicketModel? ticket)
        {
            // сделать недоступной кнопку пока не добавлен элемент (последний)
            try
            {
                TicketModel old = _dbContext.Tickets.First(p => p.Id == ticket.Id);
                _dbContext.Tickets.Update(TicketModel.Update(old, ticket));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.Tickets.FirstOrDefault(p => p.Id == ticket.Id) != null)
                    MessageBox.Show("Введите все данные билета");
                return;
            }
            FilterItems.First(p => p.Item.Id == ticket.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeleteTicketCommand(TicketModel? ticket)
        {
            if (FilterItems.Count > 1)
            {
                try
                {
                    _dbContext.Tickets.Remove(ticket);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    return;
                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.Comments = "";
                FilterItems[FilterItems.Count - 1].Item.Price = 0;
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyTicketCommand()
        {
            var itemsToDelete = new ObservableCollection<ElemTicket>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count - 1)
                {
                    TicketModel ticket = _dbContext.Tickets.First(p => p.Id == itemToDelete.Item.Id);
                    _dbContext.Tickets.Remove(ticket);
                    _dbContext.SaveChanges();
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.Comments = "";
                    FilterItems[FilterItems.Count - 1].Item.Price = 0;
                }
            }
            UpdateTable();
        }

        public void UpdateTable()
        {
            FilterItems.Add(new ElemTicket(new TicketModel() { Price = 0, PetId = null, Pet = null, Comments = "" }));
        }

        public void ExecuteChangeSelectionCommand(bool value)
        {
            foreach (var item in FilterItems)
                item.IsSelected = value;
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
            table.GetRow(0).GetCell(0).SetText("Цена билета (руб)");
            table.GetRow(0).GetCell(1).SetText("Имя питомца");
            table.GetRow(0).GetCell(2).SetText("Комментарии");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                TicketModel ticket = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(ticket.Price.ToString());
                table.GetRow(i + 1).GetCell(1).SetText(ticket.Pet == null ? "NULL" : ticket.Pet.Name);
                table.GetRow(i + 1).GetCell(2).SetText(ticket.Comments);
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
                    var worksheet = workbook.Worksheets.Add("Билеты");
                    List<string> headers = new List<string>() { "Цена", "Имя питомца", "Комментарии" };
                    List<List<string>> tickets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        tickets.Add(new List<string>() { FilterItems[i].Item.Price.ToString(), FilterItems[i].Item.Pet == null ? "NULL" : FilterItems[i].Item.Pet.Name, FilterItems[i].Item.Comments });
                    }
                    worksheet.Cell("A1").InsertTable(tickets);
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

        public void ExecuteElemUpdatedCommand(ElemTicket ticketUpdated)
        {
            if (_dbContext.Tickets.FirstOrDefault(p => p.Equals(ticketUpdated.Item)) == null || !ticketUpdated.Item.Equals(Items.First(p => p.Item.Id == ticketUpdated.Item.Id).Item))
            {
                ticketUpdated.IsUpdated = true;
            }
        }
    }
}
