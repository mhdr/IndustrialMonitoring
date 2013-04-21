using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonitoringAdmin.MonitoringServiceReference;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Service1Client _proxy;

        public MainWindow()
        {
            InitializeComponent();
        }

        public Service1Client Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy=new Service1Client();
                }

                return _proxy;
            }
            set { _proxy = value; }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (Proxy.StartDataCollectorServer())
            {
                led1.Value = true;
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            if (Proxy.StopDataCollectorServer())
            {
                led1.Value = false;
            }
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
