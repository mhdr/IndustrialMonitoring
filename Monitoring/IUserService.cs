using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Monitoring.ViewModels;

namespace Monitoring
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserService" in both code and config file together.
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        UserViewModel GetUserByUserName(string userName);

        [OperationContract]
        UserViewModel GetUserByUserId(int userId);

        [OperationContract]
        bool Authorize(string userName, string password);

        [OperationContract]
        bool CheckPermission(int userId, int itemId);
    }
}
