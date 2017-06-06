using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustrialMonitoring.Lib
{
    public class TimeChangedEventArgs:EventArgs
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeChangedEventArgs()
        {
            
        }

        public TimeChangedEventArgs(DateTime startTime, DateTime endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
        }
    }
}
