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
using Telerik.Windows;
using Telerik.Windows.Controls;

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
            StyleManager.ApplicationTheme=new Windows8Theme();
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
            AddToStackPanel("Data Collector is Started successfully",Brushes.Green);
            MenuItemStop.IsEnabled = true;
            MenuItemStart.IsEnabled = false;
        }

        private void StartUIFailed()
        {
            AddToStackPanel("Data Collector is failed to Start", Brushes.Red);
            MenuItemStart.IsEnabled = true;
            MenuItemStop.IsEnabled = false;
        }

        public void StartDataColletor()
        {
            MenuItemStart.IsEnabled = false;
            MenuItemStop.IsEnabled = false;

            AddToStackPanel("Data Collector is Starting ...", Brushes.Black);
            StartAsync();
        }

        public void StopDataColletor()
        {
            MenuItemStart.IsEnabled = false;
            MenuItemStop.IsEnabled = false;
            AddToStackPanel("Data Collector is Stopping ...", Brushes.Black);
            StopAsync();
        }

        private void StopAsync()
        {
            Thread thread = new Thread(() => StopDoWork());
            thread.Start();
        }

        private void StopDoWork()
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
            AddToStackPanel("Data Collector is Stopped successfully", Brushes.Green);
            MenuItemStop.IsEnabled = false;
            MenuItemStart.IsEnabled = true;
        }

        private void StopUIFailed()
        {
            AddToStackPanel("Data Collector is failed to Stop", Brushes.Red);
            MenuItemStart.IsEnabled = false;
            MenuItemStop.IsEnabled = true;
        }


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            MenuItemStop.IsEnabled = false;
            MenuItemStart.IsEnabled = true;
        }

        public void AddToStackPanel(string msg,Brush brush)
        {
            Dispatcher.BeginInvoke(new Action(() => AddToStackPanelUI(msg, brush)));
        }

        private void AddToStackPanelUI(string msg, Brush brush)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = string.Format("{0} - {1}",DateTime.Now,msg);
            textBlock.Foreground = brush;
            textBlock.FontSize = 14;
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.Margin = new Thickness(5, 2, 5, 2);
            StackPanelMain.Children.Add(textBlock);
        }

        private void MenuItemStart_OnClick(object sender, RadRoutedEventArgs e)
        {
            StartDataColletor();
        }

        private void MenuItemStop_OnClick(object sender, RadRoutedEventArgs e)
        {
            StopDataColletor();
        }

    }
}
