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

        }

        private void RibbonButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void Start()
        {
            BusyIndicator.IsBusy = true;

            var items = ProcessDataServiceClient.GetItemsAll();
            var tabs = ProcessDataServiceClient.GetTabsAll();
            var tabItems = ProcessDataServiceClient.GetTabItemsAll();

            foreach (var itemsAioViewModel in items.OrderBy(x=>x.ItemType))
            {
                ItemsAIOViewModel model = itemsAioViewModel;
                var tabIds = tabItems.Where(x => x.ItemId == model.ItemId);

                foreach (var tabsItemsViewModel in tabIds)
                {
                    string tabName = tabs.FirstOrDefault(x => x.TabId == tabsItemsViewModel.TabId).TabName;

                    WrapPanel wrapPanel = (WrapPanel) this.FindName(tabName);

                    ChartLiveData chartLiveData = new ChartLiveData();
                    chartLiveData.ItemsAioViewModel = model;
                    chartLiveData.ProcessDataServiceClient = this.ProcessDataServiceClient;
                    
                    // TODO Parameter
                    chartLiveData.Width = 200;

                    // TODO Parameter
                    chartLiveData.Height = 200;

                    // TODO Parameter
                    chartLiveData.Margin=new Thickness(4,2,4,2);

                    wrapPanel.Children.Add(chartLiveData);
                    chartLiveData.Start();
                }
            }

            BusyIndicator.IsBusy = false;
        }
    }
}
