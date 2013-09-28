using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Tab2
    {
        [DataMember]
        public int TabId { get; set; }

        [DataMember]
        public string TabName { get; set; }

        [DataMember]
        List<Item3> Items { get; set; } 

        public Tab2(Tab tab,List<Item3> items)
        {
            this.TabId = tab.TabId;
            this.TabName = tab.TabName;
            this.Items = items;
        }
    }
}
