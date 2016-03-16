using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using SharedLibrary;
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
            try
            {
                if (Lib.Static.UserServicesPermission.Contains(1))
                {
                    MenuItemHorn.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void Resume()
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void StartAsync()
        {
            try
            {
                Thread t1 = new Thread(Start);

                this.StartAsyncCompleted += MainWindow_StartAsyncCompleted;
                BusyIndicator.IsBusy = true;

                t1.Start();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        void MainWindow_StartAsyncCompleted(object sender, EventArgs e)
        {
            try
            {
                // TODO Parameter
                TabControlIOs.SelectedIndex = 0;
                BusyIndicator.IsBusy = false;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void Start()
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void GenerateTab(Tab1 tabsViewModel)
        {
            try
            {
                Tab1 model = tabsViewModel;
                var items = ProcessDataServiceClient.GetItemsForTab(model.TabId);

                RadTabItem radTabItem = new RadTabItem();
                radTabItem.Name = string.Format("TabItem{0}", tabsViewModel.TabName).Replace(" ", "");
                radTabItem.MinWidth = 80;
                radTabItem.Height = 25;

                TabHeaderUserControl tabHeader = new TabHeaderUserControl();
                tabHeader.SetHeader(tabsViewModel.TabName);

                radTabItem.Header = tabHeader;
                radTabItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                radTabItem.VerticalContentAlignment = VerticalAlignment.Center;

                WrapPanel wrapPanel = new WrapPanel();
                wrapPanel.Name = string.Format("WrapPanel{0}", tabsViewModel.TabName).Replace(" ", "");

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
                    chartLiveData.Width = 240;

                    // TODO Parameter
                    chartLiveData.Height = 200;

                    // TODO Parameter
                    chartLiveData.Margin = new Thickness(4, 2, 4, 2);

                    wrapPanel.Children.Add(chartLiveData);
                    AllCharts.Add(chartLiveData);
                    chartLiveData.Start();
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        void chartLiveData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            try
            {
                ClearStatusBar();

                StatusBarBottom.Items.Add(msg);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ClearStatusBar()
        {
            try
            {
                StatusBarBottom.Items.Clear();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ClearStatusBar();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemAddItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
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

                MenuItem newMenuItem = new MenuItem();
                newMenuItem.Header = selected.ItemsAioViewModel.ItemName;

                MenuItem newContextMenuItem = new MenuItem();
                newContextMenuItem.Header = selected.ItemsAioViewModel.ItemName;

                MenuItemItems.Items.Add(newMenuItem);
                ContextMenuItemItems.Items.Add(newContextMenuItem);
                MenuItemClearItems.IsEnabled = true;
                ContextMenuItemClearItems.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemClearItems_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                ItemsForCompare.Clear();
                MenuItemItems.Items.Clear();
                ContextMenuItemItems.Items.Clear();
                MenuItemClearItems.IsEnabled = false;
                ContextMenuItemClearItems.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void OpenWindowChartHistoryInCompareMode(ChartType chartType)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemStart_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void InitializeHorn()
        {
            try
            {
                timerNotifications = new Timer(CheckNotifications, null, 0, 1000);
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    horn = Horn.GetInstance();
                }));
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void CheckNotifications(object state)
        {
            try
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
                                TabHeaderUserControl tabHeader = (TabHeaderUserControl)tabItem.Header;
                                string header = tabHeader.GetHeader();

                                if (tabs.Any(x => x.Equals(header)))
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemStop_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemGrid_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemCompareData_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                if (ItemsForCompare.Any())
                {
                    MenuItemClearItems.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemShowNotifications_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                WindowNotifications windowNotifications = new WindowNotifications();
                windowNotifications.NotificationServiceClient = this.NotificationServiceClient;
                // TODO Parameter
                windowNotifications.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

                // TODO Parameter
                windowNotifications.EndTime = DateTime.Now;
                windowNotifications.Show();
                windowNotifications.ShowData();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemShowNotificationForCurrentItem_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
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

                WindowNotifications windowNotifications = new WindowNotifications();
                windowNotifications.NotificationServiceClient = this.NotificationServiceClient;
                // TODO Parameter
                windowNotifications.StartTime = DateTime.Now - new TimeSpan(0, 24, 0, 0);

                // TODO Parameter
                windowNotifications.EndTime = DateTime.Now;
                windowNotifications.NotificationServiceClient = this.NotificationServiceClient;
                windowNotifications.ItemId = selected.ItemsAioViewModel.ItemId;
                windowNotifications.Show();
                windowNotifications.ShowData();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemLineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.LineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void OpenWindowsCharthistory(ChartType chartType)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemSplineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.SplineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemStepLineSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.StepLineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemPointSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.PointSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.AreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemSplineAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.SplineAreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemStepAreaSeries_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowsCharthistory(ChartType.StepAreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemLineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.LineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemSplineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.SplineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemStepLineSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.StepLineSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemPointSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.PointSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.AreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemSplineAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.SplineAreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void ContextMenuItemStepAreaSeriesCompare_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                OpenWindowChartHistoryInCompareMode(ChartType.StepAreaSeries);
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemChangePasswordWindow_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                WindowChangePassword windowChangePassword = new WindowChangePassword();
                windowChangePassword.UserServiceClient = this.UserServiceClient;
                windowChangePassword.ShowDialog();

                if (windowChangePassword.ChangePasswordCount > 0)
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemTools_OnSubmenuOpened(object sender, RadRoutedEventArgs e)
        {
            try
            {
                if (Static.CurrentUser == null)
                {
                    MenuItemChangePasswordWindow.IsEnabled = false;
                }

                if (string.IsNullOrEmpty(Static.CurrentUser.UserName))
                {
                    MenuItemChangePasswordWindow.IsEnabled = false;
                }

                MenuItemChangePasswordWindow.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemIssues_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/mhdr/IndustrialMonitoring/issues");
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemNewIssue_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/mhdr/IndustrialMonitoring/issues/new");
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }

        private void MenuItemHorn_OnClick(object sender, RadRoutedEventArgs e)
        {
            try
            {
                WindowHorn windowHorn = new WindowHorn();
                windowHorn.ProcessDataService = this.ProcessDataServiceClient;
                windowHorn.Show();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
                
            }
        }
    }
}
