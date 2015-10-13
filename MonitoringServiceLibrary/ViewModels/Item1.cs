using System.Runtime.Serialization;
using SharedLibrary;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class Item1
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public ItemType ItemType { get; set; }

        [DataMember]
        public int ShowInUITimeInterval { get; set; }

        [DataMember]
        public string Unit { get; set; }

        public Item1()
        {
            
        }

        public Item1(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = item.ItemName;
            this.ItemType = (ItemType) item.ItemType;
            this.ShowInUITimeInterval = item.ShowInUITimeInterval;

            if (string.IsNullOrEmpty(item.Unit))
            {
                this.Unit = "";
            }
            else
            {
                this.Unit = item.Unit;
            }
        }
    }
}