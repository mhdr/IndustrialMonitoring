using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonitoringServiceLibrary.ViewModels;

namespace MonitoringServiceLibrary
{
    public class UserService:IUserService
    {
        public UserViewModel GetUserByUserName(string userName)
        {
            UserViewModel result = null;
            var Entities = new IndustrialMonitoringEntities();
            User user = Entities.Users.FirstOrDefault(x => x.UserName == userName);

            result = new UserViewModel(user);

            return result;
        }

        public UserViewModel GetUserByUserId(int userId)
        {
            UserViewModel result = null;
            var Entities = new IndustrialMonitoringEntities();
            User user = Entities.Users.FirstOrDefault(x => x.UserId == userId);

            result = new UserViewModel(user);

            return result;
        }

        public bool Authorize(string userName, string password)
        {
            var Entities = new IndustrialMonitoringEntities();
            if (Entities.Users.Any(x => x.UserName == userName && x.Password == password))
            {
                return true;
            }

            return false;
        }

        public bool CheckPermission(int userId, int itemId)
        {
            var Entities = new IndustrialMonitoringEntities();
            if (Entities.UsersItemsPermissions.Any(x => x.UserId == userId && x.ItemId == itemId))
            {
                return true;
            }

            return false;
        }

        public bool UserHaveItemInTab(int userId, int tabId)
        {
            var Entities = new IndustrialMonitoringEntities();
            var userItemsPermissionQuery = Entities.UsersItemsPermissions.Where(x => x.UserId == userId);

            foreach (var u in userItemsPermissionQuery)
            {
                bool tabExist = Entities.TabsItems.Any(x => x.ItemId == u.ItemId && x.TabId == tabId);

                if (tabExist)
                {
                    return true;
                }
            }

            return false;
        }

        public List<Users2> GetUsers2()
        {
            IndustrialMonitoringEntities entities=new IndustrialMonitoringEntities();
            var usersQuery = entities.Users;

            List<Users2> result=new List<Users2>();

            foreach (User user in usersQuery)
            {
                Users2 users2=new Users2(user);
                result.Add(users2);
            }

            return result;
        }
    }
}
