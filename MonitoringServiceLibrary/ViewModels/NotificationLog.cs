using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class NotificationLog
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public string NotificationMsg { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public bool HasFault { get; set; }

        [DataMember]
        public string Category { get; set; }

        public NotificationLog(int itemId,string itemName,string notificationMsg,DateTime dateTime,bool hasFault,string category)
        {
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.NotificationMsg = notificationMsg;
            this.DateTime = dateTime;
            this.HasFault = hasFault;
            this.Category = category;
        }
    }
}
