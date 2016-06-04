using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace TechnicalFanCoilAndroid
{
    [Activity(Label = "Fan Coil", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button buttonRefresh;
        private ToggleButton toggleButtonMotor1;
        private ToggleButton toggleButtonMotor2;
        private RadioButton radioButtonMotor1Speed1;
        private RadioButton radioButtonMotor1Speed2;
        private RadioButton radioButtonMotor1Speed3;
        private RadioButton radioButtonMotor2Speed1;
        private RadioButton radioButtonMotor2Speed2;
        private RadioButton radioButtonMotor2Speed3;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            buttonRefresh = FindViewById<Button>(Resource.Id.buttonRefresh);
            buttonRefresh.Click += ButtonRefresh_Click;

            toggleButtonMotor1 = FindViewById<ToggleButton>(Resource.Id.toggleButtonMotor1);
            toggleButtonMotor2 = FindViewById<ToggleButton>(Resource.Id.toggleButtonMotor2);

            radioButtonMotor1Speed1 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed1);
            radioButtonMotor1Speed2 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed2);
            radioButtonMotor1Speed3 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed3);

            radioButtonMotor2Speed1 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed1);
            radioButtonMotor2Speed2 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed2);
            radioButtonMotor2Speed3 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed3);
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();
            Dictionary<int, int> status = null;

            try
            {
                status = technicalFanCoil.GetStatus2();
            }
            catch (SocketException ex)
            {
                AlertDialog.Builder alert=new AlertDialog.Builder(this);
                alert.SetTitle("Network Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", delegate(object o, DialogClickEventArgs args)
                {
                    return;
                });
                alert.Create().Show();
            }

            if (status == null)
            {
                return;
            }

            toggleButtonMotor1.Enabled = true;
            toggleButtonMotor2.Enabled = true;
            radioButtonMotor1Speed1.Enabled = true;
            radioButtonMotor1Speed2.Enabled = true;
            radioButtonMotor1Speed3.Enabled = true;
            radioButtonMotor2Speed1.Enabled = true;
            radioButtonMotor2Speed2.Enabled = true;
            radioButtonMotor2Speed3.Enabled = true;

            Thread thread=new Thread((() =>
            {
                Thread.Sleep(1000*60);

                RunOnUiThread(() =>
                {
                    toggleButtonMotor1.Enabled = false;
                    toggleButtonMotor2.Enabled = false;
                    radioButtonMotor1Speed1.Enabled = false;
                    radioButtonMotor1Speed2.Enabled = false;
                    radioButtonMotor1Speed3.Enabled = false;
                    radioButtonMotor2Speed1.Enabled = false;
                    radioButtonMotor2Speed2.Enabled = false;
                    radioButtonMotor2Speed3.Enabled = false;
                });
            }));
            thread.Start();


            int motor1Status = status[1];
            int motor2Status = status[2];

            if (motor1Status == 0)
            {
                toggleButtonMotor1.Checked = false;
                radioButtonMotor1Speed1.Checked = false;
                radioButtonMotor1Speed2.Checked = false;
                radioButtonMotor1Speed3.Checked = false;
            }
            else if (motor1Status==1)
            {
                toggleButtonMotor1.Checked = true;
                radioButtonMotor1Speed1.Checked = true;
            }
            else if (motor1Status == 2)
            {
                toggleButtonMotor1.Checked = true;
                radioButtonMotor1Speed2.Checked = true;
            }
            else if (motor1Status == 3)
            {
                toggleButtonMotor1.Checked = true;
                radioButtonMotor1Speed3.Checked = true;
            }

            if (motor2Status == 0)
            {
                toggleButtonMotor2.Checked = false;
                radioButtonMotor2Speed1.Checked = false;
                radioButtonMotor2Speed2.Checked = false;
                radioButtonMotor2Speed3.Checked = false;
            }
            else if (motor2Status == 1)
            {
                toggleButtonMotor2.Checked = true;
                radioButtonMotor2Speed1.Checked = true;
            }
            else if (motor2Status == 2)
            {
                toggleButtonMotor2.Checked = true;
                radioButtonMotor2Speed2.Checked = true;

            }
            else if (motor2Status == 3)
            {
                toggleButtonMotor2.Checked = true;
                radioButtonMotor2Speed3.Checked = true;
            }
        }
    }
}

