using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndustrialMonitoring.UserServiceReference;

namespace IndustrialMonitoring.Lib
{
    public class Static
    {
        public static UserViewModel CurrentUser { get; set; }
        public static List<int> UserServicesPermission { get; set; }
    }
}
