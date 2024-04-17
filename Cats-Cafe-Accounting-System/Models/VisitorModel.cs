using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.Models
{
    public class VisitorModel : ObservableObject, ICloneable, IEquatable<VisitorModel>
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Pathronymic { get; set; }
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public virtual Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public VisitorModel()
        {
            Gender = new Gender();
        }

        public static VisitorModel Update(VisitorModel oldPet, VisitorModel newPet)
        {
            oldPet.FirstName = newPet.FirstName;
            oldPet.LastName = newPet.LastName;
            oldPet.Pathronymic = newPet.Pathronymic;
            oldPet.GenderId = newPet.GenderId;
            oldPet.Gender = newPet.Gender;
            oldPet.Birthday = newPet.Birthday;
            oldPet.PhoneNumber = newPet.PhoneNumber;
            return oldPet;
        }
        public object Clone()
        {
            return new VisitorModel
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Pathronymic = Pathronymic,
                GenderId = GenderId,
                Gender = Gender,
                PhoneNumber = PhoneNumber,
                Birthday = Birthday
            };
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(VisitorModel? other)
        {
            return other?.LastName == LastName
                && other?.FirstName == FirstName
                && other?.Pathronymic == Pathronymic
                && other?.GenderId == GenderId
                && other?.Gender == Gender
                && other?.Birthday == Birthday
                && other?.PhoneNumber == PhoneNumber;
        }
    }
}
