using System.Collections.Generic;
using SharedLibrarySocket.Warpper;

namespace SharedLibrarySocket.Interfaces
{
    public interface ITechnicalFanCoil
    {
        Dictionary<int, int> GetStatus2();

        bool SetStatus(SetStatusWrapper value);
    }
}