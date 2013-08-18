using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class ItemsLogLatestAIOViewModel
    {
        [DataMember]
        public int ItemLogLatestId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public DateTime Time { get; set; }


        public ItemsLogLatestAIOViewModel()
        {
            
        }

        public ItemsLogLatestAIOViewModel(ItemsLogLatest itemsLogLatest)
        {
            this.ItemLogLatestId = itemsLogLatest.ItemLogLatestId;
            this.ItemId = itemsLogLatest.ItemId;
            this.Time = itemsLogLatest.Time;
            this.Value = itemsLogLatest.Value;
        }
    }
}