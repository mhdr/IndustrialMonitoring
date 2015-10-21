using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            IndustrialMonitoringEntities Entities = null;
            try
            {
                Entities = new IndustrialMonitoringEntities();
                Items = Entities.Items.ToList();
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

            foreach (var item in Items)
            {
                try
                {
                    if (item.ThreadGroupId == null)
                    {
                        ItemCollector itemCollector = new ItemCollector(item);
                        Thread thread = new Thread(() =>
                        {
                            itemCollector.ReadValueInfinite();
                        });
                        thread.Start();

                        ItemCollectors.Add(itemCollector);
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMonitoringServiceLibrary(ex);
                }
            }

            var groups = Items.GroupBy(x => x.ThreadGroup).Select(x => x.First());

            foreach (Item item in groups)
            {
                try
                {
                    if (item.ThreadGroupId != null & item.ThreadGroupId > 0)
                    {
                        var itemsInGroup = Entities.Items.Where(x => x.ThreadGroupId == item.ThreadGroupId).ToList();

                        Thread thread = new Thread(() =>
                          {
                              ReadValues(itemsInGroup);
                          });

                        thread.Start();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMonitoringServiceLibrary(ex);
                }
            }

            this._serverStatus = ServerStatus.Run;
        }

        public async Task ReadValues(List<Item> items)
        {
            var threadGroup = items.First().ThreadGroup;

            while (true)
            {
                try
                {
                    foreach (Item item in items)
                    {
                        try
                        {
                            var collector = new ItemCollector(item);
                            await collector.ReadValue();

                            await Task.Delay(threadGroup.IntervalBetweenItems);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogMonitoringServiceLibrary(ex);
                        }
                    }

                    await Task.Delay(threadGroup.IntervalBetweenCycle);
                }
                catch (Exception ex)
                {
                    Logger.LogMonitoringServiceLibrary(ex);
                }
            }
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