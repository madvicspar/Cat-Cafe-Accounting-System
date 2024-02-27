using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class SecureStringBinding : DependencyObject
    {
        public static readonly DependencyProperty SecureTextProperty =
            DependencyProperty.RegisterAttached("SecureText", typeof(SecureString), typeof(SecureStringBinding), new FrameworkPropertyMetadata(null, OnSecureTextChanged));

        public static SecureString GetSecureText(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(SecureTextProperty);
        }

        public static void SetSecureText(DependencyObject obj, SecureString value)
        {
            obj.SetValue(SecureTextProperty, value);
        }

        private static void OnSecureTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= OnSecureTextPasswordChanged;
                passwordBox.PasswordChanged += OnSecureTextPasswordChanged;
            }
        }

        private static void OnSecureTextPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetSecureText(passwordBox, ConvertToSecureString(passwordBox.Password));
            }
        }

        private static SecureString ConvertToSecureString(string text)
        {
            SecureString secureString = new SecureString();
            foreach (char c in text)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }
    }
}