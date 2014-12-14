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
using IndustrialMonitoring.NotificationServiceReference;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowNotifications.xaml
    /// </summary>
    public partial class WindowNotifications : Window
    {
        private int _itemId = 0;
        private NotificationServiceClient _notificationServiceClient;

        public WindowNotifications()
        {
            InitializeComponent();
        }

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public NotificationServiceClient NotificationServiceClient
        {
            get { return _notificationServiceClient; }
            set { _notificationServiceClient = value; }
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
            var notifications= NotificationServiceClient.GetNotificationLogs(Static.CurrentUser.UserId,
                DateTime.Now - new TimeSpan(0, 24, 0, 0), DateTime.Now);

            if (notifications == null)
            {
                return;
            }

            foreach (NotificationLog notification in notifications)
            {
                NotificationListBoxUserControl notificationListBoxUserControl=new NotificationListBoxUserControl();
                notificationListBoxUserControl.SetItemName(notification.ItemName);
                notificationListBoxUserControl.SetTime(notification.DateTime);
                notificationListBoxUserControl.SetDesription(notification.NotificationMsg);
                notificationListBoxUserControl.SetHasFault(notification.HasFault);

                ListBoxNotification.Items.Add(notificationListBoxUserControl);
            }

        }
    }
}
