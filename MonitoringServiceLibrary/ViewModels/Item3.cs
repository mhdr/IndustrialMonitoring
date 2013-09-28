using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Item3
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        public Item3(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = item.ItemName;
        }
    }
}
