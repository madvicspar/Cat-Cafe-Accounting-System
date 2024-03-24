using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetTransferLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        public int VisitorId { get; set; }
        [ForeignKey("VisitorId")]
        public virtual VisitorModel Visitor { get; set; }
        public int PetId { get; set; }
        [ForeignKey("PetId")]
        public virtual PetModel Pet { get; set; }
        public DateTime Date { get; set; }
        public PetTransferLogEntryModel()
        {
            Visitor = new VisitorModel();
            Pet = new PetModel();
        }
    }
}
