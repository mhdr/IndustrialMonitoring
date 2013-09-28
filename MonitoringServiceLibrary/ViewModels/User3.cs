using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class User3
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        List<Item3> Items { get; set; } 

        public User3(User user,List<Item3> items)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Items = items;
        }
    }
}
