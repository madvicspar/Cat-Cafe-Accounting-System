using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.Models
{
    public class TicketModel : ObservableObject, ICloneable, IEquatable<TicketModel>
    {
        [Key]
        public int Id { get; set; }
        public float Price { get; set; }
        public int? PetId { get; set; }
        [ForeignKey("PetId")]
        public virtual PetModel? Pet { get; set; }
        public string Comments { get; set; }
        public TicketModel()
        {
            Pet = new PetModel();
        }

        public static TicketModel Update(TicketModel oldTicket, TicketModel newTicket)
        {
            oldTicket.PetId = newTicket.PetId;
            oldTicket.Pet = newTicket.Pet;
            oldTicket.Price = newTicket.Price;
            oldTicket.Comments = newTicket.Comments;
            return oldTicket;
        }
        public object Clone()
        {
            return new TicketModel
            {
                Id = Id,
                PetId = PetId,
                Pet = Pet,
                Price = Price,
                Comments = Comments
            };
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(TicketModel? other)
        {
            return other?.PetId == PetId
                && other?.Pet == Pet
                && other?.Price == Price
                && other?.Comments == Comments;
        }
    }
}
