using Cats_Cafe_Accounting_System.Models;
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
    public class PopularPetsViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        private ObservableCollection<Elem<PopularPetsModel>> items = new ObservableCollection<Elem<PopularPetsModel>>();
        public ObservableCollection<Elem<PopularPetsModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<Elem<PopularPetsModel>> filterItems = new ObservableCollection<Elem<PopularPetsModel>>();
        public ObservableCollection<Elem<PopularPetsModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
            }
        }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }

        public PopularPetsViewModel(ApplicationDbContext contex)
        {
            _dbContext = contex;
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            int i = 1;
            var sortedItems = _dbContext.VisitLogEntries
                .Include(t => t.Ticket)
                .Where(t => t.Ticket.PetId != null)
                .GroupBy(g => g.TicketId)
                .ToList()
                .OrderByDescending(x => x.Count());

            foreach (var item in sortedItems)
            {
                List<VisitLogEntryModel> visits = item.ToList();
                int score = item.Sum(visit => visit.TicketsCount);
                PopularPetsModel model = new PopularPetsModel() { Pet = item.First().Ticket.Pet, PetTransferLogEntryModels = visits, Place = i, Score = score };
                Items.Add(new Elem<PopularPetsModel>(model));
                FilterItems.Add(new Elem<PopularPetsModel>(model));
                i++;
            }
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
            table.GetRow(0).GetCell(0).SetText("Место");
            table.GetRow(0).GetCell(1).SetText("Кличка");
            table.GetRow(0).GetCell(2).SetText("Балл");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                PopularPetsModel pet = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(pet.Place.ToString());
                table.GetRow(i + 1).GetCell(1).SetText(pet.Pet.Name);
                table.GetRow(i + 1).GetCell(2).SetText(pet.Score.ToString());
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
                    var worksheet = workbook.Worksheets.Add("Популярные питомцы");
                    List<string> headers = new List<string>() { "Место", "Кличка", "Балл" };
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Place.ToString(), FilterItems[i].Item.Pet.Name, FilterItems[i].Item.Score.ToString() });
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
    }
}
