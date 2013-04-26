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
using IndustrialMonitoring.ProcessDataServiceReference;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessDataServiceClient _processDataServiceClient;

        public MainWindow()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get
            {
                if (_processDataServiceClient == null)
                {
                    _processDataServiceClient=new ProcessDataServiceClient();
                }

                return _processDataServiceClient;
            }
            set { _processDataServiceClient = value; }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsBusy = true;

            var items = ProcessDataServiceClient.GetItemsAll();

            foreach (var itemsAioViewModel in items)
            {
                
            }

            BusyIndicator.IsBusy = false;
        }
    }
}
