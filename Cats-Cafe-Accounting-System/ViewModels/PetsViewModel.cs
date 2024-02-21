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
        private string searchName = "";
        public string SearchName
        {
            get { return searchName; }
            set
            {
                searchName = value;
                OnPropertyChanged(nameof(SearchName));
            }
        }
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>());
        private ObservableCollection<Elem<PetModel>> items = new ObservableCollection<Elem<PetModel>>();
        public ObservableCollection<Elem<PetModel>> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ObservableCollection<FilterElem<PetModel>> selectedNames = new ObservableCollection<FilterElem<PetModel>>();
        public ObservableCollection<FilterElem<PetModel>> SelectedNames
        {
            get { return selectedNames; }
            set
            {
                selectedNames = value;
                OnPropertyChanged(nameof(SelectedNames));
            }
        }

        private ObservableCollection<FilterElem<Gender>> selectedGenders = new ObservableCollection<FilterElem<Gender>>();
        public ObservableCollection<FilterElem<Gender>> SelectedGenders
        {
            get { return selectedGenders; }
            set
            {
                selectedGenders = value;
                OnPropertyChanged(nameof(SelectedGenders));
            }
        }

        private ObservableCollection<FilterElem<Breed>> selectedBreeds = new ObservableCollection<FilterElem<Breed>>();
        public ObservableCollection<FilterElem<Breed>> SelectedBreeds
        {
            get { return selectedBreeds; }
            set
            {
                selectedBreeds = value;
                OnPropertyChanged(nameof(SelectedBreeds));
            }
        }

        private ObservableCollection<FilterElem<Status>> selectedStatuses = new ObservableCollection<FilterElem<Status>>();
        public ObservableCollection<FilterElem<Status>> SelectedStatuses
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
        public ICommand FilterCommand { get; set; }
        public ICommand SearchNameCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public PetsViewModel()
        {
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
            {
                Items.Add(new Elem<PetModel>(item));
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                SelectedGenders.Add(new FilterElem<Gender>(item));
            }
            foreach (var item in _dbContext.Breeds.ToList())
            {
                SelectedBreeds.Add(new FilterElem<Breed>(item));
            }
            foreach (var item in _dbContext.Statuses.ToList())
            {
                SelectedStatuses.Add(new FilterElem<Status>(item));
            }
            foreach (var item in _dbContext.Pets.ToList())
            {
                SelectedNames.Add(new FilterElem<PetModel>(item));
            }
            Items.Add(new Elem<PetModel>(new PetModel() { Breed = SelectedBreeds[0].Item, Gender = SelectedGenders[0].Item, Status = SelectedStatuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddPetCommand = new RelayCommand(ExecuteAddPetCommand);
            UpdatePetCommand = new RelayCommand<PetModel>(ExecuteUpdatePetCommand);
            DeletePetCommand = new RelayCommand<PetModel>(ExecuteDeletePetCommand);
            DeleteManyPetCommand = new RelayCommand(ExecuteDeleteManyPetCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            //SearchNameCommand = new RelayCommand(ExecuteSearchNameCommand);
        }

        public void ExecuteAddPetCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastPet = Items[Items.Count - 1].Item;

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
            UpdateTable();
        }

        private void ExecuteUpdatePetCommand(PetModel? pet)
        {
            _dbContext.Pets.Update(pet);
            _dbContext.SaveChanges();
            UpdateTable();
        }

        private void ExecuteDeletePetCommand(PetModel? pet)
        {
            _dbContext.Pets.Remove(pet);
            _dbContext.SaveChanges();
            UpdateTable();
        }

        private void ExecuteDeleteManyPetCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<PetModel>>(Items.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                _dbContext.Pets.Remove(itemToDelete.Item);
            }
            _dbContext.SaveChanges();
            UpdateTable();

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
                PetModel pet = Items[i].Item;

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
            var petNames = new ObservableCollection<string>(SelectedNames.Where(p => p.IsSelected).Select(p => p.Item.Name));
            var petGenders = new ObservableCollection<Gender>(SelectedGenders.Where(p => p.IsSelected).Select(p => p.Item));
            var petStatuses = new ObservableCollection<Status>(SelectedStatuses.Where(p => p.IsSelected).Select(p => p.Item));
            var petBreeds = new ObservableCollection<Breed>(SelectedBreeds.Where(p => p.IsSelected).Select(p => p.Item));

            var filteredPets = _dbContext.Pets
                .Where(p => petNames.Contains(p.Name))
                .Where(p => petGenders.Contains(p.Gender))
                .Where(p => petStatuses.Contains(p.Status))
                .Where(p => petBreeds.Contains(p.Breed))
                .ToList();

            Items.Clear();
            foreach (var item in filteredPets)
                Items.Add(new Elem<PetModel>(item));
            Items.Add(new Elem<PetModel>(new PetModel() { Breed = SelectedBreeds[0].Item, Gender = SelectedGenders[0].Item, Status = SelectedStatuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
        }

        public void ExecuteSearchNameCommand()
        {
            var petNames = new ObservableCollection<string>(SelectedNames.Where(p => p.Item.Name.Contains(searchName)).Select(p => p.Item.Name));
            // нужно доп коллекцию или что-то подобное для поиска
        }
    }
}
