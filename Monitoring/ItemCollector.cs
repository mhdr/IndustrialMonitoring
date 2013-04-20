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
            switch (Type)
            {
                    case ItemType.Analog:
                    Timer = new Timer(CheckAnalogValue, new object(), 0, ScanCycle);   
                    break;
                    case ItemType.Digital:
                    Timer=new Timer(CheckDigitalValue,new object(), 0,ScanCycle);
                    break;
            }
            
        }

        public void Stop()
        {
            Timer.Dispose();
        }

        private void CheckAnalogValue(object state)
        {
            NetworkVariableBufferedSubscriber<Int32> subscriber = new NetworkVariableBufferedSubscriber<int>(this.Location);
            subscriber.Connect();

            var data = subscriber.ReadData();
            int value = data.GetValue();

            if (LastItemLog == null)
            {
                System.Diagnostics.Debug.WriteLine(1);
                SaveAnalogValueInItemsLog(value.ToString());
            }
            else if (SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
            {
                System.Diagnostics.Debug.WriteLine(2);
                TimeSpan timeSpan = DateTime.Now - LastItemLog.Time;

                if (timeSpan.Seconds > SaveInItemsLogTimeInterval*1000)
                {
                    SaveAnalogValueInItemsLog(value.ToString());    
                }
            }
            else if (SaveInItemsLogWhen == WhenToLog.OnChange)
            {
                System.Diagnostics.Debug.WriteLine(3);
                if (LastItemLog.Value != value.ToString())
                {
                    SaveAnalogValueInItemsLog(value.ToString());    
                }
            }

            if (LastItemLogLatest == null)
            {
                System.Diagnostics.Debug.WriteLine(4);
                SaveAnalogValueInItemsLogLatest(value.ToString());    
            }
            else if (SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
            {
                System.Diagnostics.Debug.WriteLine(5);
                TimeSpan timeSpan = DateTime.Now - LastItemLog.Time;

                if (timeSpan.Seconds > SaveInItemsLogTimeInterval * 1000)
                {
                    SaveAnalogValueInItemsLogLatest(value.ToString()); 
                }
            }
            else if (SaveInItemsLogLastWhen == WhenToLog.OnChange)
            {
                System.Diagnostics.Debug.WriteLine(6);
                if (LastItemLog.Value != value.ToString())
                {
                    SaveAnalogValueInItemsLogLatest(value.ToString()); 
                }
            }

            subscriber.Disconnect();
            subscriber.Dispose();
        }

        private void SaveAnalogValueInItemsLogLatest(string value)
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

        private void SaveAnalogValueInItemsLog(string value)
        {
            ItemsLog itemsLog = new ItemsLog();
            itemsLog.ItemId = this.ItemId;
            itemsLog.Time = DateTime.Now;
            itemsLog.Value = value;
            Entities.ItemsLogs.Add(itemsLog);

            Entities.SaveChanges();

            this.LastItemLog = itemsLog;
        }

        private void CheckDigitalValue(object state)
        {
            return;
            NetworkVariableBufferedSubscriber<bool> subscriber = new NetworkVariableBufferedSubscriber<bool>(this.Location);
            subscriber.Connect();

            var data = subscriber.ReadData();
            bool value = data.GetValue();

            if (SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
            {

            }
            else if (SaveInItemsLogWhen == WhenToLog.OnChange)
            {

            }

            if (SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
            {

            }
            else if (SaveInItemsLogLastWhen == WhenToLog.OnChange)
            {

            }

            ItemsLogLatest itemsLogLatest = new ItemsLogLatest();

            if (Convert.ToBoolean(LastItemLogLatest.Value) != value)
            {
                
                itemsLogLatest.ItemId = this.ItemId;
                itemsLogLatest.Time = DateTime.Now;
                itemsLogLatest.Value = Convert.ToInt32(value).ToString();
                Entities.ItemsLogLatests.Add(itemsLogLatest);
                Entities.SaveChanges();

                subscriber.Disconnect();
                subscriber.Dispose();
            }

            LastItemLogLatest = itemsLogLatest;
        }
    }
}