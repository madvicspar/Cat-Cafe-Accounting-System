using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Cats_Cafe_Accounting_System.Models
{
    public class PetTransferLogEntryModel : ObservableObject, ICloneable, IEquatable<PetTransferLogEntryModel>
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

        public static PetTransferLogEntryModel Update(PetTransferLogEntryModel oldPetTransferLogEntryModel, PetTransferLogEntryModel newPetTransferLogEntryModel)
        {
            oldPetTransferLogEntryModel.Date = newPetTransferLogEntryModel.Date;
            oldPetTransferLogEntryModel.PetId = newPetTransferLogEntryModel.PetId;
            oldPetTransferLogEntryModel.Pet = newPetTransferLogEntryModel.Pet;
            oldPetTransferLogEntryModel.VisitorId = newPetTransferLogEntryModel.VisitorId;
            oldPetTransferLogEntryModel.Visitor = newPetTransferLogEntryModel.Visitor;
            return oldPetTransferLogEntryModel;
        }

        public object Clone()
        {
            return new PetTransferLogEntryModel
            {
                Id = Id,
                Date = Date,
                PetId = PetId,
                VisitorId = VisitorId,
                Visitor = Visitor
            };
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(PetTransferLogEntryModel? other)
        {
            return other?.Date == Date
                && other?.PetId == PetId
                && other?.Pet == Pet
                && other?.PetId == PetId;
        }
    }
}
