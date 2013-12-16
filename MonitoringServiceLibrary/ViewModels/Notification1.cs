using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Notification1
    {
        [DataMember]
        public int NotificationId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string NotificationMsg { get; set; }

        public Notification1(NotificationItem notificationItem)
        {
            this.NotificationId = notificationItem.NotificationId;
            this.ItemId = notificationItem.ItemId;
            this.NotificationMsg = notificationItem.NotificationMsg;
        }
    }
}
