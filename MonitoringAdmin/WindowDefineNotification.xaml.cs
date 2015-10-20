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
using MonitoringAdmin.Lib;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowDefineNotification.xaml
    /// </summary>
    public partial class WindowDefineNotification : Window
    {
        private NotificationViewModel _notification=null;
        private int _notificationId=0;
        private int _itemId;
        private WindowNotifications _reference;
        public WindowDefineNotification()
        {
            InitializeComponent();
        }

        public NotificationViewModel Notification
        {
            get { return _notification; }
            set { _notification = value; }
        }

        public int NotificationId
        {
            get { return _notificationId; }
            set { _notificationId = value; }
        }

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public WindowNotifications Reference
        {
            get { return _reference; }
            set { _reference = value; }
        }

        private void WindowDefineNotification_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataFormNotification.CurrentItem = Notification;
        }

        private void WindowDefineNotification_OnClosed(object sender, EventArgs e)
        {
            if (NotificationId > 0)
            {
                Reference.EditNotification(this.NotificationId,this.ItemId,(NotificationViewModel) DataFormNotification.CurrentItem);
            }
            else
            {
                Reference.AddNotification(this.ItemId, (NotificationViewModel)DataFormNotification.CurrentItem);
            }
        }
    }
}
