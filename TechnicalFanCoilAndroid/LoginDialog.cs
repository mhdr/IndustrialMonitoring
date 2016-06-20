
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using TechnicalFanCoilAndroid.Model;
using TechnicalFanCoilAndroid.RPC;

namespace TechnicalFanCoilAndroid
{
	public class LoginDialog : DialogFragment
	{
	    private EditText editTextUserName;
	    private EditText editTextPassword;
	    private Button buttonLogin;
	    private Button buttonCancelLogin;
	    private TextView textViewLoginErrorMessage;

        public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetStyle(DialogFragmentStyle.NoTitle, 0);
		    // Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);


            Activity.ActionBar.Title="Login";

		    Android.Views.View view= inflater.Inflate(Resource.Layout.Login, container, false);

		    editTextUserName= view.FindViewById<EditText>(Resource.Id.editTextUserName);
		    editTextPassword = view.FindViewById<EditText>(Resource.Id.editTextPassword);
		    buttonLogin = view.FindViewById<Button>(Resource.Id.buttonLogin);
            buttonLogin.Click += ButtonLogin_Click;

		    buttonCancelLogin = view.FindViewById<Button>(Resource.Id.buttonCancelLogin);
            buttonCancelLogin.Click += ButtonCancelLogin_Click;

		    textViewLoginErrorMessage = (TextView) view.FindViewById(Resource.Id.textViewLoginErrorMessage);

            return view;
		}

        private void ButtonCancelLogin_Click(object sender, EventArgs e)
        {
            Activity.FragmentManager.PopBackStack();
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            buttonLogin.Enabled = false;
            buttonLogin.Text = "Loging in...";

            string userName = editTextUserName.Text;
            string password = editTextPassword.Text;

            Thread thread=new Thread(() =>
            {
                UserService userService = new UserService();
                string result = userService.AuthorizeAndGetSession(userName, password);

                if (result.Length > 0)
                {

                    var logins = Login.GetValues(x => x.IsAuthorized);

                    if (logins != null)
                    {
                        foreach (Login l in logins)
                        {
                            l.IsAuthorized = false;
                            Login.Update(l);
                        }
                    }

                    Login login = new Login();
                    login.UserName = userName;
                    login.IsAuthorized = true;
                    login.SessionKey = result;

                    Login.Insert(login);

                    Activity.FragmentManager.PopBackStack();
                }
                else
                {
                    Activity?.RunOnUiThread(() =>
                    {
                        textViewLoginErrorMessage.Visibility = ViewStates.Visible;
                        textViewLoginErrorMessage.Text = "Wrong username or password";
                    });
                }

                Activity?.RunOnUiThread(() =>
                {
                    buttonLogin.Enabled = true;
                    buttonLogin.Text = "Login";
                });
            });

            thread.Start();
            
        }
    }
}

