using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using SharedLibrary;

namespace MonitoringServiceLibrary
{
    public sealed class DataCollector
    {
        private static DataCollector _collector=null;
        private static object padLock=new object();
        private List<Item> _items=new List<Item>();
        private List<ItemCollector> _itemCollectors;
        private ServerStatus _serverStatus;

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

        public List<ItemCollector> ItemCollectors
        {
            get
            {
                if (_itemCollectors == null)
                {
                    _itemCollectors=new List<ItemCollector>();
                }
                return _itemCollectors;
            }
            set { _itemCollectors = value; }
        }

        public ServerStatus ServerStatus
        {
            get { return _serverStatus; }
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
            IndustrialMonitoringEntities Entities=new IndustrialMonitoringEntities();
            Items = Entities.Items.ToList();

            foreach (var item in Items)
            {
                ItemCollector itemCollector=new ItemCollector(item);
                itemCollector.Start();

                ItemCollectors.Add(itemCollector);
                Thread.Sleep(1);
            }

            TelegramBot telegramBot=TelegramBot.Initialize();
            telegramBot.StartResponseServer();

            this._serverStatus = ServerStatus.Run;
        }

        public void Stop()
        {
            foreach (var item in ItemCollectors)
            {
                item.Stop();
                Thread.Sleep(1);
            }

            this._serverStatus=ServerStatus.Stop;
        }
    }
}