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

namespace RegistrationSystem.View.MainView.Account
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordUser.xaml
    /// </summary>
    public partial class ChangePasswordUser : UserControl
    {
        public ChangePasswordUser()
        {
            InitializeComponent();
        }

        private void CheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = PassPassBoxOne.Password.Trim();
            PassPassBoxOne.Visibility = Visibility.Hidden;
            PassTextBlockOne.Visibility = Visibility.Visible;
        }

        private void UnCheckPasswordOne(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockOne.Text = "";
            PassTextBlockOne.Visibility = Visibility.Hidden;
            PassPassBoxOne.Visibility = Visibility.Visible;
        }

        private void CheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = PassPassBoxTwo.Password.Trim();
            PassPassBoxTwo.Visibility = Visibility.Hidden;
            PassTextBlockTwo.Visibility = Visibility.Visible;
        }

        private void UnCheckPasswordTwo(object sender, MouseButtonEventArgs e)
        {
            PassTextBlockTwo.Text = "";
            PassTextBlockTwo.Visibility = Visibility.Hidden;
            PassPassBoxTwo.Visibility = Visibility.Visible;
        }
        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
            PanelPassword.Visibility = Visibility.Hidden;
        }
    }
}
