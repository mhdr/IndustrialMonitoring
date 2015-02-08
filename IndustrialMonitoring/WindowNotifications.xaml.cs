using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using IndustrialMonitoring.ProcessDataServiceReference;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowNotifications.xaml
    /// </summary>
    public partial class WindowNotifications : Window
    {
        private int _itemId = 0;
        private NotificationServiceClient _notificationServiceClient;
        public event EventHandler<ShowNotificationsCompletedEventArgs> ShowDataCompleted;

        protected virtual void OnShowDataCompleted(ShowNotificationsCompletedEventArgs e)
        {
            EventHandler<ShowNotificationsCompletedEventArgs> handler = ShowDataCompleted;
            if (handler != null) handler(this, e);
        }

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
            this.ShowDataCompleted += WindowNotifications_ShowDataCompleted;
        }

        void WindowNotifications_ShowDataCompleted(object sender, ShowNotificationsCompletedEventArgs e)
        {
            var notifications = e.Notifications;

            foreach (NotificationLog notification in notifications)
            {
                NotificationListBoxUserControl notificationListBoxUserControl = new NotificationListBoxUserControl();
                notificationListBoxUserControl.SetItemName(notification.ItemName);
                notificationListBoxUserControl.SetTime(notification.DateTime);
                notificationListBoxUserControl.SetDesription(notification.NotificationMsg);
                notificationListBoxUserControl.SetHasFault(notification.HasFault);

                ListBoxNotification.Items.Add(notificationListBoxUserControl);
            }

            BusyIndicator.IsBusy = false;
        }

        private void ClearStatusBar()
        {
            StatusBarBottom.Items.Clear();
        }

        public void ShowData()
        {
            BusyIndicator.IsBusy = true;

            Thread t1 = new Thread(() => ShowDataAsync());
            t1.Priority = ThreadPriority.AboveNormal;
            t1.Start();
        }

        private void ShowDataAsync()
        {
            if (ItemId == 0)
            {
                var notifications = NotificationServiceClient.GetNotificationLogs(Static.CurrentUser.UserId,
    DateTime.Now - new TimeSpan(0, 24, 0, 0), DateTime.Now);

                if (notifications == null)
                {
                    return;
                }

                Dispatcher.BeginInvoke(new Action(() => OnShowDataCompleted(new ShowNotificationsCompletedEventArgs(notifications))));
            }
            else
            {
                List<NotificationLog> notifications = NotificationServiceClient.GetNotificationLog(Static.CurrentUser.UserId,ItemId,
DateTime.Now - new TimeSpan(0, 24, 0, 0), DateTime.Now);

                if (notifications == null)
                {
                    return;
                }

                Dispatcher.BeginInvoke(new Action(() => OnShowDataCompleted(new ShowNotificationsCompletedEventArgs(notifications))));
            }
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            ClearStatusBar();

            StatusBarBottom.Items.Add(msg);
        }
    }
}
