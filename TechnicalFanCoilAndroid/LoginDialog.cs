
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using TechnicalFanCoilAndroid.Model;

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
            string userName = editTextUserName.Text;
            string password = editTextPassword.Text;

            //UserService userService=new UserService();
            //bool result = userService.Authorize(userName, password);


            bool result = false;

            if (userName == "mahmood" && password == "12345")
            {
                result = true;
            }

            if (result)
            {
                SQLiteConnection  connection=new SQLiteConnection(Statics.DatabaseFilePath);

                Login login = new Login();
                login.UserName = userName;
                login.IsAuthorized = true;

                connection.Insert(login, typeof(Login));

                Activity.FragmentManager.PopBackStack();
            }
            else
            {
                textViewLoginErrorMessage.Visibility=ViewStates.Visible;
                textViewLoginErrorMessage.Text = "Wrong username or password";
            }
        }
    }
}

