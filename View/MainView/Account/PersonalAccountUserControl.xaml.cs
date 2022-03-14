using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using RegistrationSystem.Model;

namespace RegistrationSystem.View.MainView.Account
{
    public partial class PersonalAccountUserControl : UserControl
    {
        int idUser;
        List<Genders> genders;
        string choiceGender;
        public PersonalAccountUserControl(int ID)
        {
            idUser = ID;
            InitializeComponent();
            using (var context = new TestDataBaseEntities())
            {
                genders = context.Genders.ToList();
                foreach (var i in genders)
                    BoxGender.Items.Add(i.NameGender);
            }
            LoadUser();
        }

        private void LoadUser()
        {
            using (var context = new TestDataBaseEntities())
            {
                var user = context.Users.SingleOrDefault(x => x.Id == idUser);
                if (user != null)
                {
                    if (user.Name != null)
                        NameTextBox.Text = user.Name;
                    if (user.SurName != null)
                        SurNameTextBox.Text = user.SurName;
                    if (user.LastName != null)
                        LastNameTextBox.Text = user.LastName;
                    if (user.PhotoUser != null)
                    {
                        Image image = byteArrayToImage(user.PhotoUser);
                        PhotoUser.Source = image.Source;
                    }
                    if (user.Genders != null)
                        TextGender.Text = user.Genders.NameGender;
                }
            }
        }


        public Image byteArrayToImage(byte[] byteArray)
        {
            Image image = new Image();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None,BitmapCacheOption.OnLoad);
            }
            return image;
        }
        public byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
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
                using (var context = new TestDataBaseEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                    if(us != null)
                    {
                        us.PhotoUser = getJPGFromImageControl(PhotoUser.Source as BitmapImage);
                        context.SaveChanges();
                        MessageBox.Show("Изменения прошли успешно");
                    }
                }
                LoadUser();
            }
        }

        private void UpdateNameUser(object sender, RoutedEventArgs e)
        {
            using (var context = new TestDataBaseEntities())
            {
                var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                if (us != null)
                {
                    us.Name = NameTextBox.Text;
                    us.SurName = SurNameTextBox.Text;
                    us.LastName = LastNameTextBox.Text;
                    context.Entry(us).State = EntityState.Modified;
                    context.SaveChanges();
                    MessageBox.Show("Изменения прошли успешно");

                }
            }
            LoadUser();
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

            using (var context = new TestDataBaseEntities())
            {
                var us = context.Users.SingleOrDefault(x => x.Id == idUser && x.Password == PassPassBox.Password.Trim());

                if (PassPassBoxOne.Password == PassPassBoxTwo.Password && us != null)
                {
                    us.Password = PassPassBoxOne.Password.Trim();
                    context.Entry(us).State = EntityState.Modified;
                    context.SaveChanges();
                    PassPassBox.Clear();
                    PassPassBoxOne.Clear();
                    PassPassBoxTwo.Clear();
                    MessageBox.Show("Изменения прошли успешно");

                }
                else
                {
                    MessageBox.Show("ВЫ ВВЕЛИ НЕВЕРНЫЙ ПАРОЛЬ!");
                }
            }
        }

        private void BoxRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = BoxGender.SelectedIndex;
            Object selectedItem = BoxGender.SelectedItem;
            choiceGender = selectedItem.ToString();
        }
        private void UpdateGender(object sender, RoutedEventArgs e)
        {
            if(choiceGender != null)
            {
                using (var context = new TestDataBaseEntities())
                {
                    var us = context.Users.SingleOrDefault(x => x.Id == idUser);
                    if (us != null)
                    {
                        foreach(var i in genders)
                        {
                            if(i.NameGender == choiceGender)
                            {
                                us.Gender = i.Id;
                                context.Entry(us).State = EntityState.Modified;
                                context.SaveChanges();
                                MessageBox.Show("Изменения прошли успешно");
                            }
                        }
                    }
                }
                LoadUser();
            }
            else
            {
                MessageBox.Show("Выбирете гендер");
            }
        }
    }
}
