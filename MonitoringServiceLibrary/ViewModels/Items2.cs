using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Items2
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public ItemType ItemType { get; set; }

        [DataMember]
        public string ItemTypeString { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public int SaveInItemsLogTimeInterval { get; set; }

        [DataMember]
        public int SaveInItemsLogLastesTimeInterval { get; set; }

        [DataMember]
        public int ShowInUITimeInterval { get; set; }

        [DataMember]
        public int ScanCycle { get; set; }

        [DataMember]
        public WhenToLog SaveInItemsLogWhen { get; set; }

        [DataMember]
        public string SaveInItemsLogWhenString { get; set; }

        [DataMember]
        public WhenToLog SaveInItemsLogLastWhen { get; set; }

        [DataMember]
        public string SaveInItemsLogLastWhenString { get; set; }

        public Items2(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = item.ItemName;
            this.ItemType = (ItemType) item.ItemType;
            if (this.ItemType == ItemType.Analog)
            {
                ItemTypeString = "Analog";
            }
            else if (this.ItemType == ItemType.Digital)
            {
                ItemTypeString = "Digital";
            }

            this.Location = item.Location;
            this.SaveInItemsLogTimeInterval = item.SaveInItemsLogTimeInterval;
            this.SaveInItemsLogLastesTimeInterval = item.SaveInItemsLogLastesTimeInterval;
            this.ShowInUITimeInterval = item.ShowInUITimeInterval;
            this.ScanCycle = item.ScanCycle;
            this.SaveInItemsLogWhen = (WhenToLog) item.SaveInItemsLogWhen;

            if (this.SaveInItemsLogWhen == WhenToLog.OnTimerElapsed)
            {
                this.SaveInItemsLogWhenString = "On Timer Elapsed";
            }
            else if (this.SaveInItemsLogWhen == WhenToLog.OnChange)
            {
                this.SaveInItemsLogWhenString = "On Change";
            }

            this.SaveInItemsLogLastWhen = (WhenToLog) item.SaveInItemsLogLastWhen;

            if (this.SaveInItemsLogLastWhen == WhenToLog.OnTimerElapsed)
            {
                this.SaveInItemsLogLastWhenString = "On Timer Elapsed";
            }
            else if (this.SaveInItemsLogLastWhen == WhenToLog.OnChange)
            {
                this.SaveInItemsLogLastWhenString = "On Change";
            }
        }
    }
}
