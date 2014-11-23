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

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowNotifications.xaml
    /// </summary>
    public partial class WindowNotifications : Window
    {
        public WindowNotifications()
        {
            InitializeComponent();
        }

        private void MenuItemShowSetTimeDialog_OnClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void MenuItemShowSetTimeDialog_OnMouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void WindowNotifications_OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationListBoxUserControl notificationListBoxUserControl1=new NotificationListBoxUserControl();
            notificationListBoxUserControl1.SetItemName("Item 1");
            notificationListBoxUserControl1.SetTime(DateTime.Now);
            notificationListBoxUserControl1.SetDesription("Item 1 description");
            notificationListBoxUserControl1.SetHasFault(false);

            NotificationListBoxUserControl notificationListBoxUserControl2 = new NotificationListBoxUserControl();
            notificationListBoxUserControl2.SetItemName("Item 2");
            notificationListBoxUserControl2.SetTime(DateTime.Now);
            notificationListBoxUserControl2.SetDesription("Item 2 description");
            notificationListBoxUserControl2.SetHasFault(true);

            NotificationListBoxUserControl notificationListBoxUserControl3 = new NotificationListBoxUserControl();
            notificationListBoxUserControl3.SetItemName("Item 2");
            notificationListBoxUserControl3.SetTime(DateTime.Now);
            notificationListBoxUserControl3.SetDesription("Item 2 description");
            notificationListBoxUserControl3.SetHasFault(true);

            ListBoxNotification.Items.Add(notificationListBoxUserControl1);
            ListBoxNotification.Items.Add(notificationListBoxUserControl2);
            ListBoxNotification.Items.Add(notificationListBoxUserControl3);

        }
    }
}
