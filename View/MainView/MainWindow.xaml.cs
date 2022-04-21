using System.Windows;
using RegistrationSystem.View.MainView.Account;
using RegistrationSystem.View.MainView.AdminPatchingUsers;
using System.Windows.Threading;
using System;
using RegistrationSystem.Model;
using System.Linq;
using System.Windows.Input;

namespace RegistrationSystem.View.MainView
{

    public partial class MainWindow : Window
    {
        int idUser;
        int tick = 0;
        Point now = new Point(0, 0);
        DispatcherTimer timer = new DispatcherTimer();
        public MainWindow(int ID)
        {
            idUser = ID;
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();


            using (var context = new TestDataBaseEntities())
            {
                var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                if (us != null)
                {
                    if (us.Statuses.NameStatus != "Active")
                    {
                        AdminPButtonUsers.Visibility = Visibility.Hidden;
                    }
                }
            }


            PageChange.Content = new PersonalAccountUserControl(idUser);
        }

        private void AccountPageChange(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PersonalAccountUserControl(idUser);
        }

        private void PatchingUsersPage(object sender, RoutedEventArgs e)
        {
            PageChange.Content = new PatchingUserControl();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            ++tick;
            if (tick == 20)
            {
                timer.Stop();
                AuthenticationView.AuthenticationWindow authentication = new AuthenticationView.AuthenticationWindow();
                this.Close();
                authentication.Show();
            }
        }

        private void Window_KeyDownAndUp(object sender, KeyEventArgs e)
        {
            tick = 0;
        }

        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tick = 0;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point pp = e.GetPosition(this);
            if (pp != now)
            {
                tick = 0;
            }
            now = pp;
        }
    }
}
