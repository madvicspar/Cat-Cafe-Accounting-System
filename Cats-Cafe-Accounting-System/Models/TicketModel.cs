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
        public TicketModel(int id, float price, int petId, string comments)
        {
            Id = id;
            Price = price;
            PetId = petId;
            Comments = comments;
            Pet = new PetModel(PetId);
        }
        public TicketModel(int id)
        {
            DataRow row = DBContext.GetById("tickets", id);
            Id = Convert.ToInt32(row["id"]);
            Price = (float)Convert.ToDouble(row["price"]);
            PetId = row["petid"].ToString() == "" ? 0 : Convert.ToInt32(row["petid"]);
            Comments = row["comments"].ToString();
            Pet = new PetModel(PetId);
        }
        public TicketModel()
        {
            Pet = new PetModel();
        }
    }
}
