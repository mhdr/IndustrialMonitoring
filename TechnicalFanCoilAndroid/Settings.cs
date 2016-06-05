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
using SQLite;

namespace TechnicalFanCoilAndroid
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int SettingId { get; set; }


        public int DatabaseVersion;
    }
}