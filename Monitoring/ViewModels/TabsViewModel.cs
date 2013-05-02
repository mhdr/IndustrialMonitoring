using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring.ViewModels
{
    public class TabsViewModel
    {
        public int TabId { get; set; }
        public string TabName { get; set; }

        public TabsViewModel()
        {
            
        }

        public TabsViewModel(Tab tab)
        {
            this.TabId = tab.TabId;
            this.TabName = tab.TabName;
        }
    }
}