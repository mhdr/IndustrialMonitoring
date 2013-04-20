using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public enum ItemType
    {
        Unknown =0,
        Digital=1,
        Analog=2
    }

    public enum WhenToLog
    {
        Unknown=0,
        OnTimerElapsed=1,
        OnChange=2
    }
}
