﻿using System;
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
using System.Windows.Shapes;

namespace RegistrationSystem.View.AuthenticationView
{
    /// <summary>
    /// Логика взаимодействия для AuthenticationWindow.xaml
    /// </summary>
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
            PageLoginOrReg.Content = new LoginUserControl();
        }

        private void LoginPageChange(object sender, RoutedEventArgs e)
        {
            PageLoginOrReg.Content = new LoginUserControl();
        }

        private void RegistrationPageChange(object sender, RoutedEventArgs e)
        {
            PageLoginOrReg.Content = new RegistrationUserControl();
        }
    }
}
