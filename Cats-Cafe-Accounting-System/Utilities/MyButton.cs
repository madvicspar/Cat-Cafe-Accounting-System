using System.Windows.Controls;
using System.Windows;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class MyButton : RadioButton
    {
        static MyButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyButton), new FrameworkPropertyMetadata(typeof(MyButton)));
        }
    }
}
