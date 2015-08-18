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
using System.Windows.Shapes;
using IndustrialMonitoring.Lib;
using IndustrialMonitoring.UserServiceReference;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowChangePassword.xaml
    /// </summary>
    public partial class WindowChangePassword : Window
    {
        private UserServiceClient _userServiceClient;
        public WindowChangePassword()
        {
            InitializeComponent();
        }

        public UserServiceClient UserServiceClient
        {
            get { return _userServiceClient; }
            set { _userServiceClient = value; }
        }

        private void WindowChangePassword_OnLoaded(object sender, RoutedEventArgs e)
        {
            PasswordBoxOldPassword.Focus();
            TextBlockUsername.Text = Static.CurrentUser.UserName;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePassword();
        }

        private void ChangePassword()
        {
            string oldPassword = PasswordBoxOldPassword.Password;
            string newPassword = PasswordBoxNewPassword.Password;
            string confirmPassword = PasswordBoxConfirmPassword.Password;

            if (string.IsNullOrEmpty(oldPassword))
            {
                MessageBox.Show("Old password can not be empty");
                return;
            }

            if (Static.CurrentUser.Password != oldPassword)
            {
                MessageBox.Show("Old password is not correct");
                return;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("password can not be empty");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirm passwprd does not match");
                return;
            }

            int result = UserServiceClient.SetPassword(Static.CurrentUser.UserId, oldPassword, newPassword);

            if (result != 1)
            {
                MessageBox.Show("Error in saving password");
            }
            else
            {
                MessageBox.Show("Password saved successfully");
            }
        }

        private void ButtonSaveAndClose_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePassword();
            this.Close();
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
