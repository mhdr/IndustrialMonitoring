using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndustrialMonitoring.ProcessDataServiceReference;

namespace IndustrialMonitoring.Lib
{
    public class ShowDataCompletedEventArgs : EventArgs
    {
        public List<ItemsLogChartHistoryViewModel> Data { get; set; }
        public KeyValuePair<int,int> ItemId;
        public ItemsAIOViewModel CurrentItem;

        public ShowDataCompletedEventArgs(KeyValuePair<int,int> itemId, List<ItemsLogChartHistoryViewModel> data,ItemsAIOViewModel currentItem)
        {
            this.ItemId = itemId;
            this.Data = data;
            this.CurrentItem = currentItem;
        }
    }
}
