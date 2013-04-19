using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monitoring
{
    public sealed class DataCollector
    {
        private static DataCollector _collector=null;
        private static object padLock=new object();

        public static DataCollector Collector
        {
            get {
                lock (padLock)
                {
                    if (_collector == null)
                    {
                        _collector=new DataCollector();
                    }

                    return _collector;
                }
            }
        }

        public DataCollector()
        {
            
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}