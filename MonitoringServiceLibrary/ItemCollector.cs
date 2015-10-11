using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using MonitoringServiceLibrary.BSProcessDataServiceReference;
using SharedLibrary;
using NationalInstruments.NetworkVariable;

namespace MonitoringServiceLibrary
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
        private NetworkVariableBufferedSubscriber<dynamic> _subscriberInt;
        private NetworkVariableBufferedSubscriber<Boolean> _subscriberBool;
        private ItemDefinationType _itemDefinationType;

        private ItemsLog _lastItemLog;
        private ItemsLogLatest _lastItemLogLatest;

        private ProcessDataServiceClient _bSProcessDataServiceClient;

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

        public NetworkVariableBufferedSubscriber<dynamic> SubscriberInt
        {
            get { return _subscriberInt; }
            set { _subscriberInt = value; }
        }

        public NetworkVariableBufferedSubscriber<bool> SubscriberBool
        {
            get { return _subscriberBool; }
            set { _subscriberBool = value; }
        }

        public ItemDefinationType DefinationType
        {
            get { return _itemDefinationType; }
            set { _itemDefinationType = value; }
        }

        public ProcessDataServiceClient BSProcessDataServiceClient
        {
            get { return _bSProcessDataServiceClient; }
            set { _bSProcessDataServiceClient = value; }
        }

        public ItemCollector(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = ItemName;
            this.Type = (ItemType)item.ItemType;

            if (item.Location != null)
            {
                this.Location = item.Location;    
            }
            else
            {
                this.Location = "";
            }
            
            this.SaveInItemsLogTimeInterval = item.SaveInItemsLogTimeInterval;
            this.SaveInItemsLogLastesTimeInterval = item.SaveInItemsLogLastesTimeInterval;
            this.ScanCycle = item.ScanCycle;
            this.SaveInItemsLogWhen = (WhenToLog) item.SaveInItemsLogWhen;
            this.SaveInItemsLogLastWhen = (WhenToLog) item.SaveInItemsLogLastWhen;
            this.DefinationType = (ItemDefinationType) item.DefinationType;

            this.BSProcessDataServiceClient=new ProcessDataServiceClient();
        }

        [Obsolete]
        public void Start()
        {
            try
            {
                if (this.DefinationType == ItemDefinationType.SqlDefined)
                {
                    if (this.Type == ItemType.Digital)
                    {
                        SubscriberBool = new NetworkVariableBufferedSubscriber<bool>(this.Location);
                        SubscriberBool.Connect();
                    }
                    else if (this.Type == ItemType.Analog)
                    {
                        SubscriberInt = new NetworkVariableBufferedSubscriber<dynamic>(this.Location);
                        SubscriberInt.Connect();
                    }
                }
                else if (this.DefinationType == ItemDefinationType.CustomDefiend)
                {

                }

                Timer = new Timer(CheckValue, new object(), 0,ScanCycle);
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            } 
        }

        public async Task ReadValue()
        {
            while (true)
            {
                try
                {
                    if (this.DefinationType == ItemDefinationType.SqlDefined)
                    {
                        if (this.Type == ItemType.Digital)
                        {
                            SubscriberBool = new NetworkVariableBufferedSubscriber<bool>(this.Location);
                            SubscriberBool.Connect();
                        }
                        else if (this.Type == ItemType.Analog)
                        {
                            SubscriberInt = new NetworkVariableBufferedSubscriber<dynamic>(this.Location);
                            SubscriberInt.Connect();
                        }
                    }
                    else if (this.DefinationType == ItemDefinationType.CustomDefiend)
                    {

                    }


                    string value = "-1000";

                    if (this.DefinationType == ItemDefinationType.SqlDefined)
                    {
                        if (this.Type == ItemType.Digital)
                        {
                            var data = SubscriberBool.ReadData();
                            value = Convert.ToInt32(data.GetValue()).ToString();
                        }
                        else if (this.Type == ItemType.Analog)
                        {
                            var data = SubscriberInt.ReadData();
                            value = Math.Round(data.GetValue(), 2).ToString();
                        }
                    }
                    else if (this.DefinationType == ItemDefinationType.CustomDefiend)
                    {
                        switch (this.ItemId)
                        {
                            case 10:
                                value = (BSProcessDataServiceClient.GetPreHeatingZoneTemperature() / 10).ToString();
                                break;
                            case 13:
                                value = BSProcessDataServiceClient.GetSterilizerZoneTemperature().ToString();
                                break;
                            case 14:
                                value = (BSProcessDataServiceClient.GetCoolingZoneTemperature() / 10).ToString();
                                break;
                        }
                    }

                    lock (padlock)
                    {
                        if (value == null)
                        {
                            return;
                        }

                        if (value == "-1000")
                        {
                            return;
                        }

                        if (LastItemLog == null)
                        {
                            SaveValueInItemsLog(value);
                        }
                        else if (SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
                        {
                            TimeSpan timeSpan = DateTime.Now - LastItemLog.Time;

                            if (timeSpan.TotalSeconds >= SaveInItemsLogTimeInterval)
                            {
                                SaveValueInItemsLog(value);
                            }
                        }
                        else if (SaveInItemsLogWhen == WhenToLog.OnChange)
                        {
                            if (LastItemLog.Value != value)
                            {
                                SaveValueInItemsLog(value);
                            }
                        }

                        if (LastItemLogLatest == null)
                        {
                            SaveValueInItemsLogLatest(value);
                        }
                        else if (SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
                        {
                            TimeSpan timeSpan = DateTime.Now - LastItemLogLatest.Time;

                            if (timeSpan.TotalSeconds >= SaveInItemsLogLastesTimeInterval)
                            {
                                SaveValueInItemsLogLatest(value);
                            }
                        }
                        else if (SaveInItemsLogLastWhen == WhenToLog.OnChange)
                        {
                            if (LastItemLogLatest.Value != value)
                            {
                                SaveValueInItemsLogLatest(value);
                            }
                        }
                    }

                    await Task.Delay(ScanCycle);
                }
                catch (Exception ex)
                {
                    Logger.LogMonitoringServiceLibrary(ex);
                }
            }
        }

        public void Stop()
        {
            Timer.Dispose();
        }

        [Obsolete]
        private void CheckValue(object state)
        {
            try
            {
                string value = "-1000";

                if (this.DefinationType == ItemDefinationType.SqlDefined)
                {
                    if (this.Type == ItemType.Digital)
                    {
                        var data = SubscriberBool.ReadData();
                        value = Convert.ToInt32(data.GetValue()).ToString();
                    }
                    else if (this.Type == ItemType.Analog)
                    {
                        var data = SubscriberInt.ReadData();
                        value = Math.Round(data.GetValue(),2).ToString();
                    }
                }
                else if (this.DefinationType == ItemDefinationType.CustomDefiend)
                {
                    switch (this.ItemId)
                    {
                        case 10:
                            value = (BSProcessDataServiceClient.GetPreHeatingZoneTemperature()/10).ToString();
                            break;
                        case 13:
                            value = BSProcessDataServiceClient.GetSterilizerZoneTemperature().ToString();
                            break;
                        case 14:
                            value = (BSProcessDataServiceClient.GetCoolingZoneTemperature()/10).ToString();
                            break;
                    }

                    //Console.WriteLine("{0} : {1}",this.ItemId,value);
                }

                lock (padlock)
                {
                    if (value == null)
                    {
                        return;
                    }

                    if (value == "-1000")
                    {
                        return;
                    }

                    if (LastItemLog == null)
                    {
                        SaveValueInItemsLog(value);
                    }
                    else if (SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
                    {
                        TimeSpan timeSpan = DateTime.Now - LastItemLog.Time;

                        if (timeSpan.TotalSeconds >= SaveInItemsLogTimeInterval)
                        {
                            SaveValueInItemsLog(value);
                        }
                    }
                    else if (SaveInItemsLogWhen == WhenToLog.OnChange)
                    {
                        if (LastItemLog.Value != value)
                        {
                            SaveValueInItemsLog(value);
                        }
                    }

                    if (LastItemLogLatest == null)
                    {
                        SaveValueInItemsLogLatest(value);
                    }
                    else if (SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
                    {
                        TimeSpan timeSpan = DateTime.Now - LastItemLogLatest.Time;

                        if (timeSpan.TotalSeconds >= SaveInItemsLogLastesTimeInterval)
                        {
                            SaveValueInItemsLogLatest(value);
                        }
                    }
                    else if (SaveInItemsLogLastWhen == WhenToLog.OnChange)
                    {
                        if (LastItemLogLatest.Value != value)
                        {
                            SaveValueInItemsLogLatest(value);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }
        }

        private void SaveValueInItemsLogLatest(string value)
        {
            try
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
                    itemsLogLatest = new ItemsLogLatest();
                    itemsLogLatest.ItemId = this.ItemId;
                    itemsLogLatest.Time = DateTime.Now;
                    itemsLogLatest.Value = value;
                    Entities.ItemsLogLatests.Add(itemsLogLatest);
                }

                Entities.SaveChanges();
                
                this.LastItemLogLatest = itemsLogLatest;
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }
        }


        private void SaveValueInItemsLog(string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
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
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }

        }

    }
}