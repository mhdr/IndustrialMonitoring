using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using SharedLibrary;

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

        private DateTime _startTime;
        private DateTime _endTime;

        protected virtual void OnShowDataCompleted(ShowNotificationsCompletedEventArgs e)
        {
            EventHandler<ShowNotificationsCompletedEventArgs> handler = ShowDataCompleted;
            if (handler != null) handler(this, e);
        }

        public WindowNotifications()
        {
            InitializeComponent();
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
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
            try
            {
                DialogSetTime dialogSetTime = new DialogSetTime();
                dialogSetTime.TimeChanged += dialogSetTime_TimeChanged;
                dialogSetTime.StartTime = StartTime;
                dialogSetTime.EndTime = EndTime;
                dialogSetTime.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        void dialogSetTime_TimeChanged(object sender, Lib.TimeChangedEventArgs e)
        {
            try
            {
                this.StartTime = e.StartTime;
                this.EndTime = e.EndTime;

                ShowData();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemShowSetTimeDialog_OnMouseEnter(object sender, MouseEventArgs e)
        {

            try
            {
                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = "Start Time : ";
                textBlock1.FontWeight = FontWeights.Bold;

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = StartTime.ToString();

                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Orientation = Orientation.Horizontal;

                stackPanel1.Children.Add(textBlock1);
                stackPanel1.Children.Add(textBlock2);

                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = "End Time : ";
                textBlock3.FontWeight = FontWeights.Bold;

                TextBlock textBlock4 = new TextBlock();
                textBlock4.Text = EndTime.ToString();

                StackPanel stackPanel2 = new StackPanel();
                stackPanel2.Orientation = Orientation.Horizontal;

                stackPanel2.Children.Add(textBlock3);
                stackPanel2.Children.Add(textBlock4);

                StackPanel stackPanel3 = new StackPanel();
                stackPanel3.Children.Add(stackPanel1);
                stackPanel3.Children.Add(stackPanel2);

                MenuItemShowSetTimeDialog.ToolTip = stackPanel3;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ClearStatusBar();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void WindowNotifications_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowDataCompleted += WindowNotifications_ShowDataCompleted;
        }

        void WindowNotifications_ShowDataCompleted(object sender, ShowNotificationsCompletedEventArgs e)
        {
            try
            {
                ListBoxNotification.Items.Clear();
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ClearStatusBar()
        {
            StatusBarBottom.Items.Clear();
        }

        public void ShowData()
        {
            try
            {
                BusyIndicator.IsBusy = true;

                Thread t1 = new Thread(() => ShowDataAsync());
                t1.Priority = ThreadPriority.AboveNormal;
                t1.Start();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ShowDataAsync()
        {
            try
            {
                if (ItemId == 0)
                {
                    var notifications = NotificationServiceClient.GetNotificationLogs(Static.CurrentUser.UserId,this.StartTime, this.EndTime);

                    if (notifications == null)
                    {
                        return;
                    }

                    Dispatcher.BeginInvoke(new Action(() => OnShowDataCompleted(new ShowNotificationsCompletedEventArgs(notifications))));
                }
                else
                {
                    List<NotificationLog> notifications = NotificationServiceClient.GetNotificationLog(Static.CurrentUser.UserId, ItemId,
    this.StartTime, this.EndTime);

                    if (notifications == null)
                    {
                        return;
                    }

                    Dispatcher.BeginInvoke(new Action(() => OnShowDataCompleted(new ShowNotificationsCompletedEventArgs(notifications))));
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            try
            {
                ClearStatusBar();

                StatusBarBottom.Items.Add(msg);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }
    }
}
