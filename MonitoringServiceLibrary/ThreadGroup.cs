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
    
    public partial class ThreadGroup
    {
        public ThreadGroup()
        {
            this.Items = new HashSet<Item>();
        }
    
        public int ThreadGroupId { get; set; }
        public string ThreadGroupName { get; set; }
        public int IntervalBetweenItems { get; set; }
        public int IntervalBetweenCycle { get; set; }
    
        public virtual ICollection<Item> Items { get; set; }
    }
}