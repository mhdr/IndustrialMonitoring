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

        [OperationContract]
        bool UserHaveItemInTab(int userId, int tabId);

        [OperationContract]
        List<User2> GetUsers2();

        [OperationContract]
        int SetPassword(int userId, string oldPassword, string newPassword);

        [OperationContract]
        List<int> GetUserServicesPermission(int userId);
    }
}
