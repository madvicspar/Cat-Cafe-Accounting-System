using System;
using System.Windows;
using System.Windows.Controls;

namespace Cats_Cafe_Accounting_System.Views
{
    /// <summary>
    /// Interaction logic for EmployeeShiftLogView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        readonly string SearchPlaceholder = "Поиск..";
        readonly string FilterFirstNamePlaceholder = "Поиск по имени..";
        readonly string FilterLastNamePlaceholder = "Поиск по фамилии..";
        readonly string FilterGenderPlaceholder = "Поиск по полу";
        readonly string FilterJobPlaceholder = "Поиск по должности";
        public EmployeesView()
        {
            InitializeComponent();
            Search.Text = SearchPlaceholder;
            FilterSearchLastName.Text = FilterLastNamePlaceholder;
            FilterSearchFirstName.Text = FilterFirstNamePlaceholder;
            FilterSearchGender.Text = FilterGenderPlaceholder;
            FilterSearchJob.Text = FilterJobPlaceholder;
            FilterLastNameAll.IsChecked = true;
            FilterGenderAll.IsChecked = true;
            FilterFirstNameAll.IsChecked = true;
            FilterJobAll.IsChecked = true;
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

        private void popUpLastName_Closed(object sender, EventArgs e)
        {
            FilterSearchLastName.Text = FilterLastNamePlaceholder;
        }

        private void popUpGender_Closed(object sender, EventArgs e)
        {
            FilterSearchGender.Text = FilterGenderPlaceholder;
        }

        private void popUpFirstName_Closed(object sender, EventArgs e)
        {
            FilterSearchFirstName.Text = FilterFirstNamePlaceholder;
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

        private void FilterSearchJob_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchJob.Text == FilterJobPlaceholder)
                FilterSearchJob.Text = "";
        }

        private void FilterSearchJob_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearchJob.Text == "")
                FilterSearchJob.Text = FilterJobPlaceholder;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popUpFirstName.PlacementTarget = sender as Button;
            popUpFirstName.VerticalOffset = 5;
            popUpFirstName.IsOpen = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            popUpGender.PlacementTarget = sender as Button;
            popUpGender.VerticalOffset = 5;
            popUpGender.IsOpen = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            popUpJob.PlacementTarget = sender as Button;
            popUpJob.VerticalOffset = 5;
            popUpJob.IsOpen = true;
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

        private void popUpJob_Closed(object sender, EventArgs e)
        {
            FilterSearchJob.Text = FilterJobPlaceholder;
        }
    }
}
