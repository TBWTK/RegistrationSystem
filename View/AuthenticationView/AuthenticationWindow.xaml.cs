
using System.Windows;

namespace RegistrationSystem.View.AuthenticationView
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
            PageLoginOrReg.Content = new LoginUserControl();
        }

        private void LoginPageChange(object sender, RoutedEventArgs e)
        {
            PageLoginOrReg.Content = new LoginUserControl();
        }

        private void RegistrationPageChange(object sender, RoutedEventArgs e)
        {
            PageLoginOrReg.Content = new RegistrationUserControl();
        }
    }
}
