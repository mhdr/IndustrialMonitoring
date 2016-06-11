using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TechnicalFanCoilAndroid
{
    public static class Statics
    {
        public static string DatabaseFilePath;

        public static string GetConnectionString()
        {
            return string.Format("Data Source={0};Version=3;", DatabaseFilePath);
        }
    }
}