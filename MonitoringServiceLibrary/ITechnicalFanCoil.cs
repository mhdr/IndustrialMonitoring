using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SharedLibrarySocket.Warpper;

namespace MonitoringServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITechnicalFanCoil" in both code and config file together.
    [ServiceContract]
    public interface ITechnicalFanCoil
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        bool TurnOffMotor1();

        [OperationContract]
        bool TurnOffMotor2();

        [OperationContract]
        bool ChangeSpeedMotor1(int speed);

        [OperationContract]
        bool ChangeSpeedMotor2(int speed);

        [OperationContract]
        string GetStatus();

        [OperationContract]
        Dictionary<int, int> GetStatus2();

        [OperationContract]
        bool SetStatus(SetStatusWrapper value);
    }
}
