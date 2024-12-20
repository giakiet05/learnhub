﻿using LearnHub.Commands;
using LearnHub.ViewModels.AdminViewModels;
using LearnHub.ViewModels.AuthenticationViewModels;
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

namespace LearnHub.Views.AuthenticationViews
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbUsername.Clear();
            tbUsername.Focus();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ICommand navigate= new NavigateViewCommand(()=> new AdminViewModel());
            navigate.Execute(this);
        }
      
    }
}
