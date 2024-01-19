using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

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
        public PetsViewModel()
        {
            // Инициализация коллекции питомцев
            Pets = new ObservableCollection<PetModel>();
            Pets.Add(new PetModel { Name = "Барсик", Gender = new Gender(0, "Мужской"), Status = new Status(0, "Числится"), Breed = new Breed(0, "Чихуа-хуа"), Birthday = new DateOnly(2023, 05, 23), CheckInDate = new DateOnly(2024, 01, 01), PassNumber = "fneiwjnk123" });
            Pets.Add(new PetModel { Name = "Мурзик", Gender = new Gender(0, "Мужской"), Status = new Status(1, "Не числится"), Breed = new Breed(1, "бульдог"), Birthday = new DateOnly(2023, 02, 01), CheckInDate = new DateOnly(2023, 05, 05), PassNumber = "hiofw789" });
            Pets.Add(new PetModel { Name = "Пушинка", Gender = new Gender(1, "Женский"), Status = new Status(0, "Числится"), Breed = new Breed(2, "Кошка"), Birthday = new DateOnly(2023, 11, 19), CheckInDate = new DateOnly(2024, 01, 01), PassNumber = "puh456" });

        }
    }
}
