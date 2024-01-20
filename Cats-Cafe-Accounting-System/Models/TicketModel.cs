using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cats_Cafe_Accounting_System.RegularClasses;
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
        [ForeignKey("Pet")]
        public int PetId { get; set; }
        public PetModel Pet { get; set; }
        public string Comments { get; set; }
        public TicketModel(int id, float price, int petId, string comments, PetModel pet)
        {
            Id = id;
            Price = price;
            PetId = petId;
            Comments = comments;
            Pet = pet;
        }
        public TicketModel(int id)
        {
            DataRow row = DBContext.GetById("ticket", id);
            Id = Convert.ToInt32(row["id"]);
            Price = (float)Convert.ToDouble(row["price"]);
            //PetId = Convert.ToInt32(row["pet_id"]);
            PetId = 5;
            Comments = row["comments"].ToString();
            Pet = new PetModel(PetId);
        }
    }
}
