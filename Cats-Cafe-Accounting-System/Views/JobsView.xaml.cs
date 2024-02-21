using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.Views
{
    /// <summary>
    /// Interaction logic for JobsView.xaml
    /// </summary>
    public partial class JobsView : UserControl
    {
        readonly string FilterTitlePlaceholder = "Поиск по названию..";
        private bool isTitleFind = false;
        public JobsView()
        {
            InitializeComponent();
            FilterSearch.Text = FilterTitlePlaceholder;
        }

        private void FilterSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearch.Text == FilterTitlePlaceholder)
                FilterSearch.Text = "";
            isTitleFind = false;
        }

        private void FilterSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isTitleFind)
                FilterSearch.Text = FilterTitlePlaceholder;
        }

        private void FilterSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                isTitleFind = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpTitle.PlacementTarget = sender as Button;
            popUpTitle.VerticalOffset = 5;
            popUpTitle.IsOpen = true;
        }
    }
}
