using System.Windows;
using RegistrationSystem.View.MainView.Account;
using RegistrationSystem.View.MainView.AdminPatchingUsers;


namespace RegistrationSystem.View.MainView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PageChange.Content = new PersonalAccountUserControl();
        }

        private void AccountPageChange(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PersonalAccountUserControl();
        }

        private void PatchingUsersPage(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PatchingUsers();
        }
    }
}
