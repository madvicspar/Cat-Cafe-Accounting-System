using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class Elem<T> : ObservableObject where T : new()
    {
        public bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public T item;
        public T Item
        {
            get { return item; }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }
        public Elem(T _item)
        {
            IsSelected = false;
            Item = _item;
        }
    }
}