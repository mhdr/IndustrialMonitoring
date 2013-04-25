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

            result=new ItemsLogLatestAIOViewModel(current);

            return result;
        }
    }
}
