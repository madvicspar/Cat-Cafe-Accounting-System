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
    public class PetModel : ObservableObject
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
    }
}
