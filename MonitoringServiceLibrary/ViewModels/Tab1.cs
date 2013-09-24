using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Tab1
    {
        [DataMember]
        public int TabId { get; set; }

        [DataMember]
        public string TabName { get; set; }

        public Tab1()
        {
            
        }

        public Tab1(Tab tab)
        {
            this.TabId = tab.TabId;
            this.TabName = tab.TabName;
        }
    }
}