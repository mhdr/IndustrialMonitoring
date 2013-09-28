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
using MonitoringAdmin.DataCollectorServiceReference;
using MonitoringAdmin.ProcessDataServiceReference;
using SharedLibrary;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowAddItems.xaml
    /// </summary>
    public partial class WindowAddItems : Window
    {
        private ProcessDataServiceClient _proxyProcessDataServiceClient=new ProcessDataServiceClient();
        private DataCollectorServiceReference.DataCollectorServiceClient _proxyDataCollectorServiceClient=new DataCollectorServiceClient();
        public event EventHandler AddItemCompletedSuccessfully;
        public MainWindow MainWindow { get; set; }
        private volatile ServerStatus serverStatusBeforeAction;

        protected virtual void OnAddItemCompletedSuccessfully()
        {
            EventHandler handler = AddItemCompletedSuccessfully;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public WindowAddItems()
        {
            InitializeComponent();
        }


        public DataCollectorServiceClient ProxyDataCollectorServiceClient
        {
            get { return _proxyDataCollectorServiceClient; }
            set { _proxyDataCollectorServiceClient = value; }
        }

        public ProcessDataServiceClient ProxyProcessDataServiceClient
        {
            get { return _proxyProcessDataServiceClient; }
            set { _proxyProcessDataServiceClient = value; }
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

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            AddItemAsync();
        }

        public void AddItemAsync()
        {
            Item2 item=new Item2();
            item.ItemName = TextBoxItemName.Text;
            ItemType itemType = ItemType.Unknown;

            if (ComboBoxItemType.SelectedIndex == 0)
            {
                itemType=ItemType.Digital;
            }
            else if (ComboBoxItemType.SelectedIndex == 1)
            {
                itemType=ItemType.Analog;
            }

            item.ItemType = itemType;

            item.Location = TextBoxLocation.Text;


            item.SaveInItemsLogTimeInterval = Convert.ToInt32(NumericUpDownSaveInItemsLogTimeInterval.Value);
            item.SaveInItemsLogLastesTimeInterval = Convert.ToInt32(NumericUpDownSaveInItemsLogLastesTimeInterval.Value);
            item.ShowInUITimeInterval = Convert.ToInt32(NumericUpDownShowInUITimeInterval.Value);
            item.ScanCycle = Convert.ToInt32(NumericUpDownScanCycle.Value);

            WhenToLog whenToLog1=WhenToLog.Unknown;

            if (ComboBoxSaveInItemsLogWhen.SelectedIndex == 0)
            {
                whenToLog1=WhenToLog.OnTimerElapsed;
            }
            else if (ComboBoxSaveInItemsLogWhen.SelectedIndex == 1)
            {
                whenToLog1=WhenToLog.OnChange;
            }

            item.SaveInItemsLogWhen = whenToLog1;

            WhenToLog whenToLog2=WhenToLog.Unknown;

            if (ComboBoxSaveInItemsLogLastWhen.SelectedIndex == 0)
            {
                whenToLog2 = WhenToLog.OnTimerElapsed;
            }
            else if (ComboBoxSaveInItemsLogLastWhen.SelectedIndex == 1)
            {
                whenToLog2 = WhenToLog.OnChange;
            }

            item.SaveInItemsLogLastWhen = whenToLog2;



            Thread t1=new Thread(()=>AddItem(item));
            t1.Start();
        }

        private void AddItem(Item2 item)
        {
            serverStatusBeforeAction = ProxyDataCollectorServiceClient.GetServerStatus();

            if (serverStatusBeforeAction == ServerStatus.Run)
            {
                StopDataCollector();
            }

            bool result= ProxyProcessDataServiceClient.AddItem2(item);

            Dispatcher.BeginInvoke(new Action(() => AddItemUI(result)));
        }

        private void AddItemUI(bool status)
        {
            if (status)
            {
                OnAddItemCompletedSuccessfully();
                ShowMsgOnStatusBar("Item Added Successfully");
            }
            else
            {
                ShowMsgOnStatusBar("Item Failed to Add");
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
    }
}
