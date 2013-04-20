using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Monitoring
{
    public sealed class DataCollector
    {
        private static DataCollector _collector=null;
        private static object padLock=new object();
        private List<Item> _items;
        private IndustrialMonitoringEntities _entities=new IndustrialMonitoringEntities();

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

        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public IndustrialMonitoringEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public void Start()
        {
            this.DoStart();
        }

        public void StartAsync()
        {
            
        }

        private void DoStart()
        {
            Items = Entities.Items.ToList();

            foreach (var item in Items)
            {
                ItemCollector itemCollector=new ItemCollector(item);
                itemCollector.Start();
                Thread.Sleep(1);
            }
        }

        public void Stop()
        {
            
        }
    }
}