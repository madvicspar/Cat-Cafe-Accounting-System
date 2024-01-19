using System.Windows.Controls;
using System.Windows;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class Button : RadioButton
    {
        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        }
    }
}
