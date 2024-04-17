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
    public class PetsViewModel : ObservableObject
    {
        public enum Fields
        {
            name, breed
        }

        public Fields field;
        public Fields Field
        {
            get { return field; }
            set
            {
                field = value;
                OnPropertyChanged(nameof(field));
            }
        }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        public bool IsEnabled { get; set; }

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

        private string searchStatus = "";
        public string SearchStatus
        {
            get { return searchStatus; }
            set
            {
                searchStatus = value;
                OnPropertyChanged(nameof(SearchStatus));
            }
        }

        private string searchBreed = "";
        public string SearchBreed
        {
            get { return searchBreed; }
            set
            {
                searchBreed = value;
                OnPropertyChanged(nameof(SearchBreed));
            }
        }

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

        private ObservableCollection<Elem<PetModel>> filterItems = new ObservableCollection<Elem<PetModel>>();
        public ObservableCollection<Elem<PetModel>> FilterItems
        {
            get { return filterItems; }
            set
            {
                filterItems = value;
                OnPropertyChanged(nameof(FilterItems));
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

        private ObservableCollection<FilterElem<Status>> statuses = new ObservableCollection<FilterElem<Status>>();
        public ObservableCollection<FilterElem<Status>> Statuses
        {
            get { return statuses; }
            set
            {
                statuses = value;
                OnPropertyChanged(nameof(Statuses));
            }
        }

        private ObservableCollection<FilterElem<Breed>> breeds = new ObservableCollection<FilterElem<Breed>>();
        public ObservableCollection<FilterElem<Breed>> Breeds
        {
            get { return breeds; }
            set
            {
                breeds = value;
                OnPropertyChanged(nameof(Breeds));
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

        private ObservableCollection<FilterElem<Status>> filterStatuses = new ObservableCollection<FilterElem<Status>>();
        public ObservableCollection<FilterElem<Status>> FilterStatuses
        {
            get { return filterStatuses; }
            set
            {
                filterStatuses = value;
                OnPropertyChanged(nameof(FilterStatuses));
            }
        }

        private ObservableCollection<FilterElem<Breed>> filterBreeds = new ObservableCollection<FilterElem<Breed>>();
        public ObservableCollection<FilterElem<Breed>> FilterBreeds
        {
            get { return filterBreeds; }
            set
            {
                filterBreeds = value;
                OnPropertyChanged(nameof(FilterBreeds));
            }
        }

        public ICommand AddPetCommand { get; set; }
        public ICommand UpdatePetCommand { get; set; }
        public ICommand DeletePetCommand { get; set; }
        public ICommand DeleteManyPetCommand { get; set; }
        public ICommand ChangeSelectionCommand { get; set; }
        public ICommand ChangeNameSelectionCommand { get; set; }
        public ICommand ChangeGenderSelectionCommand { get; set; }
        public ICommand ChangeStatusSelectionCommand { get; set; }
        public ICommand ChangeBreedSelectionCommand { get; set; }
        public ICommand ChangeFieldCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchNameCommand { get; set; }
        public ICommand SearchGenderCommand { get; set; }
        public ICommand SearchStatusCommand { get; set; }
        public ICommand SearchBreedCommand { get; set; }
        public ICommand ExcelExportCommand { get; set; }
        public ICommand WordExportCommand { get; set; }
        public ICommand UpdateCheckBoxSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxGenderSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxStatusSelectionCommand { get; set; }
        public ICommand UpdateCheckBoxBreedSelectionCommand { get; set; }
        public ICommand DeleteDateFiltersCommand { get; set; }
        public ICommand ElemUpdatedCommand { get; set; }
        public PetsViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            IsEnabled = Data.user?.Job.Id != 3;
            foreach (var item in _dbContext.Pets.Include(p => p.Gender).Include(p => p.Status).Include(p => p.Breed).ToList())
            {
                FilterItems.Add(new Elem<PetModel>(item.Clone() as PetModel));
                Items.Add(new Elem<PetModel>(item.Clone() as PetModel));
                if (!Names.Any(p => p.Item.Name == item.Name))
                {
                    Names.Add(new FilterElem<PetModel>(item.Clone() as PetModel));
                    FilterNames.Add(new FilterElem<PetModel>(item.Clone() as PetModel));
                }
            }
            foreach (var item in _dbContext.Genders.ToList())
            {
                Genders.Add(new FilterElem<Gender>(item));
                FilterGenders.Add(new FilterElem<Gender>(item));
            }
            foreach (var item in _dbContext.Breeds.ToList())
            {
                Breeds.Add(new FilterElem<Breed>(item));
                FilterBreeds.Add(new FilterElem<Breed>(item));
            }
            foreach (var item in _dbContext.Statuses.ToList())
            {
                Statuses.Add(new FilterElem<Status>(item));
                FilterStatuses.Add(new FilterElem<Status>(item));
            }
            FilterItems.Add(new Elem<PetModel>(new PetModel() { Breed = Breeds[0].Item, Gender = Genders[0].Item, Status = Statuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
            ExecuteDeleteDateFiltersCommand();
            ExcelExportCommand = new RelayCommand(ExecuteExcelExportCommand);
            WordExportCommand = new RelayCommand(ExecuteWordExportCommand);
            AddPetCommand = new RelayCommand(ExecuteAddPetCommand);
            UpdatePetCommand = new RelayCommand<PetModel>(ExecuteUpdatePetCommand);
            DeletePetCommand = new RelayCommand<PetModel>(ExecuteDeletePetCommand);
            DeleteManyPetCommand = new RelayCommand(ExecuteDeleteManyPetCommand);
            ChangeSelectionCommand = new RelayCommand<bool>(ExecuteChangeSelectionCommand);
            ChangeNameSelectionCommand = new RelayCommand<bool>(ExecuteChangeNameSelectionCommand);
            ChangeGenderSelectionCommand = new RelayCommand<bool>(ExecuteChangeGenderSelectionCommand);
            ChangeStatusSelectionCommand = new RelayCommand<bool>(ExecuteChangeStatusSelectionCommand);
            ChangeBreedSelectionCommand = new RelayCommand<bool>(ExecuteChangeBreedSelectionCommand);
            ChangeFieldCommand = new RelayCommand<string>(ExecuteChangeFieldCommand);
            FilterCommand = new RelayCommand(ExecuteFilterCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            SearchNameCommand = new RelayCommand(ExecuteSearchNameCommand);
            SearchGenderCommand = new RelayCommand(ExecuteSearchGenderCommand);
            SearchStatusCommand = new RelayCommand(ExecuteSearchStatusCommand);
            SearchBreedCommand = new RelayCommand(ExecuteSearchBreedCommand);
            UpdateCheckBoxSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxSelectionCommand);
            UpdateCheckBoxGenderSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxGenderSelectionCommand);
            UpdateCheckBoxStatusSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxStatusSelectionCommand);
            UpdateCheckBoxBreedSelectionCommand = new RelayCommand(ExecuteUpdateCheckBoxBreedSelectionCommand);
            DeleteDateFiltersCommand = new RelayCommand(ExecuteDeleteDateFiltersCommand);
            ElemUpdatedCommand = new RelayCommand<Elem<PetModel>>(ExecuteElemUpdatedCommand);
        }
        public void ExecuteDeleteDateFiltersCommand()
        {
            DateTime birtdayMin = FilterItems.MinBy(item => item.Item.Birthday).Item.Birthday;
            DateTime birtdayMax = FilterItems.MaxBy(item => item.Item.Birthday).Item.Birthday;
            DateTime checkInDateMin = FilterItems.MinBy(item => item.Item.CheckInDate).Item.CheckInDate;
            DateTime checkInDateMax = FilterItems.MaxBy(item => item.Item.CheckInDate).Item.CheckInDate;
            if (birtdayMin < checkInDateMin)
                StartDate = birtdayMin;
            else
                StartDate = checkInDateMin;
            if (birtdayMax > checkInDateMax)
                EndDate = birtdayMax;
            else
                EndDate = checkInDateMax;
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteElemUpdatedCommand(Elem<PetModel> petUpdated)
        {
            if (_dbContext.Pets.FirstOrDefault(p => p.Equals(petUpdated.Item)) == null || !petUpdated.Item.Equals(Items.First(p => p.Item.Id == petUpdated.Item.Id).Item))
            {
                petUpdated.IsUpdated = true;
            }
        }
        public void ExecuteAddPetCommand()
        {
            // добавить проверку на то, что все введено, и введено правильно
            var lastPet = FilterItems[FilterItems.Count - 1].Item;
            PetModel petToAdd = new PetModel();
            try
            {
                petToAdd = new PetModel
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

                _dbContext.Pets.Add(petToAdd.Clone() as PetModel);
                _dbContext.SaveChanges();
            }
            catch {
                MessageBox.Show("Введите все данные питомца");
                return;
            }
            petToAdd.Id = _dbContext.Pets.First(p => p.Name == petToAdd.Name).Id;
            if (Names.FirstOrDefault(p => p.Item.Name == petToAdd.Name) == null)
            {
                Names.Add(new FilterElem<PetModel>(petToAdd.Clone() as PetModel));
                FilterNames.Add(new FilterElem<PetModel>(petToAdd.Clone() as PetModel));
                Items.Add(new Elem<PetModel>(petToAdd.Clone() as PetModel));
                FilterItems.Add(new Elem<PetModel>(petToAdd.Clone() as PetModel));
            }
            UpdateTable();
        }
        /// <summary>
        /// Обновление PetModel в базе данных, обновление коллекций Names, FilterNames для корректных поиска и фильтрации
        /// </summary>
        /// <param name="pet"> Измененный PetModel </param>
        public void ExecuteUpdatePetCommand(PetModel? pet)
        {
            string name = "";
            try
            {
                PetModel old = _dbContext.Pets.First(p => p.Id == pet.Id);
                name = old.Name;
                _dbContext.Pets.Update(PetModel.Update(old, pet));
                _dbContext.SaveChanges();
            }
            catch
            {
                if (_dbContext.Pets.FirstOrDefault(p => p.Id == pet.Id) != null)
                    MessageBox.Show("Введите все данные питомца");
                return;
            }
            if (pet.Name != name)
            {
                Names.First(p => p.Item.Name == name).Item.Name = pet.Name;
                FilterNames.First(p => p.Item.Id == pet.Id).Item.Name = pet.Name;
            }
            FilterItems.First(p => p.Item.Id == pet.Id).IsUpdated = false;
            UpdateTable();
        }

        public void ExecuteDeletePetCommand(PetModel? pet)
        {
            if (FilterItems.Count > 1)
            {
                string name = "";
                try
                {
                    name = _dbContext.Pets.First(p => p == pet).Name;
                    _dbContext.Pets.Remove(pet);
                    _dbContext.SaveChanges();
                }
                catch
                {
                    return;
                }
                if (_dbContext.Pets.FirstOrDefault(p => p.Name == name) == null)
                {
                    Names.Remove(Names.First(p => p.Item.Name == pet.Name));
                    FilterNames.Remove(FilterNames.First(p => p.Item.Name == pet.Name));
                }
            }
            else
            {
                FilterItems[FilterItems.Count - 1].Item.Name = "";
                FilterItems[FilterItems.Count - 1].Item.PassNumber = "";
            }
            UpdateTable();
        }

        public void ExecuteDeleteManyPetCommand()
        {
            var itemsToDelete = new ObservableCollection<Elem<PetModel>>(FilterItems.Where(x => x.IsSelected).ToList());
            foreach (var itemToDelete in itemsToDelete)
            {
                if (FilterItems.IndexOf(itemToDelete) != FilterItems.Count-1)
                {
                    try
                    {
                        PetModel pet = _dbContext.Pets.First(p => p.Id == itemToDelete.Item.Id);
                        _dbContext.Pets.Remove(pet);
                        _dbContext.SaveChanges();
                    }
                    catch
                    {
                        return;
                    }
                    if (_dbContext.Pets.FirstOrDefault(p => p.Name == itemToDelete.Item.Name) == null)
                    {
                        Names.Remove(Names.First(p => p.Item.Name == itemToDelete.Item.Name));
                        FilterNames.Remove(FilterNames.First(p => p.Item.Name == itemToDelete.Item.Name));
                    }
                }
                else
                {
                    FilterItems[FilterItems.Count - 1].Item.Name = "";
                    FilterItems[FilterItems.Count - 1].Item.PassNumber = "";
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
            XWPFTable table = document.CreateTable(FilterItems.Count, 7);

            // Заполнение заголовков столбцов
            table.GetRow(0).GetCell(0).SetText("Кличка");
            table.GetRow(0).GetCell(1).SetText("Пол");
            table.GetRow(0).GetCell(2).SetText("Порода");
            table.GetRow(0).GetCell(3).SetText("Статус");
            table.GetRow(0).GetCell(4).SetText("Дата рождения");
            table.GetRow(0).GetCell(5).SetText("Дата появления в котокафе");
            table.GetRow(0).GetCell(6).SetText("Номер паспорта");

            // Заполнение данных о питомцах
            for (int i = 0; i < FilterItems.Count - 1; i++)
            {
                PetModel pet = FilterItems[i].Item;

                table.GetRow(i + 1).GetCell(0).SetText(pet.Name);
                table.GetRow(i + 1).GetCell(1).SetText(pet.Gender.Title);
                table.GetRow(i + 1).GetCell(2).SetText(pet.Breed.Title);
                table.GetRow(i + 1).GetCell(3).SetText(pet.Status.Title);
                table.GetRow(i + 1).GetCell(4).SetText(pet.Birthday.ToString());
                table.GetRow(i + 1).GetCell(5).SetText(pet.CheckInDate.ToString());
                table.GetRow(i + 1).GetCell(6).SetText(pet.PassNumber);
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
                    List<string> headers = new List<string>() { "Кличка", "Пол", "Порода", "Статус", "Дата рождения", "Дата появления в котокафе", "Номер паспорта" };
                    List<List<string>> pets = new List<List<string>>();
                    for (int i = 0; i < FilterItems.Count - 1; i++)
                    {
                        pets.Add(new List<string>() { FilterItems[i].Item.Name, FilterItems[i].Item.Gender.Title,
                            FilterItems[i].Item.Breed.Title, FilterItems[i].Item.Status.Title, FilterItems[i].Item.Birthday.ToString("dd.MM.yyyy"),
                            FilterItems[i].Item.CheckInDate.ToString("dd.MM.yyyy"), FilterItems[i].Item.PassNumber });
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

        public void ExecuteChangeNameSelectionCommand(bool value)
        {
            foreach (var item in FilterNames)
                item.IsSelected = !value;
        }

        public void ExecuteChangeGenderSelectionCommand(bool value)
        {
            foreach (var item in FilterGenders)
                item.IsSelected = !value;
        }

        public void ExecuteChangeStatusSelectionCommand(bool value)
        {
            foreach (var item in FilterStatuses)
                item.IsSelected = !value;
        }

        public void ExecuteChangeBreedSelectionCommand(bool value)
        {
            foreach (var item in FilterBreeds)
                item.IsSelected = !value;
        }

        public void ExecuteChangeFieldCommand(string? value)
        {
            switch (value)
            {
                case "По имени":
                    Field = Fields.name;
                    break;
                case "По породе":
                    Field = Fields.breed;
                    break;
                default:
                    Field = Fields.name;
                    break;
            }
        }

        [ExcludeFromCodeCoverage]
        public void ExecuteFilterCommand()
        {
            foreach (var item in Names)
            {
                var pet = GetWithoutNameFilter().FirstOrDefault(p => p.Item.Name == item.Item.Name);
                if (pet is not null)
                {
                    item.IsSelected = pet.IsSelected;
                }
            }
            foreach (var item in Genders)
            {
                var g = GetWithoutGenderFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (g is not null)
                {
                    item.IsSelected = g.IsSelected;
                }
            }
            foreach (var item in Statuses)
            {
                var g = GetWithoutStatusFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (g is not null)
                {
                    item.IsSelected = g.IsSelected;
                }
            }
            foreach (var item in Breeds)
            {
                var g = GetWithoutBreedFilter().FirstOrDefault(p => p.Item.Title == item.Item.Title);
                if (g is not null)
                {
                    item.IsSelected = g.IsSelected;
                }
            }
            UpdateTable();
        }

        public void UpdateTable()
        {
            var petNames = new ObservableCollection<string>(Names.Where(p => p.IsSelected).Select(p => p.Item.Name));
            var petGenders = new ObservableCollection<Gender>(Genders.Where(p => p.IsSelected).Select(p => p.Item));
            var petStatuses = new ObservableCollection<Status>(Statuses.Where(p => p.IsSelected).Select(p => p.Item));
            var petBreeds = new ObservableCollection<Breed>(Breeds.Where(p => p.IsSelected).Select(p => p.Item));
            var petSearched = FilterItems.Select(p => p.Item);

            var filteredPets = _dbContext.Pets
                //.Where(p => petSearched.Contains(p))
                .Where(p => petNames.Contains(p.Name))
                .Where(p => petGenders.Contains(p.Gender))
                .Where(p => petStatuses.Contains(p.Status))
                .Where(p => petBreeds.Contains(p.Breed))
                .Where(p => p.Birthday >= StartDate && p.Birthday <= EndDate)
                .Where(p => p.CheckInDate >= StartDate && p.CheckInDate <= EndDate)
                .ToList();

            FilterItems.Clear();
            foreach (var item in filteredPets)
                FilterItems.Add(new Elem<PetModel>(item.Clone() as PetModel));
            FilterItems.Add(new Elem<PetModel>(new PetModel() { Breed = Breeds[0].Item, Gender = Genders[0].Item, Status = Statuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
        }

        public void ExecuteUpdateCheckBoxSelectionCommand()
        {
            foreach (var item in FilterNames)
                Names.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxGenderSelectionCommand()
        {
            foreach (var item in FilterGenders)
                Genders.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxStatusSelectionCommand()
        {
            foreach (var item in FilterStatuses)
                Statuses.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteUpdateCheckBoxBreedSelectionCommand()
        {
            foreach (var item in FilterBreeds)
                Breeds.First(p => p.Item == item.Item).IsSelected = item.IsSelected;
        }

        public void ExecuteSearchNameCommand()
        {
            if (SearchName.Length > 5 && SearchName[..5] == "Поиск")
            {
                FilterNames.Clear();
                foreach (var item in Names)
                {
                    if (!FilterNames.Any(p => p.Item.Name == item.Item.Name))
                    {
                        FilterNames.Add(item);
                        FilterNames.Last().IsSelected = item.IsSelected;
                    }
                }
                return;
            }
            var petNames = new ObservableCollection<string>(Names.Where(p => p.Item.Name.ToLower().Contains(SearchName.ToLower())).Select(p => p.Item.Name));

            FilterNames.Clear();
            foreach (var item in Names.Where(p => petNames.Contains(p.Item.Name)))
            {
                if (!FilterNames.Any(p => p.Item.Name == item.Item.Name))
                {
                    FilterNames.Add(item);
                    FilterNames.Last().IsSelected = item.IsSelected;
                }
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

        public void ExecuteSearchStatusCommand()
        {
            if (SearchStatus.Length > 5 && SearchStatus[..5] == "Поиск")
            {
                FilterStatuses.Clear();
                foreach (var item in Statuses)
                {
                    FilterStatuses.Add(item);
                    FilterStatuses.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var petStatuses = new ObservableCollection<string>(Statuses.Where(p => p.Item.Title.ToLower().Contains(SearchStatus.ToLower())).Select(p => p.Item.Title));

            FilterStatuses.Clear();
            foreach (var item in Statuses.Where(p => petStatuses.Contains(p.Item.Title)))
            {
                FilterStatuses.Add(item);
                FilterStatuses.Last().IsSelected = item.IsSelected;
            }
        }

        public void ExecuteSearchBreedCommand()
        {
            if (SearchBreed.Length > 5 && SearchBreed[..5] == "Поиск")
            {
                FilterBreeds.Clear();
                foreach (var item in Breeds)
                {
                    FilterBreeds.Add(item);
                    FilterBreeds.Last().IsSelected = item.IsSelected;
                }
                return;
            }
            var petBreeds = new ObservableCollection<string>(Breeds.Where(p => p.Item.Title.ToLower().Contains(SearchBreed.ToLower())).Select(p => p.Item.Title));

            FilterBreeds.Clear();
            foreach (var item in Breeds.Where(p => petBreeds.Contains(p.Item.Title)))
            {
                FilterBreeds.Add(item);
                FilterBreeds.Last().IsSelected = item.IsSelected;
            }
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<PetModel>> GetWithoutNameFilter()
        {
            var collection = new ObservableCollection<FilterElem<PetModel>>();
            foreach (var item in Names)
            {
                if (!collection.Any(p => p.Item.Name == item.Item.Name))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<Gender>> GetWithoutGenderFilter()
        {
            var collection = new ObservableCollection<FilterElem<Gender>>();
            foreach (var item in Genders)
            {
                if (!collection.Any(p => p.Item.Title == item.Item.Title))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<Status>> GetWithoutStatusFilter()
        {
            var collection = new ObservableCollection<FilterElem<Status>>();
            foreach (var item in Statuses)
            {
                if (!collection.Any(p => p.Item.Title == item.Item.Title))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        [ExcludeFromCodeCoverage]
        public ObservableCollection<FilterElem<Breed>> GetWithoutBreedFilter()
        {
            var collection = new ObservableCollection<FilterElem<Breed>>();
            foreach (var item in Breeds)
            {
                if (!collection.Any(p => p.Item.Title == item.Item.Title))
                {
                    collection.Add(item);
                    collection.Last().IsSelected = item.IsSelected;
                }
            }
            return collection;
        }

        public void ExecuteSearchCommand()
        {
            if (SearchText.Length > 5 && SearchText[..5] == "Поиск")
            {
                FilterItems.Clear();
                foreach (var item in Items)
                {
                    if (FilterNames.FirstOrDefault(p => p.Item.Name == item.Item.Name && p.IsSelected == true) is not null
                        && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                        && FilterStatuses.FirstOrDefault(p => p.Item.Title == item.Item.Status.Title && p.IsSelected == true) is not null
                        && FilterBreeds.FirstOrDefault(p => p.Item.Title == item.Item.Breed.Title && p.IsSelected == true) is not null
                        && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate
                        && item.Item.CheckInDate >= StartDate && item.Item.CheckInDate <= EndDate)
                        FilterItems.Add(item);
                }
                FilterItems.Add(new Elem<PetModel>(new PetModel() { Breed = Breeds[0].Item, Gender = Genders[0].Item, Status = Statuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
                return;
            }

            switch (Field)
            {
                case Fields.name:
                    var petNames = new ObservableCollection<string>(Names.Where(p => p.Item.Name.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.Name));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => petNames.Contains(p.Item.Name)))
                    {
                        if (FilterNames.FirstOrDefault(p => p.Item.Name == item.Item.Name && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterStatuses.FirstOrDefault(p => p.Item.Title == item.Item.Status.Title && p.IsSelected == true) is not null
                            && FilterBreeds.FirstOrDefault(p => p.Item.Title == item.Item.Breed.Title && p.IsSelected == true) is not null
                            && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate
                            && item.Item.CheckInDate >= StartDate && item.Item.CheckInDate <= EndDate)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<PetModel>(new PetModel() { Breed = Breeds[0].Item, Gender = Genders[0].Item, Status = Statuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
                    break;
                case Fields.breed:
                    var petBreeds = new ObservableCollection<string>(Breeds.Where(p => p.Item.Title.ToLower().Contains(SearchText.ToLower())).Select(p => p.Item.Title));

                    FilterItems.Clear();
                    foreach (var item in Items.Where(p => petBreeds.Contains(p.Item.Breed.Title)))
                    {
                        if (FilterNames.FirstOrDefault(p => p.Item.Name == item.Item.Name && p.IsSelected == true) is not null
                            && FilterGenders.FirstOrDefault(p => p.Item.Title == item.Item.Gender.Title && p.IsSelected == true) is not null
                            && FilterStatuses.FirstOrDefault(p => p.Item.Title == item.Item.Status.Title && p.IsSelected == true) is not null
                            && FilterBreeds.FirstOrDefault(p => p.Item.Title == item.Item.Breed.Title && p.IsSelected == true) is not null
                            && item.Item.Birthday >= StartDate && item.Item.Birthday <= EndDate
                            && item.Item.CheckInDate >= StartDate && item.Item.CheckInDate <= EndDate)
                            FilterItems.Add(item);
                    }
                    FilterItems.Add(new Elem<PetModel>(new PetModel() { Breed = Breeds[0].Item, Gender = Genders[0].Item, Status = Statuses[0].Item, Birthday = DateTime.Today, CheckInDate = DateTime.Today }));
                    break;
            }

        }
    }
}
