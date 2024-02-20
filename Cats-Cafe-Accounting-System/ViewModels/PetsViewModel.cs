using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class Elem : ObservableObject
    {
        public bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public PetModel Pet { get; set; }
        public Elem(PetModel pet)
        {
            IsSelected = false;
            Pet = pet;
        }
        public Elem()
        {
            IsSelected = false;
            Pet = new PetModel();
        }
    }

    public class FilterElem<T> : ObservableObject
    {
        public bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public T Item { get; set; }
        public FilterElem(T item)
        {
            IsSelected = true;
            Item = item;
        }
    }

    public class FilterElem : ObservableObject
    {
        public string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public List<string> SelectedItems { get; set; }
        public FilterElem(string _name, List<string> _items)
        {
            Name = _name;
            SelectedItems = _items;
        }
    }

    public class PetsViewModel : ObservableObject
    {
        private ObservableCollection<Elem> items = new ObservableCollection<Elem>();
        public ObservableCollection<Elem> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
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

        private ObservableCollection<FilterElem> filters = new ObservableCollection<FilterElem>() { new FilterElem("Name", new List<string>()),  new FilterElem("GenderId", new List<string>()), new FilterElem("BreedId", new List<string>()), new FilterElem("StatusId", new List<string>()) };
        public ObservableCollection<FilterElem> Filters
        {
            get { return filters; }
            set
            {
                filters = value;
                OnPropertyChanged(nameof(Filters));
            }
        }

        private ObservableCollection<FilterElem<string>> selectedNames = new ObservableCollection<FilterElem<string>>() { new FilterElem<string>("Все клички") };
        public ObservableCollection<FilterElem<string>> SelectedNames
        {
            get { return selectedNames; }
            set
            {
                selectedNames = value;
                OnPropertyChanged(nameof(SelectedNames));
            }
        }

        private ObservableCollection<FilterElem<string>> selectedGenders = new ObservableCollection<FilterElem<string>>() { new FilterElem<string>("Все гендеры") };
        public ObservableCollection<FilterElem<string>> SelectedGenders
        {
            get { return selectedGenders; }
            set
            {
                selectedGenders = value;
                OnPropertyChanged(nameof(SelectedGenders));
            }
        }

        private ObservableCollection<FilterElem<string>> selectedBreeds = new ObservableCollection<FilterElem<string>>() { new FilterElem<string>("Все породы") };
        public ObservableCollection<FilterElem<string>> SelectedBreeds
        {
            get { return selectedBreeds; }
            set
            {
                selectedBreeds = value;
                OnPropertyChanged(nameof(SelectedBreeds));
            }
        }

        private ObservableCollection<FilterElem<string>> selectedStatuses = new ObservableCollection<FilterElem<string>> () { new FilterElem<string>("Все статусы") };
        public ObservableCollection<FilterElem<string>> SelectedStatuses
        {
            get { return selectedStatuses; }
            set
            {
                selectedStatuses = value;
                OnPropertyChanged(nameof(SelectedStatuses));
            }
        }

        public ICommand AddPetCommand { get; set; }
        public ICommand UpdatePetCommand { get; set; }
        public ICommand DeletePetCommand { get; set; }
        public ICommand DeleteManyPetCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand NameFilterCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public PetsViewModel()
        {
            // Инициализация коллекции питомцев
            Pets = GetPetsFromTable("pets", false);
            foreach (var item in Pets)
            {
                items.Add(new Elem(item));
            }
            foreach (var item in Data.gendersList)
                SelectedGenders.Add(new FilterElem<string>(item.Title));
            foreach (var item in Data.breedsList)
                SelectedBreeds.Add(new FilterElem<string>(item.Title));
            foreach (var item in Data.statusesList)
                SelectedStatuses.Add(new FilterElem<string>(item.Title));
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddPetCommand = new RelayCommand(ExecuteAddPetCommand);
            UpdatePetCommand = new RelayCommand<PetModel>(ExecuteUpdatePetCommand);
            DeletePetCommand = new RelayCommand<PetModel>(ExecuteDeletePetCommand);
            DeleteManyPetCommand = new RelayCommand(ExecuteDeleteManyPetCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            NameFilterCommand = new RelayCommand(ExecuteNameFilterCommand);
        }

        public void ExecuteAddPetCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastPet = items[items.Count - 1].Pet;

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

        private void ExecuteUpdatePetCommand(PetModel? pet)
        {
            DBContext.UpdateNote("pets", pet);
        }

        private void ExecuteDeletePetCommand(PetModel? pet)
        {
            DBContext.DeleteNote("pets", pet.Id.ToString());
            pets.Remove(pet);
        }

        private void ExecuteDeleteManyPetCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem>();
            foreach (var item in items.Where(x => x.IsSelected))
            {
                DBContext.DeleteNote("pets", item.Pet.Id.ToString());
                pets.Remove(item.Pet);
                itemsToDelete.Add(item);
            }
            foreach (var item in itemsToDelete)
            {
                items.Remove(item);
            }
            itemsToDelete.Clear();
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

        public ObservableCollection<PetModel> GetPetsFromTable(string table, bool isFilter)
        {
            ObservableCollection<PetModel> pets = new ObservableCollection<PetModel>();

            DataTable dataTable = DBContext.GetTable(table, filters.Select(x => x.SelectedItems).ToList(), isFilter);

            foreach (DataRow row in dataTable.Rows)
            {
                PetModel pet = new PetModel(Convert.ToInt32(row["id"]), row["name"].ToString(),
                    Convert.ToInt32(row["genderid"]), Convert.ToInt32(row["statusid"]), row["breedid"].ToString(), 
                    DateTime.Parse(row["birthday"].ToString()), DateTime.Parse(row["checkindate"].ToString()), 
                    row["passnumber"].ToString());
                pets.Add(pet);
                SelectedNames.Add(new FilterElem<string>(pet.Name));
            }

            return pets;
        }

        public void ExecuteChangeSelectionCommand(bool value)
        {
            foreach (var item in Items)
                item.IsSelected = value;
        }

        public void ExecuteNameFilterCommand()
        {
            filters[0].SelectedItems = SelectedNames.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            filters[1].SelectedItems = SelectedGenders.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            filters[3].SelectedItems = SelectedStatuses.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            filters[2].SelectedItems = SelectedBreeds.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();

            Pets = GetPetsFromTable("pets", false);
            foreach (var item in Pets)
            {
                items.Add(new Elem(item));
            }
        }
    }
}
