//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringHostService
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemsLog
    {
        public int ItemLogId { get; set; }
        public int ItemId { get; set; }
        public string Value { get; set; }
        public System.DateTime Time { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual Item Item { get; set; }
    }
}