using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndustrialMonitoring.NotificationServiceReference;

namespace IndustrialMonitoring.Lib
{
    public class ShowNotificationsCompletedEventArgs : EventArgs
    {
        private List<NotificationLog> _notifications;

        public List<NotificationLog> Notifications
        {
            get { return _notifications; }
            set { _notifications = value; }
        }

        public ShowNotificationsCompletedEventArgs(List<NotificationLog> notifications)
        {
            this.Notifications = notifications;
        }
    }
}
