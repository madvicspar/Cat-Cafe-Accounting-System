using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

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
            Pets = GetPetsFromTable("pet");
        }
        public static ObservableCollection<PetModel> GetPetsFromTable(string table)
        {
            ObservableCollection<PetModel> pets = new ObservableCollection<PetModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                //DateTime temp = DateTime.Parse(row["birthday"].ToString());
                PetModel pet = new PetModel(Convert.ToInt32(row["id"]), row["name"].ToString(),
                    Convert.ToInt32(row["gender_id"]), Convert.ToInt32(row["status_id"]),
                    row["breed_id"].ToString(), DateTime.Parse(row["birthday"].ToString()),
                    DateTime.Parse(row["check_in_date"].ToString()), row["pass_number"].ToString(),
                    new Breed(row["breed_id"].ToString()), new Gender(Convert.ToInt32(row["gender_id"])), new Status(Convert.ToInt32(row["status_id"])));
                pets.Add(pet);
            }

            return pets;
        }
    }
}
