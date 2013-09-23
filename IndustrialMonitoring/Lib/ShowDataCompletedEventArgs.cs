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
        public KeyValuePair<int,int> ItemId { get; set; }
        public Items1 CurrentItem { get; set; }
        public bool GenerateLegend { get; set; }

        public ShowDataCompletedEventArgs(KeyValuePair<int,int> itemId, List<ItemsLogChartHistoryViewModel> data,Items1 currentItem,bool generateLegend)
        {
            this.ItemId = itemId;
            this.Data = data;
            this.CurrentItem = currentItem;
            this.GenerateLegend = generateLegend;
        }
    }
}
