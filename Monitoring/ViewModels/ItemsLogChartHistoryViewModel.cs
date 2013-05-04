using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Monitoring.ViewModels
{
    [DataContract]
    public class ItemsLogChartHistoryViewModel
    {
        [DataMember]
        public int ItemLogId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public System.DateTime Time { get; set; }

        public ItemsLogChartHistoryViewModel()
        {
            
        }

        public ItemsLogChartHistoryViewModel(ItemsLog itemLog)
        {
            this.ItemLogId = itemLog.ItemLogId;
            this.ItemId = itemLog.ItemId;
            this.Value = itemLog.Value;
            this.Time = itemLog.Time;
        }
    }
}