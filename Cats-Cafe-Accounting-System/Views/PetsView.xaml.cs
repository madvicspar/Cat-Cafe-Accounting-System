using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cats_Cafe_Accounting_System.Views
{
    /// <summary>
    /// Interaction logic for PetsView.xaml
    /// </summary>
    public partial class PetsView : UserControl
    {
        readonly string FilterNamePlaceholder = "Поиск по имени..";
        readonly string FilterGenderPlaceholder = "Поиск по полу..";
        public PetsView()
        {
            InitializeComponent();
            FilterSearch.Text = FilterNamePlaceholder;
            FilterSearchGender.Text = FilterGenderPlaceholder;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpName.PlacementTarget = sender as Button;
            popUpName.VerticalOffset = 5;
            popUpName.IsOpen = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popUpGender.PlacementTarget = sender as Button;
            popUpGender.VerticalOffset = 5;
            popUpGender.IsOpen = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            popUpStatus.PlacementTarget = sender as Button;
            popUpStatus.VerticalOffset = 5;
            popUpStatus.IsOpen = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            popUpBreed.PlacementTarget = sender as Button;
            popUpBreed.VerticalOffset = 5;
            popUpBreed.IsOpen = true;
        }

        private void FilterSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearch.Text == FilterNamePlaceholder)
                FilterSearch.Text = "";
        }

        private void FilterSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearch.Text == "")
                FilterSearch.Text = FilterNamePlaceholder;
        }

        private void popUpName_Closed(object sender, System.EventArgs e)
        {
            FilterSearch.Text = FilterNamePlaceholder;
        }

        private void popUpGender_Closed(object sender, System.EventArgs e)
        {
            FilterSearchGender.Text = FilterGenderPlaceholder;
        }

        private void FilterSearchGender_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchGender.Text == FilterGenderPlaceholder)
                FilterSearchGender.Text = "";
        }

        private void FilterSearchGender_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchGender.Text == "")
                FilterSearchGender.Text = FilterGenderPlaceholder;
        }

        private void popUpBreed_Closed(object sender, System.EventArgs e)
        {

        }

        private void FilterSearchBreed_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FilterSearchBreed_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
