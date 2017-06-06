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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IndustrialMonitoring.Lib;
using IndustrialMonitoring.NotificationServiceReference;
using SharedLibrary;
using Telerik.Windows.Controls.ChartView;
using IndustrialMonitoring.ProcessDataServiceReference;
using System.Threading;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for ChartLiveData.xaml
    /// </summary>
    public partial class ChartLiveData : UserControl
    {
        private Item1 _itemsAioViewModel = null;
        private Timer _timer;
        //private Timer _timerAnimation;
        private ObservableCollection<Lib.ChartLiveData> _observableCollection;
        private ItemsLogLatestAIOViewModel _latestData;
        private ProcessDataServiceClient _processDataServiceClient;
        private bool _isSelected;
        private NotificationServiceClient _notificationServiceClient;
        private bool _hasNotification;
        private bool isAnimationActive = false;
        private Storyboard storyboard;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                if (_isSelected)
                {
                    BorderUI.BorderBrush = Brushes.LightBlue;
                }
                else
                {
                    BorderUI.BorderBrush = Brushes.AliceBlue;
                }
            }
        }

        public ChartLiveData()
        {
            InitializeComponent();
        }

        public Item1 ItemsAioViewModel
        {
            get { return _itemsAioViewModel; }
            set { _itemsAioViewModel = value; }
        }

        public Timer Timer
        {
            get { return _timer; }
            set { _timer = value; }
        }

        public ObservableCollection<Lib.ChartLiveData> ObservableCollection
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

        public NotificationServiceClient NotificationServiceClient
        {
            get { return _notificationServiceClient; }
            set { _notificationServiceClient = value; }
        }

        public bool HasNotification
        {
            get { return _hasNotification; }
            set { _hasNotification = value; }
        }

        //public Timer TimerAnimation
        //{
        //    get { return _timerAnimation; }
        //    set { _timerAnimation = value; }
        //}

        private void InitChart()
        {
            try
            {
                Chart.Series.Clear();

                if (this.ItemsAioViewModel.ItemType == ItemType.Digital)
                {
                    Chart.Series.Add(new StepAreaSeries());
                    //Chart.Series.Add(new AreaSeries());
                    //AreaSeries series = (AreaSeries)this.Chart.Series[0];
                    StepAreaSeries series = (StepAreaSeries)this.Chart.Series[0];
                    series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
                    series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
                    series.Stroke = Brushes.LightBlue;
                    series.Fill = Brushes.LightBlue;
                    series.StrokeThickness = 2;

                    series.ItemsSource = ObservableCollection;
                }
                else if (this.ItemsAioViewModel.ItemType == ItemType.Analog)
                {

                    Chart.Series.Add(new LineSeries());
                    LineSeries series = (LineSeries)this.Chart.Series[0];
                    series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
                    series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
                    series.Stroke = Brushes.Blue;
                    series.StrokeThickness = 2;

                    series.ItemsSource = ObservableCollection;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        private void AIO_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBlockTitle.Text = this.ItemsAioViewModel.ItemName;
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        public void Start()
        {
            try
            {
                if (storyboard == null)
                {
                    storyboard = (Storyboard)FindResource("StoryboardAnim");
                }

                Timer = new Timer(ShowLiveData, new object(), 0, this.ItemsAioViewModel.ShowInUITimeInterval * 1000);
                //TimerAnimation = new Timer(ShowAnimation, new object(),0,3*1000);
                ObservableCollection = new ObservableCollection<Lib.ChartLiveData>();

                InitChart();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        public void Stop()
        {
            try
            {
                Timer.Dispose();
                //TimerAnimation.Dispose();
                storyboard.Stop();
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        private void ShowLiveData(object state)
        {
            try
            {
                LatestData = ProcessDataServiceClient.GeItemsLogLatest(this.ItemsAioViewModel.ItemId);
                HasNotification = NotificationServiceClient.HasNotification(this.ItemsAioViewModel.ItemId);

                if (LatestData == null)
                {
                    return;
                }

                Dispatcher.Invoke(new Action(ShowLiveDataUI));
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        private void ShowLiveDataUI()
        {
            try
            {
                // TODO Parameter
                if (ObservableCollection.Count > 49)
                {
                    ObservableCollection.RemoveAt(0);
                }

                ObservableCollection.Add(new Lib.ChartLiveData() { Value = Convert.ToDouble(LatestData.Value), Date = DateTime.Now });

                if (ItemsAioViewModel.Unit.Length > 0)
                {
                    TextBlockValue.Text = string.Format("{0} {1}", LatestData.Value, ItemsAioViewModel.Unit);
                }
                else
                {
                    TextBlockValue.Text = string.Format("{0}", LatestData.Value);
                }
                

                System.Diagnostics.Debug.WriteLine("Item Name : {0} , Count ObservableCollection : {1}",
                    this.ItemsAioViewModel.ItemName, ObservableCollection.Count);

                if (HasNotification)
                {
                    if (isAnimationActive == false)
                    {
                        storyboard.Begin();
                        isAnimationActive = true;
                    }

                }
                else
                {
                    storyboard.Stop();
                    isAnimationActive = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        private void ShowAnimation(object state)
        {
            try
            {
                HasNotification = NotificationServiceClient.HasNotification(this.ItemsAioViewModel.ItemId);

                Dispatcher.Invoke(new Action(ShowAnimationUI));
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        public int GetWidthOfTitle()
        {
            int value = TextBlockTitle.Text.Length;
            return value;
        }

        private void ShowAnimationUI()
        {
            try
            {
                if (HasNotification)
                {
                    if (isAnimationActive == false)
                    {
                        storyboard.Begin();
                        isAnimationActive = true;
                    }

                }
                else
                {
                    storyboard.Stop();
                    isAnimationActive = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogIndustrialMonitoring(ex);
            }
        }

        //private void TextBlockTitle_OnSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    double size = TextBlockTitle.Width + 20;

        //    if (size <= 240)
        //    {
        //        this.Width = 240;
        //    }
        //    else
        //    {
        //        this.Width = TextBlockTitle.Width + 10;
        //    }
        //}
    }
}
