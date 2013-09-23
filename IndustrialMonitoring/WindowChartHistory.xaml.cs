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
using IndustrialMonitoring.Lib;
using IndustrialMonitoring.ProcessDataServiceReference;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Legend;

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
        private Dictionary<int,int> _itemsId;
        //private List<ItemsLogChartHistoryViewModel> _itemsLog;
        private event EventHandler<ShowDataCompletedEventArgs> ShowDataCompleted;

        protected virtual void OnShowDataCompleted(ShowDataCompletedEventArgs e)
        {
            EventHandler<ShowDataCompletedEventArgs> handler = ShowDataCompleted;
            if (handler != null) handler(this, e);
        }

        private List<Brush> _chartBrushes;

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

        public Dictionary<int,int> ItemsId
        {
            get { return _itemsId; }
            set { _itemsId = value; }
        }

        public List<Brush> ChartBrushes
        {
            get { return _chartBrushes; }
            set { _chartBrushes = value; }
        }


        private void InitChart()
        {
            ChartBrushes=new List<Brush>();
            ChartBrushes.Add(Brushes.Blue);
            ChartBrushes.Add(Brushes.Green);
            ChartBrushes.Add(Brushes.LightSalmon);
            ChartBrushes.Add(Brushes.DarkRed);
            ChartBrushes.Add(Brushes.Gold);
            ChartBrushes.Add(Brushes.MediumBlue);
            ChartBrushes.Add(Brushes.CadetBlue);
            ChartBrushes.Add(Brushes.Coral);
            ChartBrushes.Add(Brushes.Chocolate);
            ChartBrushes.Add(Brushes.Crimson);
            ChartBrushes.Add(Brushes.Indigo);
            ChartBrushes.Add(Brushes.PaleGreen);
            ChartBrushes.Add(Brushes.OrangeRed);
            ChartBrushes.Add(Brushes.Purple);
            ChartBrushes.Add(Brushes.Fuchsia);

            int countItems = ItemsId.Count;

            if (ItemsId != null)
            {

                for (int i = 0; i < countItems; i++)
                {
                    Chart.Series.Add(new LineSeries());
                    LineSeries series = (LineSeries)this.Chart.Series[i];
                    series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Time" };
                    series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
                    series.Stroke = ChartBrushes[i];
                    series.StrokeThickness = 2;   
                }
            }
        }

        private void WindowChartHistory_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitChart();

            this.ShowDataCompleted += WindowChartHistory_ShowDataCompleted;
        }

        void WindowChartHistory_ShowDataCompleted(object sender, ShowDataCompletedEventArgs e)
        {
            if (ItemsId.Count == 1)
            {
                TextBlockTitle.Text = string.Format("{0} {1}-{2}", e.CurrentItem.ItemName, this.StartTime.ToString(), this.EndTime.ToString());    
            }
            else
            {

                TextBlockTitle.Text = string.Format("Compare {0}-{1}", this.StartTime.ToString(), this.EndTime.ToString());    
            }

            LineSeries series = (LineSeries)this.Chart.Series[e.ItemId.Key];
            series.ItemsSource =e.Data ;
            Chart.Zoom=new Size(1,1);

            if (e.GenerateLegend)
            {
                ChartLegend.Items.Add(new LegendItem() { MarkerFill = ChartBrushes[e.ItemId.Key], Title = e.CurrentItem.ItemName });    
            }

            BusyIndicator.IsBusy = false;
        }

        public void ShowData(bool generateLegend=true)
        {
            BusyIndicator.IsBusy = true;

            foreach (var dic in ItemsId)
            {
                KeyValuePair<int, int> dic1 = dic;
                Thread t1 = new Thread(()=>ShowDataAsync(dic1,generateLegend));
                t1.Start();
            }
        }

        private void ShowDataAsync(KeyValuePair<int,int> itemId,bool generateLegend)
        {
            Items1 CurrentItem = ProcessDataServiceClient.GetItem(itemId.Value);
            List<ItemsLogChartHistoryViewModel> ItemsLog = ProcessDataServiceClient.GetItemLogs(itemId.Value, StartTime, EndTime);

            Dispatcher.BeginInvoke(new Action(()=>OnShowDataCompleted(new ShowDataCompletedEventArgs( itemId,ItemsLog,CurrentItem,generateLegend))));
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

        void dialogSetTime_TimeChanged(object sender, Lib.TimeChangedEventArgs e)
        {
            this.StartTime = e.StartTime;
            this.EndTime = e.EndTime;
            ShowData(false);
        }

        private void MenuItemShowSetTimeDialog_OnMouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock textBlock1 = new TextBlock();
            textBlock1.Text = "Start Time : ";
            textBlock1.FontWeight = FontWeights.Bold;

            TextBlock textBlock2 = new TextBlock();
            textBlock2.Text = StartTime.ToString();

            StackPanel stackPanel1 = new StackPanel();
            stackPanel1.Orientation = Orientation.Horizontal;

            stackPanel1.Children.Add(textBlock1);
            stackPanel1.Children.Add(textBlock2);

            TextBlock textBlock3 = new TextBlock();
            textBlock3.Text = "End Time : ";
            textBlock3.FontWeight = FontWeights.Bold;

            TextBlock textBlock4 = new TextBlock();
            textBlock4.Text = EndTime.ToString();

            StackPanel stackPanel2 = new StackPanel();
            stackPanel2.Orientation = Orientation.Horizontal;

            stackPanel2.Children.Add(textBlock3);
            stackPanel2.Children.Add(textBlock4);

            StackPanel stackPanel3 = new StackPanel();
            stackPanel3.Children.Add(stackPanel1);
            stackPanel3.Children.Add(stackPanel2);

            MenuItemShowSetTimeDialog.ToolTip = stackPanel3;
        }

        private void MenuItemShowSetTimeDialog_OnClick(object sender, RadRoutedEventArgs e)
        {
            DialogSetTime dialogSetTime = new DialogSetTime();
            dialogSetTime.TimeChanged += dialogSetTime_TimeChanged;
            dialogSetTime.StartTime = StartTime;
            dialogSetTime.EndTime = EndTime;
            dialogSetTime.ShowDialog();
        }
    }
}
