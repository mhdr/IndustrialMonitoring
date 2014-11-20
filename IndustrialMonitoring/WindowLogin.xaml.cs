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
using IndustrialMonitoring.UserServiceReference;
using Telerik.Windows.Controls;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        private UserServiceClient _userServiceClient=new UserServiceClient();

        public WindowLogin()
        {
            StyleManager.ApplicationTheme =new Windows8Theme();
            InitializeComponent();
        }

        public UserServiceClient UserServiceClient
        {
            get { return _userServiceClient; }
            set { _userServiceClient = value; }
        }

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string userName = TextBoxUserName.Text;
            string password = PasswordBox.Password;

            bool result = UserServiceClient.Authorize(userName, password);

            if (result)
            {
                Lib.Static.CurrentUser = UserServiceClient.GetUserByUserName(userName);
                MainWindow mainWindow=new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ShowMsgOnStatusBar("Username or Password is wrong");
            }
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            ClearStatusBar();

            StatusBarBottom.Items.Add(msg);
        }

        private void ClearStatusBar()
        {
            StatusBarBottom.Items.Clear();
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearStatusBar();
        }

        private void WindowLogin_OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
