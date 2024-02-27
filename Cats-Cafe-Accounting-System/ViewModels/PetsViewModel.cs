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
        public bool IsEnabled {  get; set; }

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

        private string searchGender = "";
        public string SearchGender
        {
            get { return searchGender; }
            set
            {
                searchGender = value;
                OnPropertyChanged(nameof(SearchGender));
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

        private ObservableCollection<FilterElem<PetModel>> names = new ObservableCollection<FilterElem<PetModel>>();
        public ObservableCollection<FilterElem<PetModel>> Names
        {
            get { return names; }
            set
            {
                names = value;
                OnPropertyChanged(nameof(Names));
            }
        }

        private ObservableCollection<FilterElem<Gender>> genders = new ObservableCollection<FilterElem<Gender>>();
        public ObservableCollection<FilterElem<Gender>> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                OnPropertyChanged(nameof(Genders));
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

        private ObservableCollection<FilterElem<PetModel>> filterNames = new ObservableCollection<FilterElem<PetModel>>();
        public ObservableCollection<FilterElem<PetModel>> FilterNames
        {
            get { return filterNames; }
            set
            {
                filterNames = value;
                OnPropertyChanged(nameof(FilterNames));
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
        private ObservableCollection<FilterElem<Gender>> filterGenders = new ObservableCollection<FilterElem<Gender>>();
        public ObservableCollection<FilterElem<Gender>> FilterGenders
        {
            get { return filterGenders; }
            set
            {
                filterGenders = value;
                OnPropertyChanged(nameof(FilterGenders));
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
        public ICommand SearchGenderCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxGenderSelectionCommand { get; set; }
        public PetsViewModel()
        {
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
            {
                Items.Add(new Elem<PetModel>(item));
                Names.Add(new FilterElem<PetModel>(item));
                SelectedNames.Add(new FilterElem<PetModel>(item));
                FilterNames.Add(new FilterElem<PetModel>(item));
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                Genders.Add(new FilterElem<Gender>(item));
                SelectedGenders.Add(new FilterElem<Gender>(item));
                FilterGenders.Add(new FilterElem<Gender>(item));
            }
            foreach (var item in _dbContext.Breeds.ToList())
            {
                SelectedBreeds.Add(new FilterElem<Breed>(item));
            }
            foreach (var item in _dbContext.Statuses.ToList())
            {
                SelectedStatuses.Add(new FilterElem<Status>(item));
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
            SearchNameCommand = new RelayCommand(ExecuteSearchNameCommand);
            SearchGenderCommand = new RelayCommand(ExecuteSearchGenderCommand);
            UpdateCheckBoxSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxSelectionCommand);
            UpdateCheckBoxGenderSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxGenderSelectionCommand);
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
            if (Names.FirstOrDefault (p => p.Item.Name == petToAdd.Name) == null)
            {
                Names.Add(new FilterElem<PetModel>(petToAdd));
                SelectedNames.Add(new FilterElem<PetModel>(petToAdd));
                FilterNames.Add(new FilterElem<PetModel>(petToAdd));
            }    
            UpdateTable();
        }

        private void ExecuteUpdatePetCommand(PetModel? pet)
        {
            string name = _dbContext.Pets.First(p => p == pet).Name;
            _dbContext.Pets.Update(pet);
            _dbContext.SaveChanges();
            if (pet.Name != name)
            {
                Names.First(p => p.Item.Name == name).Item.Name = pet.Name;
                SelectedNames.First(p => p.Item.Name == name).Item.Name = pet.Name;
                FilterNames.First(p => p.Item.Name == name).Item.Name = pet.Name;
            }
            UpdateTable();
        }

        private void ExecuteDeletePetCommand(PetModel? pet)
        {
            string name = _dbContext.Pets.First(p => p == pet).Name;
            _dbContext.Pets.Remove(pet);
            _dbContext.SaveChanges();
            if (_dbContext.Pets.FirstOrDefault(p => p.Name == name) == null)
            {
                Names.Remove(Names.First(p => p.Item == pet));
                SelectedNames.Remove(SelectedNames.First(p => p.Item == pet));
                FilterNames.Remove(FilterNames.First(p => p.Item == pet));
            }
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
            var petNames = new ObservableCollection<string>(Names.Where(p => p.IsSelected).Select(p => p.Item.Name));
            var petGenders = new ObservableCollection<Gender>(Genders.Where(p => p.IsSelected).Select(p => p.Item));
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

        public void ExecuteUpdateCheckBoxSelectionCommand()
        {
            foreach (var item in FilterNames)
                Names.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
            //selectedNames.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxGenderSelectionCommand()
        {
            foreach (var item in FilterGenders)
                Genders.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
            //selectedNames.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteSearchNameCommand()
        {
            if (SearchName.Length > 5 && SearchName[..5] == "Поиск")
            {
                FilterNames.Clear();
                foreach (var item in Names)
                {
                    FilterNames.Add(item);
                    FilterNames.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var petNames = new ObservableCollection<string>(Names.Where(p => p.Item.Name.ToLower().Contains(SearchName.ToLower())).Select(p => p.Item.Name));

            FilterNames.Clear();
            foreach (var item in Names.Where(p => petNames.Contains(p.Item.Name)))
            {
                FilterNames.Add(item);
                FilterNames.Last().IsSelected = item.IsSelected;
            }
            // после удаления не обновляются значения фильтров
        }
        public void ExecuteSearchGenderCommand()
        {
            if (SearchGender.Length > 5 && SearchGender[..5] == "Поиск")
            {
                FilterGenders.Clear();
                foreach (var item in Genders)
                {
                    FilterGenders.Add(item);
                    FilterGenders.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var petGenders = new ObservableCollection<string>(Genders.Where(p => p.Item.Title.ToLower().Contains(SearchGender.ToLower())).Select(p => p.Item.Title));

            FilterGenders.Clear();
            foreach (var item in Genders.Where(p => petGenders.Contains(p.Item.Title)))
            {
                FilterGenders.Add(item);
                FilterGenders.Last().IsSelected = item.IsSelected;
            }
        }
    }
}
