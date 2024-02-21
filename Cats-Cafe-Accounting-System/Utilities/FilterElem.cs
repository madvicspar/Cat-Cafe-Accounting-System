using CommunityToolkit.Mvvm.ComponentModel;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class FilterElem<T> : ObservableObject
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
        public FilterElem(T item)
        {
            IsSelected = true;
            Item = item;
        }
    }
}