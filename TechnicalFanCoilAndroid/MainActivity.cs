using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SharedLibrarySocket;
using SharedLibrarySocket.Warpper;
using TechnicalFanCoilAndroid.Lib;
using TechnicalFanCoilAndroid.Model;
using TechnicalFanCoilAndroid.RPC;
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


        private ProgressDialog progressDialog;

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

            //DeleteDatabaseFile();
            InitializeDatabase();

            progressDialog=new ProgressDialog(this);

            Thread thread=new Thread(() =>
            {
                CheckUpdate();
            });

            thread.Start();
        }

        private void CheckUpdate()
        {
            try
            {
                string url = string.Format("http://{0}:{1}/TechnicalFanCoilAndroid/update.json",Statics.IPAddress,Statics.HttpPort);

                var client = new WebClient();
                var updateStrInJson = client.DownloadString(url);

                var update = JsonConvert.DeserializeObject<Update>(updateStrInJson);

                HttpWebRequest httpWebRequest =(HttpWebRequest) WebRequest.Create(update.Url);
                httpWebRequest.Method = "HEAD";
                HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                long length = httpWebResponse.ContentLength;

                RunOnUiThread(() =>
                {
                    if (!update.LatestVersion.Equals(Statics.Version, StringComparison.Ordinal))
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetTitle("Update");
                        alert.SetMessage(string.Format("Version {0} is available,Do you want to download it?", update.LatestVersion));
                        alert.SetPositiveButton("Yes", (sender, args) =>
                        {
                            Intent updateIntent = new Intent(this, typeof(UpdateActivity));
                            updateIntent.PutExtra("Version", update.LatestVersion);
                            updateIntent.PutExtra("Url", update.Url);
                            updateIntent.PutExtra("Length", length);
                            StartActivity(updateIntent);
                        });

                        alert.SetNegativeButton("No", (sender, args) =>
                        {
                            return;
                        });

                        alert.Create().Show();
                    }
                });
            }
            catch (Exception ex)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", (sender, args) =>
                {
                   return; 
                });

                alert.Create().Show();
            }

        }

        private void DeleteDatabaseFile()
        {
            if (File.Exists(Statics.DatabaseFilePath))
            {
                File.Delete(Statics.DatabaseFilePath);
            }            
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(Statics.DatabaseFilePath))
            {
                BinaryReader binaryReader=new BinaryReader(Assets.Open("fancoil.db3"));
                BinaryWriter binaryWriter=new BinaryWriter(new FileStream(Statics.DatabaseFilePath,FileMode.Create));

                byte[] buffer=new byte[2048];
                int len = 0;

                while ((len = binaryReader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    binaryWriter.Write(buffer, 0, len);
                }

                binaryReader.Close();
                binaryWriter.Close();
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //buttonSave.Enabled = false;
            //buttonRefresh.Enabled = false;

            progressDialog.SetTitle("Working");
            progressDialog.SetMessage("Wait while working...");
            progressDialog.Show();

            Thread thread=new Thread(()=> { buttonSaveClickedAsync(); });
            thread.Start();
        }

        private void buttonSaveClickedAsync()
        {
            try
            {
                Dictionary<int, int> dic = new Dictionary<int, int>();

                MotorStatus motor1 = MotorStatus.Off;
                MotorStatus motor2 = MotorStatus.Off;

                if (!toggleButtonMotor1.Checked)
                {
                    motor1=MotorStatus.Off;
                }
                else if (radioButtonMotor1Speed1.Checked)
                {
                    motor1=MotorStatus.Speed1;
                }
                else if (radioButtonMotor1Speed2.Checked)
                {
                    motor1=MotorStatus.Speed2;
                }
                else if (radioButtonMotor1Speed3.Checked)
                {
                    motor1=MotorStatus.Speed3;
                }

                if (!toggleButtonMotor2.Checked)
                {
                    motor2=MotorStatus.Off;
                }
                else if (radioButtonMotor2Speed1.Checked)
                {
                    motor2=MotorStatus.Speed1;
                }
                else if (radioButtonMotor2Speed2.Checked)
                {
                    motor2=MotorStatus.Speed2;
                }
                else if (radioButtonMotor2Speed3.Checked)
                {
                    motor2=MotorStatus.Speed3;
                }


                SetStatusWrapper wrapper=new SetStatusWrapper();
                wrapper.Motor1 = motor1;
                wrapper.Motor2 = motor2;

                var login = Login.GetValues(x => x.IsAuthorized).FirstOrDefault();
                wrapper.SessionKey = login.SessionKey;

                TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();
                bool result = technicalFanCoil.SetStatus(wrapper);

                ButtonRefreshClickedAsync();

                //RunOnUiThread(() =>
                //{
                //    buttonSave.Enabled = true;
                //});

                progressDialog.Dismiss();
            }
            catch (Exception ex)
            {
                //RunOnUiThread(() =>
                //{
                //    buttonSave.Enabled = true;
                //    buttonRefresh.Enabled = true;
                //});

                progressDialog.Dismiss();

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", delegate (object o, DialogClickEventArgs args)
                {
                    return;
                });
                alert.Create().Show();
            }
            
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
            //buttonRefresh.Enabled = false;
            progressDialog.SetTitle("Working");
            progressDialog.SetMessage("Wait while working...");
            progressDialog.Show();

            Thread thread=new Thread(()=> {ButtonRefreshClickedAsync();});
            thread.Start();
        }

        private void ButtonRefreshClickedAsync()
        {
            try
            {
                TechnicalFanCoil technicalFanCoil = new TechnicalFanCoil();
                Dictionary<int, int> status = null;

                status = technicalFanCoil.GetStatus2();

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


                    var values = Login.GetValues(x => x.IsAuthorized);
                    int isAuthorized = 0;

                    if (values != null)
                    {
                        isAuthorized = values.Count;
                    }

                    if (isAuthorized > 0)
                    {
                        buttonSave.Enabled = true;
                    }

                    progressDialog.Dismiss();
                });
            }
            catch (SocketException ex)
            {
                RunOnUiThread(() =>
                {
                    progressDialog.Dismiss();

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
            catch (Exception ex)
            {
                progressDialog.Dismiss();

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", delegate (object o, DialogClickEventArgs args)
                {
                    return;
                });
                alert.Create().Show();
            }
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainMenu,menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            CreateMenu(menu);
            return base.OnPrepareOptionsMenu(menu);
        }

        private void CreateMenu(IMenu menu)
        {
            var values = Login.GetValues(x => x.IsAuthorized);
            int isAuthorized = 0;

            if (values != null)
            {
                isAuthorized = values.Count;
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
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            CreateMenu(menu);
            return base.OnMenuOpened(featureId, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.LoginMenuItem:

                    var ft = FragmentManager.BeginTransaction();
                    var prev= FragmentManager.FindFragmentByTag<LoginDialog>("loginDialog");

                    if (prev != null)
                    {
                        ft.Remove(prev);
                    }

                    // when you press back-key, the current activity (which holds multiple fragments) 
                    // will load previous fragment rather than finishing itself.
                    ft.AddToBackStack(null);

                    LoginDialog loginDialog=new LoginDialog();
                    loginDialog.Show(ft, "loginDialog");

                    break;

                case Resource.Id.LogoutMenuItem:

                    var logins = Login.GetValues(x => x.IsAuthorized);

                    if (logins != null)
                    {
                        foreach (Login login in logins)
                        {
                            login.IsAuthorized = false;
                            Login.Update(login);
                        }
                    }

                    buttonSave.Enabled = false;

                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}

