using System.Collections.Generic;

namespace TechnicalFanCoilAndroid.RPC
{
    public interface ITechnicalFanCoil
    {
        Dictionary<int, int> GetStatus2();

        bool SetStatus(Dictionary<int, int> dic);
    }
}