using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using SharedLibrary;
using NationalInstruments.NetworkVariable;

namespace Monitoring
{
    public class ItemCollector
    {
        private Timer _timer;
        private int _itemId;
        private string _location;
        private string _itemName;
        private ItemType _type;
        private int _saveInItemsLogTimeInterval;
        private int _saveInItemsLogLastesTimeInterval;
        private int _showInUiTimeInterval;
        private int _scanCycle;
        private WhenToLog _saveInItemsLogWhen;
        private WhenToLog _saveInItemsLogLastWhen;
        private IndustrialMonitoringEntities _entities = new IndustrialMonitoringEntities();
        private object padlock=new object();
        private NetworkVariableBufferedSubscriber<Int32> _subscriberInt;
        private NetworkVariableBufferedSubscriber<Boolean> _subscriberBool; 

        private ItemsLog _lastItemLog;
        private ItemsLogLatest _lastItemLogLatest;

        public Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        public ItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public int SaveInItemsLogTimeInterval
        {
            get { return _saveInItemsLogTimeInterval; }
            set { _saveInItemsLogTimeInterval = value; }
        }

        public int SaveInItemsLogLastesTimeInterval
        {
            get { return _saveInItemsLogLastesTimeInterval; }
            set { _saveInItemsLogLastesTimeInterval = value; }
        }

        public int ShowInUiTimeInterval
        {
            get { return _showInUiTimeInterval; }
            set { _showInUiTimeInterval = value; }
        }

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public IndustrialMonitoringEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public int ScanCycle
        {
            get { return _scanCycle; }
            set { _scanCycle = value; }
        }

        public ItemsLog LastItemLog
        {
            get { return _lastItemLog; }
            set { _lastItemLog = value; }
        }

        public ItemsLogLatest LastItemLogLatest
        {
            get { return _lastItemLogLatest; }
            set { _lastItemLogLatest = value; }
        }

        public WhenToLog SaveInItemsLogWhen
        {
            get { return _saveInItemsLogWhen; }
            set { _saveInItemsLogWhen = value; }
        }

        public WhenToLog SaveInItemsLogLastWhen
        {
            get { return _saveInItemsLogLastWhen; }
            set { _saveInItemsLogLastWhen = value; }
        }

        public NetworkVariableBufferedSubscriber<int> SubscriberInt
        {
            get { return _subscriberInt; }
            set { _subscriberInt = value; }
        }

        public NetworkVariableBufferedSubscriber<bool> SubscriberBool
        {
            get { return _subscriberBool; }
            set { _subscriberBool = value; }
        }

        public ItemCollector(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = ItemName;
            this.Type = (ItemType)item.ItemType;
            this.Location = item.Location;
            this.SaveInItemsLogTimeInterval = item.SaveInItemsLogTimeInterval;
            this.SaveInItemsLogLastesTimeInterval = item.SaveInItemsLogLastesTimeInterval;
            this.ScanCycle = item.ScanCycle;
            this.SaveInItemsLogWhen = (WhenToLog) item.SaveInItemsLogWhen;
            this.SaveInItemsLogLastWhen = (WhenToLog) item.SaveInItemsLogLastWhen;
        }

        public void Start()
        {
            if (this.Type == ItemType.Digital)
            {
                SubscriberBool = new NetworkVariableBufferedSubscriber<bool>(this.Location);
                SubscriberBool.Connect();
            }
            else if (this.Type == ItemType.Analog)
            {
                SubscriberInt = new NetworkVariableBufferedSubscriber<int>(this.Location);   
                SubscriberInt.Connect();
            }
            Timer = new Timer(CheckValue, new object(), 0, ScanCycle);   
        }

        public void Stop()
        {
            Timer.Dispose();

            //if (this.Type == ItemType.Digital)
            //{
            //    SubscriberBool.Disconnect();
            //    SubscriberBool.Dispose();
            //}
            //else if (this.Type == ItemType.Analog)
            //{
            //    SubscriberInt.Disconnect();
            //    SubscriberInt.Dispose();
            //}
        }

        private void CheckValue(object state)
        {
            string value = null;

            if (this.Type == ItemType.Digital)
            {
                var data = SubscriberBool.ReadData();
                value = Convert.ToInt32(data.GetValue()).ToString();
            }
            else if (this.Type == ItemType.Analog)
            {
                var data = SubscriberInt.ReadData();
                value = data.GetValue().ToString();
            }

            lock (padlock)
            {


                if (LastItemLog == null)
                {
                    System.Diagnostics.Debug.WriteLine("LastItemLog : null");
                    SaveValueInItemsLog(value);
                }
                else if (SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
                {
                    TimeSpan timeSpan = DateTime.Now - LastItemLog.Time;

                    if (timeSpan.Seconds >= SaveInItemsLogTimeInterval)
                    {
                        System.Diagnostics.Debug.WriteLine("LastItemLog : TimerElapsed");
                        SaveValueInItemsLog(value);
                    }
                }
                else if (SaveInItemsLogWhen == WhenToLog.OnChange)
                {
                    if (LastItemLog.Value != value)
                    {
                        System.Diagnostics.Debug.WriteLine("LastItemLog : Changed");
                        SaveValueInItemsLog(value);
                    }
                }

                if (LastItemLogLatest == null)
                {
                    System.Diagnostics.Debug.WriteLine("LastItemLogLatest : null");
                    SaveValueInItemsLogLatest(value);
                }
                else if (SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
                {
                    TimeSpan timeSpan = DateTime.Now - LastItemLogLatest.Time;

                    if (timeSpan.Seconds >= SaveInItemsLogLastesTimeInterval)
                    {
                        System.Diagnostics.Debug.WriteLine("LastItemLogLatest : TimerElapsed");
                        SaveValueInItemsLogLatest(value);
                    }
                }
                else if (SaveInItemsLogLastWhen == WhenToLog.OnChange)
                {
                    if (LastItemLogLatest.Value != value)
                    {
                        System.Diagnostics.Debug.WriteLine("LastItemLogLatest : Changed");
                        SaveValueInItemsLogLatest(value);
                    }
                }

            }
        }

        private void SaveValueInItemsLogLatest(string value)
        {
            ItemsLogLatest itemsLogLatest = null;
            if (Entities.ItemsLogLatests.Any(x => x.ItemId == this.ItemId))
            {
                itemsLogLatest = Entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == this.ItemId);
                itemsLogLatest.Time = DateTime.Now;
                itemsLogLatest.Value = value;
            }
            else
            {
                itemsLogLatest=new ItemsLogLatest();
                itemsLogLatest.ItemId = this.ItemId;
                itemsLogLatest.Time = DateTime.Now;
                itemsLogLatest.Value = value;
                Entities.ItemsLogLatests.Add(itemsLogLatest);
            }

            Entities.SaveChanges();

            this.LastItemLogLatest = itemsLogLatest;
        }


        private void SaveValueInItemsLog(string value)
        {
            ItemsLog itemsLog = new ItemsLog();
            itemsLog.ItemId = this.ItemId;
            itemsLog.Time = DateTime.Now;
            itemsLog.Value = value;
            Entities.ItemsLogs.Add(itemsLog);

            Entities.SaveChanges();

            this.LastItemLog = itemsLog;
        }

    }
}