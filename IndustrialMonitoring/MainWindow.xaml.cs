using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IndustrialMonitoring.ProcessDataServiceReference;
using IndustrialMonitoring.UserServiceReference;
using Telerik.Windows.Controls;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessDataServiceClient _processDataServiceClient=new ProcessDataServiceClient();
        private UserServiceClient _userServiceClient=new UserServiceClient();
        private event EventHandler StartAsyncCompleted;
        private List<ChartLiveData> _allCharts=new List<ChartLiveData>();
        private List<ChartLiveData> _ItemsForCompare=new List<ChartLiveData>(); 

        protected virtual void OnStartAsyncCompleted()
        {
            EventHandler handler = StartAsyncCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get
            {
                return _processDataServiceClient;
            }
            set { _processDataServiceClient = value; }
        }

        public List<ChartLiveData> AllCharts
        {
            get { return _allCharts; }
            set { _allCharts = value; }
        }

        public UserServiceClient UserServiceClient
        {
            get { return _userServiceClient; }
            set { _userServiceClient = value; }
        }

        public List<ChartLiveData> ItemsForCompare
        {
            get { return _ItemsForCompare; }
            set { _ItemsForCompare = value; }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void RibbonButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (AllCharts==null)
            {
                StartAsync();    
            }
            else if (AllCharts.Count == 0)
            {
                StartAsync();
            }
            else if (AllCharts.Count>0)
            {
                Resume();
            }

            RibbonButtonStart.IsEnabled = false;
            RibbonButtonStop.IsEnabled = true;
        }

        private void Resume()
        {
            if (AllCharts == null)
            {
                return;
            }

            foreach (var chartLiveData in AllCharts)
            {
                chartLiveData.Start();
            }
        }

        private void StartAsync()
        {
            Thread t1=new Thread(Start);

            this.StartAsyncCompleted += MainWindow_StartAsyncCompleted;
            BusyIndicator.IsBusy = true;

            t1.Start();
        }

        void MainWindow_StartAsyncCompleted(object sender, EventArgs e)
        {
            // TODO Parameter
            TabControlIOs.SelectedIndex = 0;
            BusyIndicator.IsBusy = false;
        }

        private void Start()
        {
            var tabs = ProcessDataServiceClient.GetTabsAll();

            foreach (var tabsViewModel in tabs)
            {
                if (UserServiceClient.UserHaveItemInTab(Lib.Static.CurrentUser.UserId, tabsViewModel.TabId))
                {
                    TabsViewModel model = tabsViewModel;
                    Dispatcher.BeginInvoke(new Action(() => GenerateTab(model)));   
                }
            }

            Dispatcher.BeginInvoke(new Action(OnStartAsyncCompleted));
        }

        private void GenerateTab(TabsViewModel tabsViewModel)
        {
            TabsViewModel model = tabsViewModel;
            var items = ProcessDataServiceClient.GetItemsForTab(model.TabId);

            RadTabItem radTabItem = new RadTabItem();
            radTabItem.Name = string.Format("TabItem{0}", tabsViewModel.TabName);
            radTabItem.Width = 80;
            radTabItem.Height = 25;
            radTabItem.Header = tabsViewModel.TabName;
            radTabItem.HorizontalContentAlignment = HorizontalAlignment.Center;
            radTabItem.VerticalContentAlignment = VerticalAlignment.Center;

            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Name = string.Format("WrapPanel{0}", tabsViewModel.TabName);

            radTabItem.Content = wrapPanel;

            TabControlIOs.Items.Add(radTabItem);

            foreach (var itemsAioViewModel in items)
            {
                if (!UserServiceClient.CheckPermission(Lib.Static.CurrentUser.UserId, itemsAioViewModel.ItemId))
                {
                    continue;
                }

                ChartLiveData chartLiveData = new ChartLiveData();
                chartLiveData.ItemsAioViewModel = itemsAioViewModel;
                chartLiveData.ProcessDataServiceClient = this.ProcessDataServiceClient;
                chartLiveData.MouseDoubleClick += chartLiveData_MouseDoubleClick;


                // TODO Parameter
                chartLiveData.Width = 200;

                // TODO Parameter
                chartLiveData.Height = 200;

                // TODO Parameter
                chartLiveData.Margin = new Thickness(4, 2, 4, 2);

                wrapPanel.Children.Add(chartLiveData);
                AllCharts.Add(chartLiveData);
                chartLiveData.Start();
            }
        }

        void chartLiveData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var current = sender as ChartLiveData;

            if (current.IsSelected)
            {
                current.IsSelected = false;
                return;
            }

            foreach (var chartLiveData in AllCharts)
            {
                chartLiveData.IsSelected = false;
            }

            
            current.IsSelected = true;
        }

        private void RibbonButtonStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (AllCharts == null)
            {
                return;
            }

            foreach (var chartLiveData in AllCharts)
            {
                chartLiveData.Stop();
            }

            RibbonButtonStart.IsEnabled = true;
            RibbonButtonStop.IsEnabled = false;
        }

        private void RibbonButtonChart_OnClick(object sender, RoutedEventArgs e)
        {
            ChartLiveData selected = null;

            foreach (var chartLiveData in AllCharts)
            {
                if (chartLiveData.IsSelected)
                {
                    selected = chartLiveData;
                    break;
                }
            }

            if (selected == null)
            {
                ShowMsgOnStatusBar("First select a item");
                return;
            }

            WindowChartHistory windowChartHistory=new WindowChartHistory();
            windowChartHistory.ProcessDataServiceClient = this.ProcessDataServiceClient;
            windowChartHistory.ItemId = selected.ItemsAioViewModel.ItemId;
            
            // TODO Parameter
            windowChartHistory.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

            // TODO Parameter
            windowChartHistory.EndTime = DateTime.Now;

            windowChartHistory.Show();
            windowChartHistory.ShowData();
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

        private void RibbonButtonGrid_OnClick(object sender, RoutedEventArgs e)
        {
            ChartLiveData selected = null;

            foreach (var chartLiveData in AllCharts)
            {
                if (chartLiveData.IsSelected)
                {
                    selected = chartLiveData;
                    break;
                }
            }

            if (selected == null)
            {
                ShowMsgOnStatusBar("First select a item");
                return;
            }

            WindowGridHistory windowChartHistory = new WindowGridHistory();
            windowChartHistory.ProcessDataServiceClient = this.ProcessDataServiceClient;
            windowChartHistory.ItemId = selected.ItemsAioViewModel.ItemId;

            // TODO Parameter
            windowChartHistory.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

            // TODO Parameter
            windowChartHistory.EndTime = DateTime.Now;

            windowChartHistory.Show();
            windowChartHistory.ShowData();
        }

        private void MenuItemAddItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ChartLiveData selected = null;

            foreach (var chartLiveData in AllCharts)
            {
                if (chartLiveData.IsSelected)
                {
                    selected = chartLiveData;
                    break;
                }
            }

            if (selected == null)
            {
                ShowMsgOnStatusBar("First select a item");
                return;
            }

            ItemsForCompare.Add(selected);
            MenuItem newMenuItem=new MenuItem();
            newMenuItem.Header = selected.ItemsAioViewModel.ItemName;
            MenuItemItems.Items.Add(newMenuItem);
            MenuItemClearItems.IsEnabled = true;
        }

        private void MenuItemClearItems_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
        	ItemsForCompare.Clear();
            MenuItemItems.Items.Clear();
            MenuItemClearItems.IsEnabled = false;
        }

        private void RibbonDropDownButtonCompare_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	if (ItemsForCompare.Any())
        	{
        	    MenuItemClearItems.IsEnabled = true;
        	}
        }
    }
}
