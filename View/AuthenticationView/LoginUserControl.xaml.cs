using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RegistrationSystem.View.MainView;

namespace RegistrationSystem.View.AuthenticationView
{
    /// <summary>
    /// Логика взаимодействия для LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
        }

        private void CheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = PassPassBox.Password.Trim();
            PassPassBox.Visibility = Visibility.Hidden;
            PassTextBlock.Visibility = Visibility.Visible;
        }

        private void UnCheckPassword(object sender, MouseButtonEventArgs e)
        {
            PassTextBlock.Text = "";
            PassTextBlock.Visibility = Visibility.Hidden;
            PassPassBox.Visibility = Visibility.Visible;
        }

        private void EnterSystem(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
