using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.ViewModels
{
    public class TabsItemsViewModel
    {
        public int TabItemId { get; set; }
        public int TabId { get; set; }
        public int ItemId { get; set; }

        public TabsItemsViewModel()
        {
            
        }

        public TabsItemsViewModel(TabsItem tabsItem)
        {
            this.TabItemId = tabsItem.TabItemId;
            this.TabId = tabsItem.TabId;
            this.ItemId = tabsItem.TabItemId;
        }
    }
}