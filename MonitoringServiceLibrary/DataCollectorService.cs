using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    public class DataCollectorService : IDataCollectorService
    {
        public bool StartDataCollectorServer()
        {
            //DataCollector dataCollector = DataCollector.Collector;
            //dataCollector.Start();

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

        public ServerStatus GetServerStatus()
        {
            DataCollector dataCollector = DataCollector.Collector;
            return dataCollector.ServerStatus;
        }
    }
}
