using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using MonitoringAdmin.DataCollectorServiceReference;
using MonitoringAdmin.ProcessDataServiceReference;
using SharedLibrary;
using Telerik.Windows;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowItems.xaml
    /// </summary>
    public partial class WindowItems : Window
    {
        private ProcessDataServiceClient _proxyProcessDataServiceClient=new ProcessDataServiceClient();
        private DataCollectorServiceClient _proxyDataCollectorServiceClient=new DataCollectorServiceClient();
        private volatile ServerStatus serverStatusBeforeAction;
        public MainWindow MainWindow { get; set; }

        public WindowItems()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProxyProcessDataServiceClient
        {
            get { return _proxyProcessDataServiceClient; }
            set { _proxyProcessDataServiceClient = value; }
        }

        public DataCollectorServiceClient ProxyDataCollectorServiceClient
        {
            get { return _proxyDataCollectorServiceClient; }
            set { _proxyDataCollectorServiceClient = value; }
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            ClearStatusBar();

            StatusBarBottom.Items.Add(msg);
        }

        private void ClearStatusBar()
        {
            StatusBarBottom.Items.Clear();
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearStatusBar();
        }

        public void BindGridAsync()
        {
            BusyIndicator.IsBusy = true;
            Thread t1=new Thread(()=>BindGrid());
            t1.Start();
        }

        private void BindGrid()
        {
            List<Item2> items = ProxyProcessDataServiceClient.GetItems2();
            Dispatcher.BeginInvoke(new Action(() => BindGridUI(items)));
        }

        private void BindGridUI(List<Item2> items)
        {
            GridView.ItemsSource = items;
            BusyIndicator.IsBusy = false;
        }

        private void WindowItems_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindGridAsync();
        }

        void windowAddItems_AddItemCompletedSuccessfully(object sender, EventArgs e)
        {
            BindGridAsync();
        }

        private void DeleteItemAsync(int itemId)
        {
            BusyIndicator.IsBusy = true;
            Thread t2=new Thread(()=>DeleteItemDoWork(itemId));
            t2.Start();
        }

        private void DeleteItemDoWork(int itemId)
        {
            serverStatusBeforeAction = ProxyDataCollectorServiceClient.GetServerStatus();

            if (serverStatusBeforeAction == ServerStatus.Run)
            {
                StopDataCollector();
            }

            bool result= ProxyProcessDataServiceClient.DeleteItem(itemId);

            Dispatcher.BeginInvoke(new Action(() => DeleteItemUI(result)));
        }

        private void DeleteItemUI(bool result)
        {
            if (result)
            {
                ShowMsgOnStatusBar("Item Deleted Successfully");
                BindGridAsync();
            }
            else
            {
                BusyIndicator.IsBusy = false;
                ShowMsgOnStatusBar("Item Failed to Delete");
            }

            if (serverStatusBeforeAction == ServerStatus.Run)
            {
                StartDataCollector();
            }
        }

        private void StartDataCollector()
        {
            Dispatcher.BeginInvoke(new Action(() => MainWindow.StartDataColletor()));
        }

        private void StopDataCollector()
        {
            Dispatcher.BeginInvoke(new Action(() => MainWindow.StopDataColletor()));
        }

        void windowEditItems_AddItemCompletedSuccessfully(object sender, EventArgs e)
        {
            BindGridAsync();
        }

        private void MenuItemAdd_OnClick(object sender, RadRoutedEventArgs e)
        {
            WindowAddItems windowAddItems = new WindowAddItems();
            windowAddItems.MainWindow = MainWindow;
            windowAddItems.AddItemCompletedSuccessfully += windowAddItems_AddItemCompletedSuccessfully;
            windowAddItems.Show();
        }

        private void MenuItemEdit_OnClick(object sender, RadRoutedEventArgs e)
        {
            Item2 currentItem = (Item2)GridView.SelectedItem;

            if (currentItem == null)
            {
                ShowMsgOnStatusBar("First select an Item");
                return;
            }

            WindowEditItems windowEditItems = new WindowEditItems();
            windowEditItems.MainWindow = MainWindow;
            windowEditItems.ItemToEdit = currentItem;
            windowEditItems.AddItemCompletedSuccessfully += windowEditItems_AddItemCompletedSuccessfully;
            windowEditItems.Show();
        }

        private void MenuItemDelete_OnClick(object sender, RadRoutedEventArgs e)
        {
            Item2 currentItem = (Item2)GridView.SelectedItem;

            if (currentItem == null)
            {
                ShowMsgOnStatusBar("First select an Item");
                return;
            }

            DeleteItemAsync(currentItem.ItemId);
        }
    }
}
