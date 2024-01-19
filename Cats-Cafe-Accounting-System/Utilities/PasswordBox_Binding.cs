using System.Security;
using System.Windows.Controls;
using System.Windows;

namespace Cats_Cafe_Accounting_System.Utilities
{
    public class PasswordBox_Binding : DependencyObject
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(SecureString), typeof(PasswordBox_Binding), new FrameworkPropertyMetadata(new SecureString(), OnPasswordPropertyChanged));

        public static SecureString GetPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= OnPasswordChanged;
                passwordBox.PasswordChanged += OnPasswordChanged;
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                SetPassword(passwordBox, passwordBox.SecurePassword.Copy());
            }
        }
    }
}
