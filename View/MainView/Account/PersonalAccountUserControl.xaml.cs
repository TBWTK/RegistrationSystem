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
    /// Логика взаимодействия для PersonalAccountUserControl.xaml
    /// </summary>
    public partial class PersonalAccountUserControl : UserControl
    {
        public PersonalAccountUserControl()
        {
            InitializeComponent();
        }

        private void DowlandPhoto(object sender, RoutedEventArgs e)
        {
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            Microsoft.Win32.OpenFileDialog ofdPicture = new Microsoft.Win32.OpenFileDialog();
            ofdPicture.Filter =
                "Image files|*.bmp;*.jpg;*.JPG;*.gif;*.png;*.tif|All files|*.*";
            ofdPicture.FilterIndex = 1;

            if (ofdPicture.ShowDialog() == true)
            {
                myBitmapImage.UriSource = new Uri(ofdPicture.FileName);
                myBitmapImage.EndInit();
                PhotoUser.Source = myBitmapImage;
            }
        }

        private void UnlockChangePanel(object sender, RoutedEventArgs e)
        {
            InfoNameUser.Visibility = Visibility.Hidden;
            ChangeNameUser.Visibility = Visibility.Visible;
        }

        private void UpdateNameUser(object sender, RoutedEventArgs e)
        {
            InfoNameUser.Visibility = Visibility.Visible;
            ChangeNameUser.Visibility = Visibility.Hidden;
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePasswordContentControl.Content = new ChangePasswordUser();
        }

    }
}
