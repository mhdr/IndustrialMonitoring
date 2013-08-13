using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServiceLibrary
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        string GetData(int value);
    }
}
