using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IndustrialMonitoring.Lib;
using Telerik.Windows.Controls.ChartView;
using IndustrialMonitoring.ProcessDataServiceReference;
using System.Threading;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for AIO.xaml
    /// </summary>
    public partial class AIO : UserControl
    {
        private ItemsAIOViewModel _itemsAioViewModel = null;
        private Timer _timer;
        private ObservableCollection<ChartLiveData> _observableCollection;
        private ItemsLogLatestAIOViewModel _latestData;
        private ProcessDataServiceClient _processDataServiceClient;

        public AIO()
        {
            InitializeComponent();
        }

        public ItemsAIOViewModel ItemsAioViewModel
        {
            get { return _itemsAioViewModel; }
            set { _itemsAioViewModel = value; }
        }

        public Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public ObservableCollection<ChartLiveData> ObservableCollection
        {
            get { return _observableCollection; }
            set { _observableCollection = value; }
        }

        public ItemsLogLatestAIOViewModel LatestData
        {
            get { return _latestData; }
            set { _latestData = value; }
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get { return _processDataServiceClient; }
            set { _processDataServiceClient = value; }
        }

        private void InitChart()
        {
            Chart.Series.Clear();
            Chart.Series.Add(new LineSeries());
            LineSeries series = (LineSeries)this.Chart.Series[0];
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
            series.Stroke = Brushes.Green;
            //series.Fill = Brushes.Green;
            series.StrokeThickness = 2;
        }

        private void AIO_OnLoaded(object sender, RoutedEventArgs e)
        {
//            InitChart();
        }

        public void Start()
        {
            Timer=new Timer(ShowLiveData,new object(), 0,this.ItemsAioViewModel.ShowInUITimeInterval * 1000);
            ObservableCollection=new ObservableCollection<ChartLiveData>();

            InitChart();
            LineSeries series = (LineSeries)this.Chart.Series[0];
            series.ItemsSource = ObservableCollection;
        }

        public void Stop()
        {
            Timer.Dispose();
        }

        private void ShowLiveData(object state)
        {
            LatestData = ProcessDataServiceClient.GeItemsLogLatest(this.ItemsAioViewModel.ItemId);

            if (LatestData == null)
            {
                return;
            }

            if (ObservableCollection.Count > 99)
            {
                ObservableCollection.RemoveAt(0);
            }

            ObservableCollection.Add(new ChartLiveData(){Value = Convert.ToDouble(LatestData.Value),Date = DateTime.Now});

            System.Diagnostics.Debug.WriteLine("Item Name : {0} , Count ObservableCollection : {1}",
                this.ItemsAioViewModel.ItemName,ObservableCollection.Count);
        }
    }
}
