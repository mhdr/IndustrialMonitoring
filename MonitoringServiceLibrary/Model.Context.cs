﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringServiceLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IndustrialMonitoringEntities : DbContext
    {
        public IndustrialMonitoringEntities()
            : base("name=IndustrialMonitoringEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemsLog> ItemsLogs { get; set; }
        public DbSet<ItemsLogLatest> ItemsLogLatests { get; set; }
        public DbSet<Tab> Tabs { get; set; }
        public DbSet<TabsItem> TabsItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersItemsPermission> UsersItemsPermissions { get; set; }
        public DbSet<NotificationOccur> NotificationOccurs { get; set; }
        public DbSet<NotificationOccurUser> NotificationOccurUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationsReceiver> NotificationsReceivers { get; set; }
    }
}
