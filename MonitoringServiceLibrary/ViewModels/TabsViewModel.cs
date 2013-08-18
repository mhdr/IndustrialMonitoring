using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class TabsViewModel
    {
        [DataMember]
        public int TabId { get; set; }

        [DataMember]
        public string TabName { get; set; }

        public TabsViewModel()
        {
            
        }

        public TabsViewModel(Tab tab)
        {
            this.TabId = tab.TabId;
            this.TabName = tab.TabName;
        }
    }
}