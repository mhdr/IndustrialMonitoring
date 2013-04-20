using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;

namespace Monitoring
{
    public class Global : System.Web.HttpApplication
    {
        private Timer _timer;

        public Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Timer=new Timer(Start,new object(), 60 * 1000,Timeout.Infinite);
        }

        private void Start(object state)
        {
            DataCollector dataCollector=new DataCollector();
            dataCollector.Start();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}