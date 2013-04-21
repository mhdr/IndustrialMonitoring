using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Monitoring
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataCollectorService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataCollectorService.svc or DataCollectorService.svc.cs at the Solution Explorer and start debugging.
    public class DataCollectorService : IDataCollectorService
    {
        public bool StartDataCollectorServer()
        {
            try
            {
                DataCollector dataCollector = DataCollector.Collector;
                dataCollector.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("StartDataCollectorServer exception occured");
                return false;
            }

            return true;
        }

        public bool StopDataCollectorServer()
        {
            try
            {
                DataCollector dataCollector = DataCollector.Collector;
                dataCollector.Stop();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("StopDataCollectorServer exception occured");
                return false;
            }

            return true;
        }
    }
}
