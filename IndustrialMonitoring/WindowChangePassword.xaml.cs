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
using SharedLibrary;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowChangePassword.xaml
    /// </summary>
    public partial class WindowChangePassword : Window
    {
        private int _changePasswordCount = 0;
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

        public int ChangePasswordCount
        {
            get { return _changePasswordCount; }
            set { _changePasswordCount = value; }
        }

        private void WindowChangePassword_OnLoaded(object sender, RoutedEventArgs e)
        {
            PasswordBoxOldPassword.Focus();
            TextBlockUsername.Text = Static.CurrentUser.UserName;
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChangePassword())
            {
                ChangePasswordCount++;
            }
        }

        private bool ChangePassword()
        {
            try
            {
                string oldPassword = PasswordBoxOldPassword.Password;
                string newPassword = PasswordBoxNewPassword.Password;
                string confirmPassword = PasswordBoxConfirmPassword.Password;

                if (string.IsNullOrEmpty(oldPassword))
                {
                    MessageBox.Show("Old password can not be empty");
                    return false;
                }

                if (Static.CurrentUser.Password != Hash.GetHash(oldPassword))
                {
                    MessageBox.Show("Old password is not correct");
                    return false;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    MessageBox.Show("password can not be empty");
                    return false;
                }

                if (newPassword != confirmPassword)
                {
                    MessageBox.Show("New password and confirm passwprd does not match");
                    return false;
                }

                int result = UserServiceClient.SetPassword(Static.CurrentUser.UserId, oldPassword, newPassword);

                if (result != 1)
                {
                    MessageBox.Show("Error in saving password");
                    return false;
                }
                else
                {
                    MessageBox.Show("Password saved successfully");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void ButtonSaveAndClose_OnClick(object sender, RoutedEventArgs e)
        {
            if (ChangePassword())
            {
                ChangePasswordCount++;
                this.Close();
            }
        }

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
