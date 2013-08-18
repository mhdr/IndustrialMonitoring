using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class TabsItemsViewModel
    {
        [DataMember]
        public int TabItemId { get; set; }

        [DataMember]
        public int TabId { get; set; }

        [DataMember]
        public int ItemId { get; set; }

        public TabsItemsViewModel()
        {
            
        }

        public TabsItemsViewModel(TabsItem tabsItem)
        {
            this.TabItemId = tabsItem.TabItemId;
            this.TabId = tabsItem.TabId;
            this.ItemId = tabsItem.TabItemId;
        }
    }
}