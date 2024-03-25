using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetModel : ObservableObject, ICloneable, IEquatable<PetModel>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; } 
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public string BreedId { get; set; }
        [ForeignKey("BreedId")]
        public Breed Breed { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CheckInDate { get; set; }
        public string PassNumber { get; set; }
        public PetModel()
        {
            Breed = new Breed();
            Gender = new Gender();
            Status = new Status();
        }

        public static PetModel Update(PetModel oldPet, PetModel newPet)
        {
            oldPet.Name = newPet.Name;
            oldPet.GenderId = newPet.GenderId;
            oldPet.Gender = newPet.Gender;
            oldPet.StatusId = newPet.StatusId;
            oldPet.Status = newPet.Status;
            oldPet.BreedId = newPet.BreedId;
            oldPet.Breed = newPet.Breed;
            oldPet.Birthday = newPet.Birthday;
            oldPet.CheckInDate = newPet.CheckInDate;
            oldPet.PassNumber = newPet.PassNumber;
            return oldPet;
        }

        public object Clone()
        {
            return new PetModel
            {
                Id = Id,
                Name = Name,
                GenderId = GenderId,
                Gender = Gender,
                StatusId = StatusId,
                Status = Status,
                BreedId = BreedId,
                Breed = Breed,
                Birthday = Birthday,
                CheckInDate = CheckInDate,
                PassNumber = PassNumber
            };
        }

        public bool Equals(PetModel? other)
        {
            return other.Id == Id
                && other.Name == Name
                && other.GenderId == GenderId
                && other.Gender == Gender
                && other.StatusId == StatusId
                && other.Status == Status
                && other.BreedId == BreedId
                && other.Breed == Breed
                && other.Birthday == Birthday
                && other.CheckInDate == CheckInDate
                && other.PassNumber == PassNumber;
        }
    }
}
