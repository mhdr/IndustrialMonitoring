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

namespace TechnicalFanCoilAndroid.Model
{
    public class Setting
    {
        public int Id { get; set; }


        public string Key;
        public string Value;
    }
}