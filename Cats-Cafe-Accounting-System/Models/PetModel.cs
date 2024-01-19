using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int BreedId { get; set; }
        public Breed Breed { get; set; }
        public DateOnly Birthday { get; set; }
        public DateOnly CheckInDate { get; set; }
        public string PassNumber { get; set; }
    }
}
