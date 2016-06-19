
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using TechnicalFanCoilAndroid.Lib;
using Environment = System.Environment;
using Uri = Android.Net.Uri;

namespace TechnicalFanCoilAndroid
{
	[Activity(Label = "UpdateActivity")]
	public class UpdateActivity : Activity
	{
	    private ProgressBar progressBarUpdate;
	    private TextView textViewProgressText;
	    private TextView textViewVersionValue;
	    private TextView textViewFileSizeValue;
	    private Button buttonStartStopDownloadUpdate;
	    private Button buttonCancelDownloadUpdate;


	    private WebClient client;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Update);

		    progressBarUpdate = FindViewById<ProgressBar>(Resource.Id.progressBarUpdate);
		    progressBarUpdate.Progress = 0;

		    textViewProgressText = FindViewById<TextView>(Resource.Id.textViewProgressText);
            textViewVersionValue = FindViewById<TextView>(Resource.Id.textViewVersionValue);
            textViewFileSizeValue = FindViewById<TextView>(Resource.Id.textViewFileSizeValue);
            buttonStartStopDownloadUpdate = FindViewById<Button>(Resource.Id.buttonStartStopDownloadUpdate);
            buttonCancelDownloadUpdate = FindViewById<Button>(Resource.Id.buttonCancelDownloadUpdate);

            buttonStartStopDownloadUpdate.Click += ButtonStartStopDownloadUpdate_Click;
            buttonCancelDownloadUpdate.Click += ButtonCancelDownloadUpdate_Click;

            progressBarUpdate.Max = 100;

		    if (Intent.Extras != null)
		    {
                var version = Intent.Extras.GetString("Version");
                var url = Intent.Extras.GetString("Url");
                var length = Intent.Extras.GetLong("Length");

		        double lengthInMB = length/1000000;

		        textViewVersionValue.Text = version;
		        textViewFileSizeValue.Text = string.Format("{0} MB",lengthInMB);
		    }

		}

        private void ButtonCancelDownloadUpdate_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.CancelAsync();
            }
        }

        private void ButtonStartStopDownloadUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                buttonStartStopDownloadUpdate.Enabled = false;
                var url = Intent.Extras.GetString("Url");
                client = new WebClient();
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadDataCompleted += Client_DownloadDataCompleted;
                client.DownloadDataAsync(new System.Uri(url));
            }
            catch (Exception ex)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", (o, args) =>
                {
                    return;
                });
                alert.Create().Show();
            }

        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    var url = Intent.Extras.GetString("Url");
                    var fileName = System.IO.Path.GetFileName(url);
                    string path = "";

                    //if (Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads) != null)
                    //{
                    //    path =
                    //        Android.OS.Environment.GetExternalStoragePublicDirectory(
                    //            Android.OS.Environment.DirectoryDownloads).ToString();
                    //}
                    //else
                    //{
                    //    path = Android.OS.Environment.DataDirectory.ToString();
                    //}

                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    var filePath = Path.Combine(path, fileName);
                    var filePath2 = string.Format("file://{0}", filePath);

                    if (File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    BinaryWriter binaryWriter = new BinaryWriter(new FileStream(filePath, FileMode.Create));
                    binaryWriter.Write(e.Result);
                    binaryWriter.Close();

                    Android.Content.Intent intentInstall = new Intent(Intent.ActionView).SetDataAndType(Uri.Parse(filePath2), "application/vnd.android.package-archive");
                    intentInstall.SetFlags(ActivityFlags.NewTask);
                    StartActivity(intentInstall);
                }

                
            }
            catch (Exception ex)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Error");
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", (o, args) =>
                {
                    return;
                });
                alert.Create().Show();
            }

            buttonStartStopDownloadUpdate.Enabled = true;

        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBarUpdate.Progress = e.ProgressPercentage;
            textViewProgressText.Text = string.Format("{0}/100", e.ProgressPercentage);
        }
    }
}

