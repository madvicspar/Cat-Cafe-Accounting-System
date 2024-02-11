using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PetsViewModel : ObservableObject
    {
        private ObservableCollection<PetModel> pets;
        public ObservableCollection<PetModel> Pets
        {
            get { return pets; }
            set
            {
                pets = value;
                OnPropertyChanged(nameof(Pets));
            }
        }
        public ICommand AddPetCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public PetsViewModel()
        {
            // Инициализация коллекции питомцев
            Pets = GetPetsFromTable("pets");
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddPetCommand = new RelayCommand(ExecuteAddPetCommand);
        }

        public void ExecuteAddPetCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastPet = pets[pets.Count - 1];

            Gender g = Data.gendersList.Where(x => x.Title == lastPet.Gender.Title).FirstOrDefault();
            Breed b = Data.breedsList.Where(x => x.Title == lastPet.Breed.Title).FirstOrDefault();
            Status s = Data.statusesList.Where(x => x.Title == lastPet.Status.Title).FirstOrDefault();

            var petToAdd = new PetModel
            {
                Name = lastPet.Name,
                Gender = g,
                GenderId = g.Id,
                Status = s,
                StatusId = s.Id,
                Breed = b,
                BreedId = b.Id,
                Birthday = lastPet.Birthday,
                CheckInDate = lastPet.CheckInDate,
                PassNumber = lastPet.PassNumber
            };

            DBContext.AddNote("pets", petToAdd);
        }

        private void ExecuteWordExportCommand()
        {
            // Создание нового документа Word
            XWPFDocument document = new XWPFDocument();

            // Создание таблицы
            XWPFTable table = document.CreateTable(pets.Count + 1, 8);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Name");
            table.GetRow(0).GetCell(1).SetText("Breed");
            table.GetRow(0).GetCell(2).SetText("Gender");
            table.GetRow(0).GetCell(3).SetText("Status");
            table.GetRow(0).GetCell(4).SetText("Breed");
            table.GetRow(0).GetCell(5).SetText("Дата рождения");
            table.GetRow(0).GetCell(6).SetText("Дата появления в котокафе");
            table.GetRow(0).GetCell(7).SetText("Номер паспорта");

            // Заполнение данных о питомцах
            for (int i = 0; i < pets.Count; i++)
            {
                PetModel pet = pets[i];

                table.GetRow(i + 1).GetCell(0).SetText(pet.Name);
                table.GetRow(i + 1).GetCell(1).SetText(pet.Breed.Title);
                table.GetRow(i + 1).GetCell(2).SetText(pet.Gender.Title);
                table.GetRow(i + 1).GetCell(3).SetText(pet.Status.Title);
                table.GetRow(i + 1).GetCell(4).SetText(pet.Breed.Title);
                table.GetRow(i + 1).GetCell(5).SetText(pet.Birthday.ToString());
                table.GetRow(i + 1).GetCell(6).SetText(pet.CheckInDate.ToString());
                table.GetRow(i + 1).GetCell(7).SetText(pet.PassNumber);
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
                worksheet.Cell("A1").InsertTable(pets);
                string path = saveFileDialog.FileName + ".xlsx";
                workbook.SaveAs(path);
                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }

        public static ObservableCollection<PetModel> GetPetsFromTable(string table)
        {
            ObservableCollection<PetModel> pets = new ObservableCollection<PetModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                PetModel pet = new PetModel(Convert.ToInt32(row["id"]), row["name"].ToString(),
                    Convert.ToInt32(row["genderid"]), Convert.ToInt32(row["statusid"]), row["breedid"].ToString(), 
                    DateTime.Parse(row["birthday"].ToString()), DateTime.Parse(row["checkindate"].ToString()), 
                    row["passnumber"].ToString());
                pets.Add(pet);
            }

            return pets;
        }
    }
}
