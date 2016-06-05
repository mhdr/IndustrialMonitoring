using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;
using Environment = System.Environment;

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
        private Button buttonSave;

        private int buttonRefreshRunningThreads = 0;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            buttonRefresh = FindViewById<Button>(Resource.Id.buttonRefresh);
            buttonRefresh.Click += ButtonRefresh_Click;

            toggleButtonMotor1 = FindViewById<ToggleButton>(Resource.Id.toggleButtonMotor1);
            toggleButtonMotor1.Click += ToggleButtonMotor1_Click;

            toggleButtonMotor2 = FindViewById<ToggleButton>(Resource.Id.toggleButtonMotor2);
            toggleButtonMotor2.Click += ToggleButtonMotor2_Click;

            radioButtonMotor1Speed1 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed1);
            radioButtonMotor1Speed2 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed2);
            radioButtonMotor1Speed3 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor1Speed3);

            radioButtonMotor2Speed1 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed1);
            radioButtonMotor2Speed2 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed2);
            radioButtonMotor2Speed3 = FindViewById<RadioButton>(Resource.Id.radioButtonMotor2Speed3);

            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            buttonSave.Click += ButtonSave_Click;

            Statics.DatabaseFilePath= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"fancoil.db3");
            
            SQLiteConnection connection;
            Settings setting;
            

            try
            {
                connection = new SQLiteConnection(Statics.DatabaseFilePath);
                setting = connection.Get<Settings>(x => x.DatabaseVersion == 1);

                if (setting == null)
                {
                    CreateDatabase();
                }
                else
                {
                    if (setting.DatabaseVersion != 1)
                    {
                        CreateDatabase();
                    }
                }

                connection.Close();
            }
            catch (NullReferenceException ex)
            {
                CreateDatabase();
            }
            
        }

        private void CreateDatabase()
        {
            SQLiteConnection connection = new SQLiteConnection(Statics.DatabaseFilePath);
            connection.CreateTable<Settings>();
            connection.CreateTable<User>();

            Settings settings=new Settings();
            settings.DatabaseVersion = 1;

            connection.Insert(settings);
            connection.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToggleButtonMotor2_Click(object sender, EventArgs e)
        {
            if (toggleButtonMotor2.Checked)
            {
                radioButtonMotor2Speed1.Enabled = true;
                radioButtonMotor2Speed2.Enabled = true;
                radioButtonMotor2Speed3.Enabled = true;
            }
            else
            {
                radioButtonMotor2Speed1.Enabled = false;
                radioButtonMotor2Speed2.Enabled = false;
                radioButtonMotor2Speed3.Enabled = false;

            }
        }

        private void ToggleButtonMotor1_Click(object sender, EventArgs e)
        {
            if (toggleButtonMotor1.Checked)
            {
                radioButtonMotor1Speed1.Enabled = true;
                radioButtonMotor1Speed2.Enabled = true;
                radioButtonMotor1Speed3.Enabled = true;
            }
            else
            {
                radioButtonMotor1Speed1.Enabled = false;
                radioButtonMotor1Speed2.Enabled = false;
                radioButtonMotor1Speed3.Enabled = false;
            }
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = false;
            Thread thread=new Thread(()=> {ButtonRefreshClickedAsync();});
            thread.Start();
        }

        private void ButtonRefreshClickedAsync()
        {
            TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();
            Dictionary<int, int> status = null;

            try
            {
                status = technicalFanCoil.GetStatus2();
            }
            catch (SocketException ex)
            {
                RunOnUiThread(() =>
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Network Error");
                    alert.SetMessage(ex.Message);
                    alert.SetPositiveButton("Ok", delegate (object o, DialogClickEventArgs args)
                    {
                        return;
                    });
                    alert.Create().Show();

                    buttonRefresh.Enabled = true;
                });
            }

            RunOnUiThread(() =>
            {
                if (status == null)
                {
                    return;
                }


                Thread thread = new Thread((() =>
                {
                    buttonRefreshRunningThreads++;

                    Thread.Sleep(1000 * 60);

                    if (buttonRefreshRunningThreads > 1)
                    {
                        buttonRefreshRunningThreads--;
                        return;
                    }

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

                        buttonSave.Enabled = false;

                        buttonRefreshRunningThreads--;
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
                else if (motor1Status == 1)
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

                toggleButtonMotor1.Enabled = true;
                toggleButtonMotor2.Enabled = true;

                if (toggleButtonMotor1.Checked)
                {
                    radioButtonMotor1Speed1.Enabled = true;
                    radioButtonMotor1Speed2.Enabled = true;
                    radioButtonMotor1Speed3.Enabled = true;
                }

                if (toggleButtonMotor2.Checked)
                {
                    radioButtonMotor2Speed1.Enabled = true;
                    radioButtonMotor2Speed2.Enabled = true;
                    radioButtonMotor2Speed3.Enabled = true;
                }

                int isAuthorized = 0;

                SQLiteConnection connection = new SQLiteConnection(Statics.DatabaseFilePath);
                var users = connection.Table<User>();

                foreach (User user in users)
                {
                    if (user.IsAuthorized)
                    {
                        isAuthorized++;
                    }
                }

                if (isAuthorized > 0)
                {
                    buttonSave.Enabled = true;
                }

                buttonRefresh.Enabled = true;
            });
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu,menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            int isAuthorized = 0;

            SQLiteConnection connection = new SQLiteConnection(Statics.DatabaseFilePath);
            var users = connection.Table<User>();

            foreach (User user in users)
            {
                if (user.IsAuthorized)
                {
                    isAuthorized++;
                }
            }

            if (isAuthorized > 0)
            {
                menu.FindItem(Resource.Id.LoginMenuItem).SetVisible(false);
                menu.FindItem(Resource.Id.LogoutMenuItem).SetVisible(true);
            }
            else
            {
                menu.FindItem(Resource.Id.LoginMenuItem).SetVisible(true);
                menu.FindItem(Resource.Id.LogoutMenuItem).SetVisible(false);
            }

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.LoginMenuItem:

                    break;

                case Resource.Id.LogoutMenuItem:

                    break;
            }

            return true;
        }
    }
}

