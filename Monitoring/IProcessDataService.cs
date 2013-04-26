using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Monitoring.ViewModels;

namespace Monitoring
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProcessDataService" in both code and config file together.
    [ServiceContract]
    public interface IProcessDataService
    {
        [OperationContract]
        ItemsLogLatestAIOViewModel GeItemsLogLatest(int itemId);

        [OperationContract(Name = "GetItemsAll")]
        List<ItemsAIOViewModel> GetItems();

        [OperationContract(Name = "GetItems")]
        List<ItemsAIOViewModel> GetItems(Func<ItemsAIOViewModel, bool> predicate);
    }
}
