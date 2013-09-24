using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class TabItems2
    {
        [DataMember]
        public string Item { get; set; }

        [DataMember]
        public List<TabItems2> Items { get; set; } 

    }
}
