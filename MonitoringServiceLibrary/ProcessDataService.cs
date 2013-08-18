using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;

namespace MonitoringServiceLibrary
{
    public class ProcessDataService : IProcessDataService
    {
        private IndustrialMonitoringEntities _entities = new IndustrialMonitoringEntities();

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

            result = new ItemsLogLatestAIOViewModel(current);

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

        public ItemsAIOViewModel GetItem(int itemId)
        {
            var item = Entities.Items.FirstOrDefault(x => x.ItemId == itemId);
            ItemsAIOViewModel result = new ItemsAIOViewModel(item);

            return result;
        }

        public List<TabsViewModel> GetTabs()
        {
            List<TabsViewModel> result = new List<TabsViewModel>();

            var tabs = Entities.Tabs;

            foreach (var tab in tabs)
            {
                TabsViewModel current = new TabsViewModel(tab);

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
            List<TabsItemsViewModel> result = new List<TabsItemsViewModel>();

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

        public List<ItemsAIOViewModel> GetItems(int tabId)
        {
            List<ItemsAIOViewModel> result = new List<ItemsAIOViewModel>();

            var tabsItem = Entities.TabsItems.Where(x => x.TabId == tabId);

            foreach (var item in tabsItem)
            {
                Item currentItem = Entities.Items.FirstOrDefault(x => x.ItemId == item.ItemId);

                result.Add(new ItemsAIOViewModel(currentItem));
            }

            return result;
        }

        public List<ItemsLogChartHistoryViewModel> GetItemLogs(int itemId, DateTime startDate, DateTime endDate)
        {
            List<ItemsLogChartHistoryViewModel> result = new List<ItemsLogChartHistoryViewModel>();

            var itemsLog = Entities.ItemsLogs.Where(x => x.ItemId == itemId);

            foreach (var log in itemsLog)
            {
                if (log.Time >= startDate && log.Time <= endDate)
                {
                    result.Add(new ItemsLogChartHistoryViewModel(log));
                }

            }

            return result;
        }
    }
}
