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
    /// Interaction logic for VisitorLog.xaml
    /// </summary>
    public partial class VisitorLog : UserControl
    {
        public VisitorLog()
        {
            InitializeComponent();
            FilterTicketAll.IsChecked = true;
            FilterVisitorAll.IsChecked = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpVisitor.PlacementTarget = sender as Button;
            popUpVisitor.VerticalOffset = 5;
            popUpVisitor.IsOpen = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popUpTicket.PlacementTarget = sender as Button;
            popUpTicket.VerticalOffset = 5;
            popUpTicket.IsOpen = true;
        }
    }
}
