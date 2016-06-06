
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TechnicalFanCoilAndroid
{
	public class LoginDialog : DialogFragment
	{
	    private EditText editTextUserName;
	    private EditText editTextPassword;
	    private Button buttonLogin;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

		    Android.Views.View view= inflater.Inflate(Resource.Layout.Login, container, false);

		    editTextUserName= view.FindViewById<EditText>(Resource.Id.editTextUserName);
		    editTextPassword = view.FindViewById<EditText>(Resource.Id.editTextPassword);
		    buttonLogin = view.FindViewById<Button>(Resource.Id.buttonLogin);

		    return view;
		}
	}
}

