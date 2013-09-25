using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;

namespace MonitoringServiceLibrary
{
    [ServiceContract]
    public interface IProcessDataService
    {
        [OperationContract]
        ItemsLogLatestAIOViewModel GeItemsLogLatest(int itemId);

        [OperationContract]
        List<Items1> GetItems();

        [OperationContract]
        List<Items2> GetItems2();

        [OperationContract]
        List<Items3> GetItems3();

        [OperationContract]
        Items1 GetItem(int itemId);

        [OperationContract(Name = "GetTabsAll")]
        List<Tab1> GetTabs();

        [OperationContract(Name = "GetTab")]
        List<Tab1> GetTabs(Func<Tab1, bool> predicate);

        [OperationContract(Name = "GetTabItemsAll")]
        List<TabsItemsViewModel> GetTabItems();

        [OperationContract(Name = "GetTabItems")]
        List<TabsItemsViewModel> GetTabItems(Func<TabsItemsViewModel, bool> predicate);

        [OperationContract(Name = "GetItemsForTab")]
        List<Items1> GetItems(int tabId);

        [OperationContract]
        List<ItemsLogChartHistoryViewModel> GetItemLogs(int itemId, DateTime startDate, DateTime endDate);

        [OperationContract]
        bool AddItem2(Items2 item);

        [OperationContract]
        bool DeleteItem(int itemId);

        [OperationContract]
        bool EditItem2(Items2 item);

        [OperationContract]
        bool AddItemsToTab(string tabName, List<string> items);

        [OperationContract]
        List<Tab2> GetTabItems2();
    }
}
