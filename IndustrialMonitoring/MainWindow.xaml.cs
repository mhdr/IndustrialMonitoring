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
using IndustrialMonitoring.Lib;
using IndustrialMonitoring.ProcessDataServiceReference;
using IndustrialMonitoring.UserServiceReference;
using IndustrialMonitoring.NotificationServiceReference;
using Telerik.Windows;
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
        private NotificationServiceClient _notificationServiceClient=new NotificationServiceClient();
        private Timer timerNotifications;
        private Horn horn;
        private bool BlackAllTabs = false;
        

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

        public NotificationServiceClient NotificationServiceClient
        {
            get { return _notificationServiceClient; }
            set { _notificationServiceClient = value; }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
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
                    Tab1 model = tabsViewModel;
                    Dispatcher.BeginInvoke(new Action(() => GenerateTab(model)));   
                }
            }
            
            Dispatcher.BeginInvoke(new Action(OnStartAsyncCompleted));

            InitializeHorn();
        }

        private void GenerateTab(Tab1 tabsViewModel)
        {
            Tab1 model = tabsViewModel;
            var items = ProcessDataServiceClient.GetItemsForTab(model.TabId);

            RadTabItem radTabItem = new RadTabItem();
            radTabItem.Name = string.Format("TabItem{0}", tabsViewModel.TabName);
            radTabItem.Width = 80;
            radTabItem.Height = 25;

            TabHeaderUserControl tabHeader=new TabHeaderUserControl();
            tabHeader.SetHeader(tabsViewModel.TabName);

            radTabItem.Header = tabHeader;
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
                chartLiveData.NotificationServiceClient = this.NotificationServiceClient;
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

            //TODO Parameters
            if (ItemsForCompare.Count >= 15)
            {
                ShowMsgOnStatusBar(string.Format("Maximum number of items(={0}) for Compare is reached", 15));
                return;
            }

            if (ItemsForCompare.IndexOf(selected) >= 0)
            {
                ShowMsgOnStatusBar("This item already exists in Compare List");
                return;
            }

            ItemsForCompare.Add(selected);

            MenuItem newMenuItem=new MenuItem();
            newMenuItem.Header = selected.ItemsAioViewModel.ItemName;

            MenuItem newContextMenuItem = new MenuItem();
            newContextMenuItem.Header = selected.ItemsAioViewModel.ItemName;

            MenuItemItems.Items.Add(newMenuItem);
            ContextMenuItemItems.Items.Add(newContextMenuItem);
            MenuItemClearItems.IsEnabled = true;
            ContextMenuItemClearItems.IsEnabled = true;
        }

        private void MenuItemClearItems_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
        	ItemsForCompare.Clear();
            MenuItemItems.Items.Clear();
            ContextMenuItemItems.Items.Clear();
            MenuItemClearItems.IsEnabled = false;
            ContextMenuItemClearItems.IsEnabled = false;
        }

        private void OpenWindowChartHistoryInCompareMode(ChartType chartType)
        {
            if (ItemsForCompare.Count < 2)
            {
                ShowMsgOnStatusBar(string.Format("At least 2 Item is needed for Compare,you have selected {0}",
                    ItemsForCompare.Count));
                return;
            }

            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            int i = 0;
            foreach (var chartLiveData in ItemsForCompare)
            {
                dictionary.Add(i, chartLiveData.ItemsAioViewModel.ItemId);
                i++;
            }

            WindowChartHistory windowChartHistory = new WindowChartHistory();
            windowChartHistory.ProcessDataServiceClient = this.ProcessDataServiceClient;
            windowChartHistory.ItemsId = dictionary;
            windowChartHistory.ChartType = chartType;

            // TODO Parameter
            windowChartHistory.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

            // TODO Parameter
            windowChartHistory.EndTime = DateTime.Now;

            windowChartHistory.Show();
            windowChartHistory.ShowData();
        }

        private void MenuItemStart_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (AllCharts == null)
            {
                StartAsync();
            }
            else if (AllCharts.Count == 0)
            {
                StartAsync();
            }
            else if (AllCharts.Count > 0)
            {
                Resume();
            }

            MenuItemStart.IsEnabled = false;
            MenuItemStop.IsEnabled = true;
        }

        private void InitializeHorn()
        {
            timerNotifications = new Timer(CheckNotifications, null, 0, 1000);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                horn = Horn.GetInstance();    
            }));
        }

        private void CheckNotifications(object state)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                bool hastNotification = NotificationServiceClient.SystemHasNotification(Lib.Static.CurrentUser.UserId);

                if (hastNotification)
                {
                    horn.Start();
                    List<string> tabs =
                        NotificationServiceClient.TabsWithActiveNotification(Lib.Static.CurrentUser.UserId);

                    foreach (var item in TabControlIOs.Items)
                    {
                        RadTabItem tabItem = (RadTabItem)item;
                        TabHeaderUserControl tabHeader = (TabHeaderUserControl) tabItem.Header;
                        string header = tabHeader.GetHeader();

                        if (tabs.Any(x=>x.Equals(header)))
                        {
                            tabHeader.ShowAlarmAnimation();
                        }
                        else
                        {
                            tabHeader.HideAlarmAnimation();
                        }
                    }

                    BlackAllTabs = true;
                }
                else
                {
                    horn.Stop();

                    if (BlackAllTabs)
                    {
                        foreach (var item in TabControlIOs.Items)
                        {
                            RadTabItem tabItem = (RadTabItem)item;
                            TabHeaderUserControl tabHeader = (TabHeaderUserControl)tabItem.Header;
                            tabHeader.HideAlarmAnimation();
                        }

                        BlackAllTabs = false;
                    }
                }
            }));
        }

        private void MenuItemStop_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (AllCharts == null)
            {
                return;
            }

            foreach (var chartLiveData in AllCharts)
            {
                chartLiveData.Stop();
            }

            MenuItemStart.IsEnabled = true;
            MenuItemStop.IsEnabled = false;
        }

        private void MenuItemGrid_OnClick(object sender, RadRoutedEventArgs e)
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

        private void MenuItemCompareData_OnClick(object sender, RadRoutedEventArgs e)
        {
            if (ItemsForCompare.Any())
            {
                MenuItemClearItems.IsEnabled = true;
            }
        }

        private void MenuItemShowNotifications_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItemShowNotificationForCurrentItem_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItemLineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.LineSeries);
        }

        private void OpenWindowsCharthistory(ChartType chartType)
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


            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            dictionary.Add(0, selected.ItemsAioViewModel.ItemId);
            WindowChartHistory windowChartHistory = new WindowChartHistory();
            windowChartHistory.ProcessDataServiceClient = this.ProcessDataServiceClient;
            windowChartHistory.ItemsId = dictionary;

            // TODO Parameter
            windowChartHistory.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

            // TODO Parameter
            windowChartHistory.EndTime = DateTime.Now;
            windowChartHistory.ChartType = chartType;

            windowChartHistory.Show();
            windowChartHistory.ShowData();
        }

        private void MenuItemSplineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.SplineSeries);
        }

        private void MenuItemStepLineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.StepLineSeries);
        }

        private void MenuItemPointSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.PointSeries);
        }

        private void MenuItemAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.AreaSeries);
        }

        private void MenuItemSplineAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.SplineAreaSeries);
        }

        private void MenuItemStepAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowsCharthistory(ChartType.StepAreaSeries);
        }

        private void ContextMenuItemLineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.LineSeries);
        }

        private void ContextMenuItemSplineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.SplineSeries);
        }

        private void ContextMenuItemStepLineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.StepLineSeries);
        }

        private void ContextMenuItemPointSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.PointSeries);
        }

        private void ContextMenuItemAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.AreaSeries);
        }

        private void ContextMenuItemSplineAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.SplineAreaSeries);
        }

        private void ContextMenuItemStepAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            OpenWindowChartHistoryInCompareMode(ChartType.StepAreaSeries);
        }
    }
}
