using Cats_Cafe_Accounting_System.Models;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class ElemTicket : Elem<TicketModel>
    {
        public string description = "";
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool isEditable;
        public bool IsEditable
        {
            get => isEditable;
            set
            {
                isEditable = value;
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        public ElemTicket(TicketModel _item) : base(_item)
        {
            if (_item.Pet != null)
                Description = _item.Pet.Name + " " + _item.Pet.Breed.Id + " " + _item.Pet.PassNumber;
            IsEditable = _item.PetId == null;
        }
    }
}