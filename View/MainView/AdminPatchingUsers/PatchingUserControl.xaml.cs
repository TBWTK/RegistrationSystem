using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using RegistrationSystem.Core;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.AdminPatchingUsers
{
    /// <summary>
    /// Логика взаимодействия для PatchingUserControl.xaml
    /// </summary>
    public partial class PatchingUserControl : UserControl
    {
        public List<Users> users { get; set; }
        public List<Genders> genders { get; set; }
        public List<Statuses> statuses { get; set; }
        public List<Roles> roles { get; set; }

        private Image imageDefault { get; set; }
        private Users HandlerUser { get; set; }
        private Genders HandlerGender { get; set; }
        private Statuses HandlerStatus { get; set; }
        private Roles HandlerRoles { get; set; }
        public PatchingUserControl()
        {
            InitializeComponent();

            imageDefault = new Image();
            imageDefault.Source = PhotoUser.Source;
            HandlerUser = new Users();
            HandlerGender = new Genders();
            HandlerStatus = new Statuses();
            HandlerRoles = new Roles();

            users = new List<Users>();
            genders = new List<Genders>();
            statuses = new List<Statuses>();
            roles = new List<Roles>();

            using (var context = new TestDataBaseEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                genders = context.Genders.ToList();
                statuses = context.Statuses.ToList();
                roles = context.Roles.ToList();
            }
            LoadDataUsers();
            // Заполнение комобоксов для поиска в датагрид
            StatusesComboBox.ItemsSource = statuses;
            RolesComboBox.ItemsSource = roles;
            GendersComboBox.ItemsSource = genders;

            // Заполнение комбоксов для создания или редактирования пользователя
            GenderComboBox.ItemsSource = genders;
            StatusComboBox.ItemsSource = statuses;
            RoleComboBox.ItemsSource = roles;
        }
        private void LoadDataUsers()
        {
            using (var context = new TestDataBaseEntities())
            {
                context.Configuration.ProxyCreationEnabled = false;
                users = context.Users.ToList();
            }
            foreach (var user in users)
            {
                foreach (var gender in genders)
                    if (user.Gender == gender.Id)
                        user.UserGender = gender.NameGender;
                foreach (var role in roles)
                    if (user.Role == role.Id)
                        user.UserRole = role.NameRole;
                foreach (var status in statuses)
                    if (user.Status == status.Id)
                        user.UserStatus = status.NameStatus;
            }
            DataGridUsers.ItemsSource = users;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = users.Where(users => users.Login.StartsWith(SearchTextBox.Text) || users.Name.StartsWith(SearchTextBox.Text) || users.LastName.StartsWith(SearchTextBox.Text) || users.SurName.StartsWith(SearchTextBox.Text));
            DataGridUsers.ItemsSource = filtered;
        }

        private void StatusesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = StatusesComboBox.SelectedIndex;
            if(selectedIndex > -1)
            {
                Statuses selectedItem = (Statuses)StatusesComboBox.SelectedItem;
                var filtered = users.Where(users => users.Statuses.NameStatus.StartsWith(selectedItem.NameStatus));
                DataGridUsers.ItemsSource = filtered;
            }
        }

        private void RolesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = RolesComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Roles selectedItem = (Roles)RolesComboBox.SelectedItem;
                var filtered = users.Where(users => users.Roles.NameRole.StartsWith(selectedItem.NameRole));
                DataGridUsers.ItemsSource = filtered;
            }
        }

        private void GendersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = GendersComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Genders selectedItem = (Genders)GendersComboBox.SelectedItem;
                var filtered = users.Where(users => users.Genders.NameGender.StartsWith(selectedItem.NameGender));
                DataGridUsers.ItemsSource = filtered;
            }
        }

        private void ClearButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DataGridUsers.ItemsSource = users;
            StatusesComboBox.SelectedIndex = -1;
            RolesComboBox.SelectedIndex = -1;
            GendersComboBox.SelectedIndex = -1;
            SearchTextBox.Text = "";
        }

        private void DataGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = DataGridUsers.SelectedIndex;
            if (selectedIndex > -1 && selectedIndex < users.Count)
            {
                Users selectedItem = (Users)DataGridUsers.SelectedItem;
                HandlerUser = selectedItem;
                SurNameTextBox.Text = selectedItem.SurName;
                NameTextBox.Text = selectedItem.Name;
                LastNameTextBox.Text = selectedItem.LastName;
                LoginTextBox.Text = selectedItem.Login;
                PasswordTextBox.Text = selectedItem.Password;
                GenderTextBox.Text = selectedItem.UserGender;
                RoleTextBox.Text = selectedItem.UserRole;
                StatusTextBox.Text = selectedItem.UserStatus;
                if (selectedItem.PhotoUser != null)
                    PhotoUser.Source = byteArrayToImage(selectedItem.PhotoUser).Source;
                else
                {
                    PhotoUser.Source = imageDefault.Source;
                }
            }
        }
        public Image byteArrayToImage(byte[] byteArray)
        {
            Image image = new Image();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }
            return image;
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = GenderComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                HandlerGender = (Genders)GenderComboBox.SelectedItem;
                GenderTextBox.Text = HandlerGender.NameGender;
            }
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = RoleComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                HandlerRoles = (Roles)RoleComboBox.SelectedItem;
                RoleTextBox.Text = HandlerRoles.NameRole;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = StatusComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                HandlerStatus = (Statuses)StatusComboBox.SelectedItem;
                StatusTextBox.Text = HandlerStatus.NameStatus;
            }
        }

        private void UpdateUserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (HandlerUser != null)
            {
                using (var context = new TestDataBaseEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Id == HandlerUser.Id);
                    if (us != null)
                    {
                        us.Name = NameTextBox.Text;
                        us.SurName = SurNameTextBox.Text;
                        us.LastName = LastNameTextBox.Text;
                        us.Login = LoginTextBox.Text;
                        us.Password = PasswordTextBox.Text;
                        if (HandlerRoles.NameRole != null)
                            us.Role = HandlerRoles.Id;
                        else
                        {
                            foreach(var item in roles)
                            {
                                if (item.NameRole == RoleTextBox.Text)
                                    us.Role = item.Id;
                            }
                        }
                        if (HandlerGender.NameGender != null)
                            us.Gender = HandlerGender.Id;
                        else
                        {
                            foreach (var item in genders)
                            {
                                if (item.NameGender == GenderTextBox.Text)
                                    us.Gender = item.Id;
                            }
                        }
                        if (HandlerStatus.NameStatus != null)
                            us.Status = HandlerStatus.Id;
                        else
                        {
                            foreach (var item in statuses)
                            {
                                if (item.NameStatus == StatusTextBox.Text)
                                    us.Status = item.Id;
                            }
                        }
                        context.Entry(us).State = EntityState.Modified;
                        context.SaveChanges();
                        System.Windows.MessageBox.Show("Изменения прошли успешно");

                        LoadDataUsers();

                    }
                }
            }
        }

        private void ClearItem()
        {
            SurNameTextBox.Text = "";
            NameTextBox.Text = "";
            LastNameTextBox.Text = "";
            LoginTextBox.Text = "";
            PasswordTextBox.Text = "";
            GenderTextBox.Text = "";
            RoleTextBox.Text = "";
            StatusTextBox.Text = "";
            StatusComboBox.SelectedIndex = -1;
            RoleComboBox.SelectedIndex = -1;
            GenderComboBox.SelectedIndex = -1;
        }

        private void CreateAccountTextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CreateAccountTextBlock.Visibility = System.Windows.Visibility.Hidden;
            BackToPatchingUserPanel.Visibility = System.Windows.Visibility.Visible;
            BlockPhoto.Visibility = System.Windows.Visibility.Hidden;
            PanelDisplayUsers.Visibility = System.Windows.Visibility.Hidden;
            UpdateUserButton.Visibility = System.Windows.Visibility.Hidden;
            CreateUserButton.Visibility = System.Windows.Visibility.Visible;
            ClearItem();
        }

        private void CreateUserButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Roles choiceRole = new Roles();
                if (HandlerRoles.NameRole != null)
                    choiceRole.Id = HandlerRoles.Id;

                Genders choiceGender = new Genders();
                if (HandlerGender.NameGender != null)
                    choiceGender.Id = HandlerGender.Id;

                Statuses choiceStatus = new Statuses();
                if (HandlerStatus.NameStatus != null)
                    choiceStatus.Id = HandlerStatus.Id;

                using (var context = new TestDataBaseEntities())
                {
                    var us = new Users()
                    {
                        Name = NameTextBox.Text,
                        SurName = SurNameTextBox.Text,
                        LastName = LastNameTextBox.Text,
                        Login = LoginTextBox.Text,
                        Password = PasswordTextBox.Text,
                        Role = choiceRole?.Id,
                        Gender = choiceGender?.Id,
                        Status = choiceStatus?.Id
                    };
                    context.Users.Add(us);
                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Пользователь зарегестрирован");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Введите корректные данные");
            }


            CreateAccountTextBlock.Visibility = System.Windows.Visibility.Hidden;
            BackToPatchingUserPanel.Visibility = System.Windows.Visibility.Visible;
            BlockPhoto.Visibility = System.Windows.Visibility.Visible;
            PanelDisplayUsers.Visibility = System.Windows.Visibility.Visible;
            UpdateUserButton.Visibility = System.Windows.Visibility.Visible;
            CreateUserButton.Visibility = System.Windows.Visibility.Hidden;
            LoadDataUsers();
            ClearItem();
        }

        private void BackToPatchingUserPanel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CreateAccountTextBlock.Visibility = System.Windows.Visibility.Visible;
            BackToPatchingUserPanel.Visibility = System.Windows.Visibility.Hidden;
            BlockPhoto.Visibility = System.Windows.Visibility.Visible;
            PanelDisplayUsers.Visibility = System.Windows.Visibility.Visible;
            UpdateUserButton.Visibility = System.Windows.Visibility.Visible;
            CreateUserButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
