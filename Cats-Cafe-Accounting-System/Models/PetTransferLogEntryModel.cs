using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using System.Data;

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
        public PetTransferLogEntryModel(int id, DateTime date, int visitorId, int petId)
        {
            Id = id;
            Date = date;
            VisitorId = visitorId;
            PetId = petId;
            Visitor = new VisitorModel(VisitorId);
            Pet = new PetModel(PetId);
        }
        public PetTransferLogEntryModel(int id)
        {
            DataRow row = DBContext.GetById("pet_transfer_log_entries", id);
            Id = id;
            Date = DateTime.Parse(row["date"].ToString());
            VisitorId = Convert.ToInt32(row["visitor_id"]);
            PetId = Convert.ToInt32(row["pet_id"]);
            Visitor = new VisitorModel(VisitorId);
            Pet = new PetModel(PetId);
        }
        public PetTransferLogEntryModel()
        {
            Visitor = new VisitorModel();
            Pet = new PetModel();
        }
    }
}
