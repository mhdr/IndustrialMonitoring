using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            UserManagement userManagement=new UserManagement();
            userManagement.ChangePassword(4,"test");

            Console.ReadKey();
        }
    }
}
