using System.Collections.Generic;

namespace TechnicalFanCoilAndroid
{
    public interface ITechnicalFanCoil
    {
        Dictionary<int, int> GetStatus2();

        bool SetStatus(Dictionary<int, int> dic);
    }
}