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
    /// Interaction logic for EmployeeShiftLog.xaml
    /// </summary>
    public partial class EmployeeShiftLog : UserControl
    {
        readonly string FilterPlaceholder = "Поиск по табельному номеру..";
        public EmployeeShiftLog()
        {
            InitializeComponent();
            FilterSearch.Text = FilterPlaceholder;
            FilterNameAll.IsChecked = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUp.PlacementTarget = sender as Button;
            popUp.VerticalOffset = 5;
            popUp.IsOpen = true;
        }

        private void popUp_Closed(object sender, EventArgs e)
        {
            FilterSearch.Text = FilterPlaceholder;
        }

        private void FilterSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearch.Text == FilterPlaceholder)
                FilterSearch.Text = "";
        }

        private void FilterSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FilterSearch.Text == "")
                FilterSearch.Text = FilterPlaceholder;
        }
    }
}
