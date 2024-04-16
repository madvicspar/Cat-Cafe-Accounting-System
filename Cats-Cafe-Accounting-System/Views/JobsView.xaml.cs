using System.Windows;
using System.Windows.Controls;

namespace Cats_Cafe_Accounting_System.Views
{
    /// <summary>
    /// Interaction logic for JobsView.xaml
    /// </summary>
    public partial class JobsView : UserControl
    {
        readonly string SearchPlaceholder = "Поиск по названию..";
        public JobsView()
        {
            InitializeComponent();
            Search.Text = SearchPlaceholder;
            FilterSearchTitle.Text = SearchPlaceholder;
        }

        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == SearchPlaceholder)
                Search.Text = "";
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Search.Text == "")
                Search.Text = SearchPlaceholder;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpTitle.PlacementTarget = sender as Button;
            popUpTitle.VerticalOffset = 5;
            popUpTitle.IsOpen = true;
        }

        private void popUpTitle_Closed(object sender, System.EventArgs e)
        {
            FilterSearchTitle.Text = SearchPlaceholder;
        }

        private void FilterSearchTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchTitle.Text == SearchPlaceholder)
                FilterSearchTitle.Text = "";
        }

        private void FilterSearchTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchTitle.Text == "")
                FilterSearchTitle.Text = SearchPlaceholder;
        }
    }
}
