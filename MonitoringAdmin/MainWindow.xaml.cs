using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
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
using MonitoringAdmin.DataCollectorServiceReference;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataCollectorServiceClient _proxy;

        public MainWindow()
        {
            InitializeComponent();
        }

        public DataCollectorServiceClient Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new DataCollectorServiceClient();
                }

                return _proxy;
            }
            set { _proxy = value; }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = false;

            AddToStackPanel(string.Format("{0} - Data Collector is Starting ...",DateTime.Now.ToString()), Brushes.Black);
            StartAsync();
        }

        public void StartAsync()
        {
            Thread thread=new Thread(()=>Start());
            thread.Start();
        }

        private void Start()
        {
            if (Proxy.StartDataCollectorServer())
            {
                Dispatcher.BeginInvoke(new Action(StartUI));
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(StartUIFailed));
            }
        }

        private void StartUI()
        {
            Led1.Value = true;
            AddToStackPanel(string.Format("{0} - Data Collector is Started successfully",DateTime.Now.ToString()), Brushes.Green);
            ButtonStop.IsEnabled = true;
            ButtonStart.IsEnabled = false;
        }

        private void StartUIFailed()
        {
            AddToStackPanel(string.Format("{0} - Data Collector is failed to Start",DateTime.Now.ToString()), Brushes.Red);
            ButtonStart.IsEnabled = true;
            ButtonStop.IsEnabled = false;
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = false;

            AddToStackPanel(string.Format("{0} - Data Collector is Stopping ...", DateTime.Now.ToString()), Brushes.Black);
            StopAsync();
        }

        public void StopAsync()
        {
            Thread thread = new Thread(() => Stop());
            thread.Start();
        }

        private void Stop()
        {
            if (Proxy.StopDataCollectorServer())
            {
                Dispatcher.BeginInvoke(new Action(StopUI));
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(StopUIFailed));
            }
        }

        private void StopUI()
        {
            Led1.Value = false;
            AddToStackPanel(string.Format("{0} - Data Collector is Stopped successfully", DateTime.Now.ToString()), Brushes.Green);
            ButtonStop.IsEnabled = false;
            ButtonStart.IsEnabled = true;
        }

        private void StopUIFailed()
        {
            AddToStackPanel(string.Format("{0} - Data Collector is failed to Stop", DateTime.Now.ToString()), Brushes.Red);
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = true;
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ButtonStop.IsEnabled = false;
            ButtonStart.IsEnabled = true;
        }

        public void AddToStackPanel(string msg,Brush brush)
        {
            TextBlock textBlock=new TextBlock();
            textBlock.Text = msg;
            textBlock.Foreground = brush;
            textBlock.FontSize = 14;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin=new Thickness(5,2,5,2);
            StackPanelMain.Children.Add(textBlock);
        }
    }
}
