using System.Runtime.Serialization;
using SharedLibrary;

namespace MonitoringServiceLibrary.ViewModels
{
    [DataContract]
    public class ItemsAIOViewModel
    {
        [DataMember]
        public int ItemId { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public ItemType ItemType { get; set; }

        [DataMember]
        public int ShowInUITimeInterval { get; set; }

        public ItemsAIOViewModel()
        {
            
        }

        public ItemsAIOViewModel(Item item)
        {
            this.ItemId = item.ItemId;
            this.ItemName = item.ItemName;
            this.ItemType = (ItemType) item.ItemType;
            this.ShowInUITimeInterval = item.ShowInUITimeInterval;
        }
    }
}