using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;
using MonitoringAdmin.ProcessDataServiceReference;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowItemsManagement.xaml
    /// </summary>
    public partial class WindowItemsManagement : Window
    {
        private ProcessDataServiceClient _processDataServiceClient=new ProcessDataServiceClient();
        private List<Items3> ItemsList=new List<Items3>(); 

        public WindowItemsManagement()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get { return _processDataServiceClient; }
            set { _processDataServiceClient = value; }
        }

        private void BindListBoxItems()
        {
            BusyIndicatorItems.IsBusy = true;
            Thread thread=new Thread(()=>BindListBoxItemsAsync());
            thread.Start();
        }

        private void BindListBoxItemsAsync()
        {
            ItemsList = ProcessDataServiceClient.GetItems3();
            Dispatcher.BeginInvoke(new Action(() => BindListBoxItemsUI()));
        }

        private void BindListBoxItemsUI()
        {
            ListBoxItems.ItemsSource = ItemsList;
            BusyIndicatorItems.IsBusy = false;
        }

        private void WindowItemsManagement_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindListBoxItems();
        }
    }
}
