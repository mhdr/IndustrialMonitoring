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
using System.Windows.Shapes;
using Telerik.Windows.Controls.ChartView;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for WindowChartHistory.xaml
    /// </summary>
    public partial class WindowChartHistory : Window
    {
        public WindowChartHistory()
        {
            InitializeComponent();
        }

        private void InitChart()
        {
            Chart.Series.Add(new LineSeries());
            LineSeries series = (LineSeries)this.Chart.Series[0];
            series.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Date" };
            series.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Value" };
            series.Stroke = Brushes.Green;
            series.StrokeThickness = 2;

            //series.ItemsSource = ObservableCollection<>;
        }

        private void WindowChartHistory_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitChart();
        }
    }
}
