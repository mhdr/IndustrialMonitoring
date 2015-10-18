using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using BACnetLib;
using MathNet.Numerics.Statistics;
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
        private object padlock = new object();
        private NetworkVariableBufferedSubscriber<dynamic> _subscriberInt;
        private NetworkVariableBufferedSubscriber<Boolean> _subscriberBool;
        private ItemDefinationType _itemDefinationType;
        private Device _deviceBACnet;
        private BACnetDevice _baCnetDevice;

        private ItemsLog _lastItemLog;
        private ItemsLogLatest _lastItemLogLatest;
        private Item _item;

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

        public Device DeviceBaCnet
        {
            get { return _deviceBACnet; }
            set { _deviceBACnet = value; }
        }

        public Item ItemObj
        {
            get { return _item; }
            set { _item = value; }
        }

        public BACnetDevice BACnetDevice
        {
            get { return _baCnetDevice; }
            set { _baCnetDevice = value; }
        }

        public ItemCollector(Item item)
        {
            this.ItemObj = item;

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
            this.SaveInItemsLogWhen = (WhenToLog)item.SaveInItemsLogWhen;
            this.SaveInItemsLogLastWhen = (WhenToLog)item.SaveInItemsLogLastWhen;
            this.DefinationType = (ItemDefinationType)item.DefinationType;

            this.BSProcessDataServiceClient = new ProcessDataServiceClient();
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
                else if (this.DefinationType == ItemDefinationType.BACnet)
                {

                }

                Timer = new Timer(CheckValue, new object(), 0, ScanCycle);
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
            }
        }

        public async Task ReadValueInfinite()
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
                    else if (this.DefinationType == ItemDefinationType.BACnet)
                    {
                        if (BACnetDevice == null)
                        {
                            this.BACnetDevice = BACnetDevice.Instance;
                        }
                        
                        string ip = ItemObj.BACnetIP;
                        int port = ItemObj.BACnetPort.Value;
                        IPEndPoint endPoint=new IPEndPoint(IPAddress.Parse(ip),port);
                        uint instance = (uint) ItemObj.BACnetControllerInstance.Value;

                        DeviceBaCnet = new Device("Device",0,0,endPoint,0,instance);
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
                    else if (this.DefinationType == ItemDefinationType.BACnet)
                    {
                        if (this.Type == ItemType.Digital)
                        {
                            BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                            uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;
                            value = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                        }
                        else if (Type==ItemType.Analog)
                        {
                            BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                            uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;

                            string preValue = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                            double preValueDouble = double.Parse(preValue);
                            value =Math.Round(preValueDouble,2).ToString();
                        }
                        
                    }

                    lock (padlock)
                    {
                        if (value == null)
                        {
                            continue;
                        }

                        if (value == "-1000")
                        {
                            continue;
                        }

                        double valueDouble = double.Parse(value);

                        bool condition = !string.IsNullOrEmpty(ItemObj.MinRange) && !string.IsNullOrEmpty(ItemObj.MaxRange);
                        if (condition)
                        {
                            double minRange=double.Parse(ItemObj.MinRange);
                            double maxRange= double.Parse(ItemObj.MaxRange);

                            bool shouldNormalize = false;

                            if (ItemObj.NormalizeWhenOutOfRange != null)
                            {
                                shouldNormalize = ItemObj.NormalizeWhenOutOfRange.Value;
                            }

                            if (valueDouble < minRange)
                            {
                                if (shouldNormalize)
                                {
                                    value = ItemObj.MinRange;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (valueDouble > maxRange)
                            {
                                if (shouldNormalize)
                                {
                                    value = ItemObj.MaxRange;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }

                        // detect oulier

                        if (this.Type == ItemType.Analog)
                        {
                            var lastThreeData =
                                                Entities.ItemsLogs.Where(x => x.ItemId == ItemId).OrderByDescending(x => x.ItemLogId).Take(10).ToList();

                            List<double> lastThreeDataInDouble = new List<double>();

                            foreach (ItemsLog itemsLog in lastThreeData)
                            {
                                double currentValue = double.Parse(itemsLog.Value);

                                lastThreeDataInDouble.Add(currentValue);
                            }

                            var iqr = Statistics.InterquartileRange(lastThreeDataInDouble);
                            var lqr = Statistics.LowerQuartile(lastThreeDataInDouble);
                            var uqr = Statistics.UpperQuartile(lastThreeDataInDouble);

                            bool isOutlier = false;

                            if (valueDouble > 3 * iqr + uqr)
                            {
                                isOutlier = true;
                            }

                            if (valueDouble < lqr - 3 * iqr)
                            {
                                isOutlier = true;
                            }

                            var itemLogLatest = Entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == ItemId);

                            if (isOutlier)
                            {
                                if (itemLogLatest != null)
                                {
                                    if (itemLogLatest.PassOutlier != null)
                                    {
                                        if (itemLogLatest.PassOutlier.Value)
                                        {
                                            itemLogLatest.PassOutlier = false;
                                            Entities.SaveChanges();
                                            return;
                                        }
                                        else
                                        {
                                            itemLogLatest.PassOutlier = true;
                                            Entities.SaveChanges();
                                        }
                                    }
                                }
                            }

                            if (itemLogLatest != null)
                            {
                                itemLogLatest.PassOutlier = true;
                                Entities.SaveChanges();
                            }
                        }

                        //

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

        public async Task ReadValue()
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
                else if (this.DefinationType == ItemDefinationType.BACnet)
                {
                    if (BACnetDevice == null)
                    {
                        this.BACnetDevice = BACnetDevice.Instance;
                    }

                    string ip = ItemObj.BACnetIP;
                    int port = ItemObj.BACnetPort.Value;
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    uint instance = (uint)ItemObj.BACnetControllerInstance.Value;

                    DeviceBaCnet = new Device("Device", 0, 0, endPoint, 0, instance);
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
                else if (this.DefinationType == ItemDefinationType.BACnet)
                {
                    if (this.Type == ItemType.Digital)
                    {
                        BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                        uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;
                        value = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                    }
                    else if (Type == ItemType.Analog)
                    {
                        BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                        uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;

                        string preValue = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                        double preValueDouble = double.Parse(preValue);
                        value = Math.Round(preValueDouble, 2).ToString();
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

                    double valueDouble = double.Parse(value);

                    bool condition = !string.IsNullOrEmpty(ItemObj.MinRange) && !string.IsNullOrEmpty(ItemObj.MaxRange);
                    if (condition)
                    {
                        double minRange = double.Parse(ItemObj.MinRange);
                        double maxRange = double.Parse(ItemObj.MaxRange);

                        bool shouldNormalize = false;

                        if (ItemObj.NormalizeWhenOutOfRange != null)
                        {
                            shouldNormalize = ItemObj.NormalizeWhenOutOfRange.Value;
                        }

                        if (valueDouble < minRange)
                        {
                            if (shouldNormalize)
                            {
                                value = ItemObj.MinRange;
                            }
                            else
                            {
                                return ;
                            }
                        }

                        if (valueDouble > maxRange)
                        {
                            if (shouldNormalize)
                            {
                                value = ItemObj.MaxRange;
                            }
                            else
                            {
                                return ;
                            }
                        }
                    }

                    // detect oulier

                    if (this.Type == ItemType.Analog)
                    {
                        var lastThreeData =
                                            Entities.ItemsLogs.Where(x => x.ItemId == ItemId).OrderByDescending(x => x.ItemLogId).Take(10).ToList();

                        List<double> lastThreeDataInDouble = new List<double>();

                        foreach (ItemsLog itemsLog in lastThreeData)
                        {
                            double currentValue = double.Parse(itemsLog.Value);

                            lastThreeDataInDouble.Add(currentValue);
                        }

                        var iqr = Statistics.InterquartileRange(lastThreeDataInDouble);
                        var lqr = Statistics.LowerQuartile(lastThreeDataInDouble);
                        var uqr = Statistics.UpperQuartile(lastThreeDataInDouble);

                        bool isOutlier = false;

                        if (valueDouble > 3 * iqr + uqr)
                        {
                            isOutlier = true;
                        }

                        if (valueDouble < lqr - 3 * iqr)
                        {
                            isOutlier = true;
                        }

                        var itemLogLatest = Entities.ItemsLogLatests.FirstOrDefault(x => x.ItemId == ItemId);

                        if (isOutlier)
                        {
                            if (itemLogLatest != null)
                            {
                                if (itemLogLatest.PassOutlier != null)
                                {
                                    if (itemLogLatest.PassOutlier.Value)
                                    {
                                        itemLogLatest.PassOutlier = false;
                                        Entities.SaveChanges();
                                        return;
                                    }
                                    else
                                    {
                                        itemLogLatest.PassOutlier = true;
                                        Entities.SaveChanges();
                                    }
                                }
                            }
                        }

                        if (itemLogLatest != null)
                        {
                            itemLogLatest.PassOutlier = true;
                            Entities.SaveChanges();
                        }
                    }

                    //

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

        [Obsolete]
        public string ReadValue(bool state)
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
                else if (this.DefinationType == ItemDefinationType.BACnet)
                {
                    if (BACnetDevice == null)
                    {
                        this.BACnetDevice = BACnetDevice.Instance;
                    }

                    string ip = ItemObj.BACnetIP;
                    int port = ItemObj.BACnetPort.Value;
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    uint instance = (uint)ItemObj.BACnetControllerInstance.Value;

                    DeviceBaCnet = new Device("Device", 0, 0, endPoint, 0, instance);
                }


                string value = "-1000000";

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
                else if (this.DefinationType == ItemDefinationType.BACnet)
                {
                    if (this.Type == ItemType.Digital)
                    {
                        BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                        uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;
                        value = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                    }
                    else if (Type == ItemType.Analog)
                    {
                        BACnetEnums.BACNET_OBJECT_TYPE bacnetType = (BACnetEnums.BACNET_OBJECT_TYPE)ItemObj.BACnetItemType.Value;
                        uint itemInstance = (uint)ItemObj.BACnetItemInstance.Value;

                        string preValue = BACnetDevice.ReadValue(DeviceBaCnet, bacnetType, itemInstance).ToString();
                        double preValueDouble = double.Parse(preValue);
                        value = Math.Round(preValueDouble, 2).ToString();
                    }

                }

                lock (padlock)
                {
                    if (value == null)
                    {
                        value= "-1000000";
                    }

                    bool condition = !string.IsNullOrEmpty(ItemObj.MinRange) && !string.IsNullOrEmpty(ItemObj.MaxRange);
                    if (condition)
                    {
                        double valueDouble = double.Parse(value);
                        double minRange = double.Parse(ItemObj.MinRange);
                        double maxRange = double.Parse(ItemObj.MaxRange);

                        bool shouldNormalize = false;

                        if (ItemObj.NormalizeWhenOutOfRange != null)
                        {
                            shouldNormalize = ItemObj.NormalizeWhenOutOfRange.Value;
                        }

                        if (valueDouble < minRange)
                        {
                            if (shouldNormalize)
                            {
                                value = ItemObj.MinRange;
                            }
                        }

                        if (valueDouble > maxRange)
                        {
                            if (shouldNormalize)
                            {
                                value = ItemObj.MaxRange;
                            }
                        }
                    }

                    return value;
                }
            }
            catch (Exception ex)
            {
                Logger.LogMonitoringServiceLibrary(ex);
                return "";
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