using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RegistrationSystem.View.MainView;
using RegistrationSystem.Model;
using System.Linq;
using System.Windows.Media.Imaging;
using System.IO;

namespace RegistrationSystem.View.AuthenticationView
{
    public partial class LoginUserControl : UserControl
    {
        string captchaCode;
        byte iteratorLogin;
        byte iteratorCaptcha;

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
            if(iteratorLogin < 2)
            {
                using (var context = new TestDataBaseEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Login == LoginTextBox.Text && x.Password == PassPassBox.Password.Trim());
                    if (us != null)
                    {
                        MainWindow mainWindow = new MainWindow(us.Id);
                        mainWindow.Show();
                        Application.Current.MainWindow.Close();
                    }
                    else
                    {
                        iteratorLogin++;
                        MessageBox.Show("Неверные данные или пользователя не существует!");
                    }
                }
            }
            else
            {
                BorderLogin.Visibility = Visibility.Hidden;
                iteratorLogin = 0;
                BorderCaptcha.Visibility = Visibility.Visible;
                LoadContentCapha();
            }
        }

        private void UpdateCaptha_Click(object sender, RoutedEventArgs e)
        {
            LoadContentCapha();
        }

        private void CheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if(CapthaTextBox.Text == captchaCode)
            {
                BorderLogin.Visibility = Visibility.Visible;
                BorderCaptcha.Visibility = Visibility.Hidden;
                iteratorCaptcha = 0;
            }
            else
            {
                iteratorCaptcha++;
                MessageBox.Show("Введены неверные данные!");
            }
        }

        public void LoadContentCapha()
        {
            Random random = new Random();
            int a = random.Next(1, 100);
            int b = random.Next(1, 100);
            int c = random.Next(1, 100);
            int x = a + b - c;        
            CapthaTextBlock.Text = $"{Convert.ToString(a)} + {Convert.ToString(b)} - {Convert.ToString(c)}";
            captchaCode = Convert.ToString(x);
        }

    }
}
