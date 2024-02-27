using System.Globalization;
using System.Runtime.InteropServices;
using System;
using System.Security;
using System.Windows.Data;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class SecureStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SecureString secureString)
            {
                return ConvertSecureStringToString(secureString);
            }
            else if (value is string str)
            {
                return ConvertStringToSecureString(str);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return ConvertStringToSecureString(str);
            }

            return null;
        }

        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr bstr = IntPtr.Zero;
            try
            {
                bstr = Marshal.SecureStringToBSTR(secureString);
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }
            }
        }

        private SecureString ConvertStringToSecureString(string str)
        {
            SecureString secureString = new SecureString();
            foreach (char c in str)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }
    }
}