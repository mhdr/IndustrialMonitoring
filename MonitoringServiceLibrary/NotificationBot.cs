//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringServiceLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class NotificationBot
    {
        public int NotificationBotId { get; set; }
        public int NotificationLogId { get; set; }
        public System.DateTime SentTime { get; set; }
        public bool IsDelayed { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual NotificationItemsLog NotificationItemsLog { get; set; }
    }
}