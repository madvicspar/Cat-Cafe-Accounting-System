using System.Globalization;
using System;
using System.Windows.Data;
using System.Windows;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return ConvertBoolToVisibility(isVisible);
            }
            else if (value is Visibility visibility)
            {
                //return ConvertStringToSecureString(str);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Visibility ConvertBoolToVisibility(bool isVisible)
        {
            if (isVisible)
                return Visibility.Visible;
            return Visibility.Hidden;
        }
    }
}