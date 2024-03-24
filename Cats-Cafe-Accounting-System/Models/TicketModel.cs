using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class TicketModel
    {
        [Key]
        public int Id { get; set; }
        public float Price { get; set; }
        public int PetId { get; set; }
        [ForeignKey("PetId")]
        public virtual PetModel? Pet { get; set; }
        public string Comments { get; set; }
        public TicketModel()
        {
            Pet = new PetModel();
        }
    }
}
