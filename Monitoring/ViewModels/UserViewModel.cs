using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Monitoring.ViewModels
{
    [DataContract]
    public class UserViewModel
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        public UserViewModel()
        {
            
        }

        public UserViewModel(User user)
        {
            this.UserId = user.UserId;
            this.UserName = user.UserName;
            this.Password = user.Password;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
        }
    }
}