using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;

namespace MonitoringServiceLibrary
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        bool HasNotification(int itemId);

        [OperationContract]
        List<Notification1> GetNotifications(int itemId);

        [OperationContract]
        bool SystemHasNotification(int userId);
    }
}
