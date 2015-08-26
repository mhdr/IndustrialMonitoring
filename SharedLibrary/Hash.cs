using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class Hash
    {
        public static string GetHash(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();

            byte[] passBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashedBytes = sha256.ComputeHash(passBytes);
            string hashed = Encoding.UTF8.GetString(hashedBytes);

            return hashed;
        }
    }
}
