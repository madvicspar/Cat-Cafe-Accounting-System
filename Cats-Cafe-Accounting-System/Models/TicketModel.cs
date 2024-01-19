using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cats_Cafe_Accounting_System.Models
{
    public class TicketModel
    {
        [Key]
        public int Id { get; set; }
        public float Price { get; set; }
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public PetModel Pet { get; set; }
        public string Comments { get; set; }
    }
}
