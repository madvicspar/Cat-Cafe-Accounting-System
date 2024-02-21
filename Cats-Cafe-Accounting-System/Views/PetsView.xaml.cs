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
    /// Interaction logic for PetsView.xaml
    /// </summary>
    public partial class PetsView : UserControl
    {
        readonly string FilterNamePlaceholder = "Поиск по имени..";
        bool isNameFind = false;
        public PetsView()
        {
            InitializeComponent();
            FilterSearch.Text = FilterNamePlaceholder;
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
            isNameFind = false;
        }

        private void FilterSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isNameFind)
                FilterSearch.Text = FilterNamePlaceholder;
        }

        private void FilterSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                isNameFind = true;
            }
        }
    }
}
