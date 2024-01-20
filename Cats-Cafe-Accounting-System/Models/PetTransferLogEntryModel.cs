using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.Utilities;
using Google.Protobuf.WellKnownTypes;
using System.Data;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetTransferLogEntryModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Visitor")]
        public int VisitorId { get; set; }
        public VisitorModel Visitor { get; set; }
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public PetModel Pet { get; set; }
        public DateTime Date { get; set; }
        public PetTransferLogEntryModel(int id, DateTime date, int visitorId, int petId, VisitorModel visitor, PetModel pet)
        {
            Id = id;
            Date = date;
            VisitorId = visitorId;
            PetId = petId;
            Visitor = visitor;
            Pet = pet;
        }
        public PetTransferLogEntryModel(int id)
        {
            DataRow row = DBContext.GetById("owner_log", id);
            Id = id;
            Date = DateTime.Parse(row["date"].ToString());
            VisitorId = Convert.ToInt32(row["visitor_id"]);
            PetId = Convert.ToInt32(row["pet_id"]);
            Visitor = new VisitorModel(VisitorId);
            Pet = new PetModel(PetId);
        }
    }
}
