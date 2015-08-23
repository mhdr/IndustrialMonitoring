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

        public List<User2> GetUsers2()
        {
            IndustrialMonitoringEntities entities=new IndustrialMonitoringEntities();
            var usersQuery = entities.Users;

            List<User2> result=new List<User2>();

            foreach (User user in usersQuery)
            {
                User2 users2=new User2(user);
                result.Add(users2);
            }

            return result;
        }

        public int SetPassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();
                var user = entities.Users.FirstOrDefault(x => x.UserId == userId);

                if (user == null)
                {
                    return -1;
                }

                if (user.Password != oldPassword)
                {
                    return -2;
                }

                user.Password = newPassword;

                entities.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                // TODO log
                return 0;
            }
        }

        public List<int> GetUserServicesPermission(int userId)
        {
            try
            {
                IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();
                var permissions = entities.UsersServicesPermissions.Where(x => x.UserId == userId).ToList();

                var ids=new List<int>();

                foreach (UsersServicesPermission u in permissions)
                {
                    ids.Add(u.ServiceId);
                }

                return ids;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
