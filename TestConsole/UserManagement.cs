using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    public class UserManagement
    {
        public void ChangePassword(int userId, string password)
        {
            SHA256 sha256 = SHA256Managed.Create();

            byte[] passBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = sha256.ComputeHash(passBytes);
            string hashed = Encoding.UTF8.GetString(hashedBytes);

            IndustrialMonitoringEntities entities=new IndustrialMonitoringEntities();
            var user = entities.Users.FirstOrDefault(x => x.UserId == userId);
            user.Password = hashed;
            entities.SaveChanges();
        }
    }
}
