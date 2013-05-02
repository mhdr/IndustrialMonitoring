using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Monitoring.ViewModels;

namespace Monitoring
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProcessDataService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProcessDataService.svc or ProcessDataService.svc.cs at the Solution Explorer and start debugging.
    public class ProcessDataService : IProcessDataService
    {
        private IndustrialMonitoringEntities _entities=new IndustrialMonitoringEntities();

        public IndustrialMonitoringEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public ItemsLogLatestAIOViewModel GeItemsLogLatest(int itemId)
        {
            ItemsLogLatestAIOViewModel result = null;

            ItemsLogLatest current = Entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == itemId);

            if (current == null)
            {
                return null;
            }

            result=new ItemsLogLatestAIOViewModel(current);

            return result;
        }

        public List<ItemsAIOViewModel> GetItems()
        {
            List<ItemsAIOViewModel> result = new List<ItemsAIOViewModel>();

            var items = Entities.Items;

            foreach (var item in items)
            {
                result.Add(new ItemsAIOViewModel(item));
            }

            return result;
        }

        public List<ItemsAIOViewModel> GetItems(Func<ItemsAIOViewModel, bool> predicate)
        {
            List<ItemsAIOViewModel> result = new List<ItemsAIOViewModel>();

            var items = Entities.Items;

            foreach (var item in items)
            {
                ItemsAIOViewModel current=new ItemsAIOViewModel(item);

                if (predicate(current))
                {
                    result.Add(current);    
                }
                
            }

            return result;
        }

        public List<TabsViewModel> GetTabs()
        {
            List<TabsViewModel> result=new List<TabsViewModel>();

            var tabs = Entities.Tabs;

            foreach (var tab in tabs)
            {
                TabsViewModel current=new TabsViewModel(tab);

                result.Add(current);
            }

            return result;
        }

        public List<TabsViewModel> GetTabs(Func<TabsViewModel, bool> predicate)
        {
            List<TabsViewModel> result = new List<TabsViewModel>();

            var tabs = Entities.Tabs;

            foreach (var tab in tabs)
            {
                TabsViewModel current = new TabsViewModel(tab);

                if (predicate(current))
                {
                    result.Add(current);   
                }
            }

            return result;
        }

        public List<TabsItemsViewModel> GetTabItems()
        {
            List<TabsItemsViewModel> result=new List<TabsItemsViewModel>();

            var tabItems = Entities.TabsItems;

            foreach (var tabsItem in tabItems)
            {
                TabsItemsViewModel current = new TabsItemsViewModel(tabsItem);
                result.Add(current);
            }

            return result;
        }

        public List<TabsItemsViewModel> GetTabItems(Func<TabsItemsViewModel, bool> predicate)
        {
            List<TabsItemsViewModel> result = new List<TabsItemsViewModel>();

            var tabItems = Entities.TabsItems;

            foreach (var tabsItem in tabItems)
            {
                TabsItemsViewModel current = new TabsItemsViewModel(tabsItem);
                
                if (predicate(current))
                {
                    result.Add(current);   
                }
            }

            return result;
        }
    }
}
