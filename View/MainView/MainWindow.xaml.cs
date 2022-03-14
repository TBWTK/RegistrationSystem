using System.Windows;
using RegistrationSystem.View.MainView.Account;
using RegistrationSystem.View.MainView.AdminPatchingUsers;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int idUser;

        public MainWindow(int ID)
        {
            idUser = ID;
            InitializeComponent();
            //if (users.Role != 1)
            //    AdminPButtonUsers.Visibility = Visibility.Hidden;
            PageChange.Content = new PersonalAccountUserControl(idUser);
        }

        private void AccountPageChange(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PersonalAccountUserControl(idUser);
        }

        private void PatchingUsersPage(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PatchingUsers();
        }
    }
}
