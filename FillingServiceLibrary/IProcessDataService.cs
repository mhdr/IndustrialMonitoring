using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FillingServiceLibrary
{
    [ServiceContract]
    public interface IProcessDataService
    {
        [OperationContract]
        double GetPreHeatingZoneTemperature();

        [OperationContract]
        double GetSterilizerZoneTemperature();

        [OperationContract]
        double GetCoolingZoneTemperature();
    }
}
