using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IndustrialMonitoring.ProcessDataServiceReference;
using Telerik.Windows.Controls.ChartView;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowChartHistory.xaml
    /// </summary>
    public partial class WindowChartHistory : Window
    {
        private ProcessDataServiceClient _processDataServiceClient;
        private DateTime _startTime;
        private DateTime _endTime;
        private int _itemId;
        private List<ItemsLogChartHistoryViewModel> _itemsLog;
        private event EventHandler ShowDataCompleted;

        protected virtual void OnShowDataCompleted()
        {
            EventHandler handler = ShowDataCompleted;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private ItemsAIOViewModel CurrentItem;

        public WindowChartHistory()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get { return _processDataServiceClient; }
            set { _processDataServiceClient = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        public List<ItemsLogChartHistoryViewModel> ItemsLog
        {
            get { return _itemsLog; }
            set { _itemsLog = value; }
        }

        private void InitChart()
        {
            Chart.Series.Add(new LineSeries());
            LineSeries series = (LineSeries)this.Chart.Series[0];
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Time" };
            series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
            series.Stroke = Brushes.Green;
            series.StrokeThickness = 2;
        }

        private void WindowChartHistory_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitChart();

            this.ShowDataCompleted += WindowChartHistory_ShowDataCompleted;
        }

        void WindowChartHistory_ShowDataCompleted(object sender, EventArgs e)
        {
            RibbonViewTop.ApplicationName = string.Format("{0} {1}-{2}", CurrentItem.ItemName, this.StartTime.ToString(), this.EndTime.ToString());
            LineSeries series = (LineSeries)this.Chart.Series[0];
            series.ItemsSource = ItemsLog;
            BusyIndicator.IsBusy = false;
        }

        public void ShowData()
        {
            BusyIndicator.IsBusy = true;

            ItemsLog=new List<ItemsLogChartHistoryViewModel>();

            Thread t1=new Thread(ShowDataAsync);
            t1.Start();
        }

        private void ShowDataAsync()
        {
            CurrentItem = ProcessDataServiceClient.GetItem(ItemId);

            ItemsLog = ProcessDataServiceClient.GetItemLogs(ItemId, StartTime, EndTime);

            Dispatcher.BeginInvoke(new Action(OnShowDataCompleted));
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearStatusBar();
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

        private void RibbonButtonShowSetTimeDialog_OnClick(object sender, RoutedEventArgs e)
        {
            DialogSetTime dialogSetTime=new DialogSetTime();
            dialogSetTime.TimeChanged += dialogSetTime_TimeChanged;
            dialogSetTime.StartTime = StartTime;
            dialogSetTime.EndTime = EndTime;
            dialogSetTime.ShowDialog();
        }

        void dialogSetTime_TimeChanged(object sender, Lib.TimeChangedEventArgs e)
        {
            this.StartTime = e.StartTime;
            this.EndTime = e.EndTime;
        }

        private void RibbonButtonApply_OnClick(object sender, RoutedEventArgs e)
        {
            ShowData();
        }
    }
}
