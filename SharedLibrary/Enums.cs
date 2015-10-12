using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public enum ItemType
    {
        Unknown = 0,
        Digital = 1,
        Analog = 2
    }

    public enum WhenToLog
    {
        Unknown = 0,
        OnTimerElapsed = 1,
        OnChange = 2
    }

    public enum ServerStatus
    {
        Run = 1,
        Stop = 2,
    }

    public enum NotificationType
    {
        Lower = 1,
        Between = 2,
        Higher = 3,
    }

    public enum ItemDefinationType
    {
        SqlDefined = 1,
        CustomDefiend = 2,
        BACnet=3,
    }

    public enum QueueDirection
    {
        In = 1,
        Out = 2
    }
}
