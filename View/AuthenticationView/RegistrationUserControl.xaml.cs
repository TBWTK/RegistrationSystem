using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RegistrationSystem.View.MainView;
using RegistrationSystem.Model;
using System.Linq;



namespace RegistrationSystem.View.AuthenticationView
{
    public partial class RegistrationUserControl : UserControl
    {
        public RegistrationUserControl()
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

        private void RegistrationSystem(object sender, RoutedEventArgs e)
        {
            if(PassPassBoxOne.Password != null && PassPassBoxTwo.Password != null && (PassPassBoxOne.Password == PassPassBoxTwo.Password))
            {
                using (var context = new TestDataBaseEntities())
                {
                    var std = new Users()
                    {
                        Login = LoginTextBox.Text,
                        Password = PassPassBoxOne.Password.Trim()
                    };
                    context.Users.Add(std);
                    context.SaveChanges();

                    var us = context.Users.SingleOrDefault(x => x.Login == LoginTextBox.Text);
                    if (us != null)
                    {
                        MainWindow mainWindow = new MainWindow(us.Id);
                        mainWindow.Show();
                        Application.Current.MainWindow.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Для прохождения успешной регистрации введите два раза одинаковый пароль!");
            }
        }
    }
}
