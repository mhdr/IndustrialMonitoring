using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class ItemsLogChartHistoryViewModel
    {
        [DataMember]
        public int ItemLogId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public System.DateTime Time { get; set; }

        public ItemsLogChartHistoryViewModel()
        {
            
        }

        public ItemsLogChartHistoryViewModel(ItemsLog itemLog)
        {
            this.ItemLogId = itemLog.ItemLogId;
            this.ItemId = itemLog.ItemId;
            this.Value = Convert.ToDouble(itemLog.Value);
            this.Time = itemLog.Time;
        }
    }
}