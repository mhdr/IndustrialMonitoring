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
        List<Item1> GetItems();

        [OperationContract]
        List<Item2> GetItems2();

        [OperationContract]
        List<Item3> GetItems3();

        [OperationContract]
        Item1 GetItem(int itemId);

        [OperationContract(Name = "GetTabsAll")]
        List<Tab1> GetTabs();

        [OperationContract(Name = "GetTab")]
        List<Tab1> GetTabs(Func<Tab1, bool> predicate);

        [OperationContract(Name = "GetTabItemsAll")]
        List<TabsItemsViewModel> GetTabItems();

        [OperationContract(Name = "GetTabItems")]
        List<TabsItemsViewModel> GetTabItems(Func<TabsItemsViewModel, bool> predicate);

        [OperationContract(Name = "GetItemsForTab")]
        List<Item1> GetItems(int tabId);

        [OperationContract]
        List<ItemsLogChartHistoryViewModel> GetItemLogs(int itemId, DateTime startDate, DateTime endDate);

        [OperationContract]
        bool DeleteItemLog(int itemLogId);

        [OperationContract]
        bool AddItem2(Item2 item);

        [OperationContract]
        bool DeleteItem(int itemId);

        [OperationContract]
        bool EditItem2(Item2 item);

        [OperationContract]
        bool AddItemsToTab(string tabName, List<string> items);

        [OperationContract]
        bool GetHorn();

        [OperationContract]
        bool GetHornHMI();

        [OperationContract]
        bool GetMuteHorn();

        [OperationContract]
        void MuteHorn();

        [OperationContract]
        void UnMuteHorn();

        [OperationContract]
        bool On(string location);

        [OperationContract]
        bool Off(string location);
    }
}
