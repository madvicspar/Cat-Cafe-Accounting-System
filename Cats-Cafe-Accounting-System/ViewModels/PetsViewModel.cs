using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
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
        public PetModel pet;
        public PetModel Pet
        {
            get { return pet; }
            set
            {
                pet = value;
                OnPropertyChanged(nameof(Pet));
            }
        }
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
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
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

        private ObservableCollection<Gender> selectedGenders = new ObservableCollection<Gender>();
        public ObservableCollection<Gender> SelectedGenders
        {
            get { return selectedGenders; }
            set
            {
                selectedGenders = value;
                OnPropertyChanged(nameof(SelectedGenders));
            }
        }

        private ObservableCollection<Breed> selectedBreeds = new ObservableCollection<Breed>();
        public ObservableCollection<Breed> SelectedBreeds
        {
            get { return selectedBreeds; }
            set
            {
                selectedBreeds = value;
                OnPropertyChanged(nameof(SelectedBreeds));
            }
        }

        private ObservableCollection<Status> selectedStatuses = new ObservableCollection<Status> ();
        public ObservableCollection<Status> SelectedStatuses
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
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
            {
                Items.Add(new Elem(item));
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                SelectedGenders.Add(item);
            }
            foreach (var item in _dbContext.Breeds.ToList())
            {
                SelectedBreeds.Add(item);
            }
            foreach (var item in _dbContext.Statuses.ToList())
            {
                SelectedStatuses.Add(item);
            }
            Items.Add(new Elem(new PetModel() { Breed = SelectedBreeds[0], Gender = SelectedGenders[0], Status = SelectedStatuses[0], Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
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
            var lastPet = Items[Items.Count - 1].Pet;

            var petToAdd = new PetModel
            {
                Name = lastPet.Name,
                GenderId = lastPet.Gender.Id,
                Gender = lastPet.Gender,
                StatusId = lastPet.Status.Id,
                Status = lastPet.Status,
                BreedId = lastPet.Breed.Id,
                Breed = lastPet.Breed,
                Birthday = lastPet.Birthday,
                CheckInDate = lastPet.CheckInDate,
                PassNumber = lastPet.PassNumber
            };

            _dbContext.Pets.Add(petToAdd);
            _dbContext.SaveChanges();
            Items.Clear();
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
                Items.Add(new Elem(item));
            Items.Add(new Elem(new PetModel() { Breed = SelectedBreeds[0], Gender = SelectedGenders[0], Status = SelectedStatuses[0], Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
        }

        private void ExecuteUpdatePetCommand(PetModel? pet)
        {
            _dbContext.Pets.Update(pet);
            _dbContext.SaveChanges();
            Items.Clear();
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
                Items.Add(new Elem(item));
            Items.Add(new Elem(new PetModel() { Breed = SelectedBreeds[0], Gender = SelectedGenders[0], Status = SelectedStatuses[0], Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
        }

        private void ExecuteDeletePetCommand(PetModel? pet)
        {
            _dbContext.Pets.Remove(pet);
            _dbContext.SaveChanges();
            Items.Clear();
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
                Items.Add(new Elem(item));
            Items.Add(new Elem(new PetModel() { Breed = SelectedBreeds[0], Gender = SelectedGenders[0], Status = SelectedStatuses[0], Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
        }

        private void ExecuteDeleteManyPetCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem>(Items.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                _dbContext.Pets.Remove(itemToDelete.Pet);
            }
            _dbContext.SaveChanges();
            Items.Clear();
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
                Items.Add(new Elem(item));
            Items.Add(new Elem(new PetModel() {Breed = SelectedBreeds[0], Gender = SelectedGenders[0], Status = SelectedStatuses[0], Birthday = DateTime.Today, CheckInDate = DateTime.Today }));

        }

        private void ExecuteWordExportCommand()
        {
            // Создание нового документа Word
            XWPFDocument document = new XWPFDocument();

            // Создание таблицы
            XWPFTable table = document.CreateTable(Items.Count + 1, 8);

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
            for (int i = 0; i < Items.Count; i++)
            {
                PetModel pet = Items[i].Pet;

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

        public ObservableCollection<PetModel> GetPetsFromTable(string table, bool isFilter)
        {
            ObservableCollection<PetModel> pets = new ObservableCollection<PetModel>();

            //ApplicationDbContext dbContext = new ApplicationDbContext();
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
            //filters[0].SelectedItems = SelectedNames.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            //filters[1].SelectedItems = SelectedGenders.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            //filters[3].SelectedItems = SelectedStatuses.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();
            //filters[2].SelectedItems = SelectedBreeds.Where(x => x.IsSelected == true).Select(s => s.Item).ToList();

            //Pets = GetPetsFromTable("pets", false);
            //foreach (var item in Pets)
            //{
            //    items.Add(new Elem(item));
            //}
        }
    }
}
