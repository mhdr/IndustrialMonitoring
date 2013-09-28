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
using IndustrialMonitoring.ProcessDataServiceReference;
using Telerik.Windows;
using Telerik.Windows.Controls.ChartView;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowGridHistory.xaml
    /// </summary>
    public partial class WindowGridHistory : Window
    {
        public WindowGridHistory()
        {
            InitializeComponent();
        }

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

        private Item1 CurrentItem;

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


        void WindowChartHistory_ShowDataCompleted(object sender, EventArgs e)
        {
            TextBlockTitle.Text = string.Format("{0} {1}-{2}", CurrentItem.ItemName, this.StartTime.ToString(), this.EndTime.ToString());
            GridView.ItemsSource = ItemsLog;

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

        void dialogSetTime_TimeChanged(object sender, Lib.TimeChangedEventArgs e)
        {
            this.StartTime = e.StartTime;
            this.EndTime = e.EndTime;

            ShowData();
        }


        private void WindowGridHistory_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ShowDataCompleted += WindowChartHistory_ShowDataCompleted;
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

        private void MenuItemShowSetTimeDialog_OnClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            DialogSetTime dialogSetTime = new DialogSetTime();
            dialogSetTime.TimeChanged += dialogSetTime_TimeChanged;
            dialogSetTime.StartTime = StartTime;
            dialogSetTime.EndTime = EndTime;
            dialogSetTime.ShowDialog();
        }
    }
}
