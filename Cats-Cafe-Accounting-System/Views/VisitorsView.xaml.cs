using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cats_Cafe_Accounting_System.Views
{
    /// <summary>
    /// Interaction logic for VisitorsView.xaml
    /// </summary>
    public partial class VisitorsView : UserControl
    {
        readonly string SearchPlaceholder = "Поиск..";
        readonly string FilterFirstNamePlaceholder = "Поиск по имени..";
        readonly string FilterLastNamePlaceholder = "Поиск по фамилии..";
        //readonly string FilterStatusPlaceholder = "Поиск по статусу..";
        //readonly string FilterBreedPlaceholder = "Поиск по породе..";
        public VisitorsView()
        {
            InitializeComponent();
            Search.Text = SearchPlaceholder;
            FilterSearchLastName.Text = FilterLastNamePlaceholder;
            FilterSearchFirstName.Text = FilterFirstNamePlaceholder;
            //FilterSearchStatus.Text = FilterStatusPlaceholder;
            //FilterSearchBreed.Text = FilterBreedPlaceholder;
            FilterFirstNameAll.IsChecked = true;
            FilterLastNameAll.IsChecked = true;
            //FilterStatusAll.IsChecked = true;
            //FilterBreedAll.IsChecked = true;
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
            popUpLastName.PlacementTarget = sender as Button;
            popUpLastName.VerticalOffset = 5;
            popUpLastName.IsOpen = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popUpFirstName.PlacementTarget = sender as Button;
            popUpFirstName.VerticalOffset = 5;
            popUpFirstName.IsOpen = true;
        }

        private void popUpFirstName_Closed(object sender, EventArgs e)
        {
            FilterSearchFirstName.Text = FilterFirstNamePlaceholder;
        }

        private void popUpLastName_Closed(object sender, EventArgs e)
        {
            FilterSearchLastName.Text = FilterLastNamePlaceholder;
        }

        private void FilterSearchFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchFirstName.Text == FilterFirstNamePlaceholder)
                FilterSearchFirstName.Text = "";
        }

        private void FilterSearchFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchFirstName.Text == "")
                FilterSearchFirstName.Text = FilterFirstNamePlaceholder;
        }

        private void FilterSearchLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchLastName.Text == FilterLastNamePlaceholder)
                FilterSearchLastName.Text = "";
        }

        private void FilterSearchLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchLastName.Text == "")
                FilterSearchLastName.Text = FilterLastNamePlaceholder;
        }

        private void popUpGender_Closed(object sender, EventArgs e)
        {

        }

        private void FilterSearchGender_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void FilterSearchGender_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
