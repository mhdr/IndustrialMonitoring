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
using Telerik.Windows.Controls.ChartView;

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for AIO.xaml
    /// </summary>
    public partial class AIO : UserControl
    {
        
        public AIO()
        {
            InitializeComponent();
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
            BusyIndicator.IsBusy = true;


            InitChart();

            BusyIndicator.IsBusy = false;
        }
    }
}
