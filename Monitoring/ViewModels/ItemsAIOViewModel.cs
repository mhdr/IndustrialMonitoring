using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedLibrary;

namespace Monitoring.ViewModels
{
    public class ItemsAIOViewModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        public int ShowInUITimeInterval { get; set; }

        //TODO edame
    }
}