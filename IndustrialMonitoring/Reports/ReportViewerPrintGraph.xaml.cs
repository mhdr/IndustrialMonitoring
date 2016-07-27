using System.Windows;
using Telerik.Reporting;

namespace IndustrialMonitoring.Reports
{
    public partial class ReportViewerPrintGraph : Window
    {
        public string TitleValue;
        public object ImageValue;
        public ReportViewerPrintGraph()
        {
            InitializeComponent();
        }

        private void ReportViewerPrintGraph_OnLoaded(object sender, RoutedEventArgs e)
        {
            InstanceReportSource instanceReportSource = (InstanceReportSource)ReportViewer1.ReportSource;
            Report report = (Report)instanceReportSource.ReportDocument;
            PictureBox pictureBoxGraph = (PictureBox)report.Items.Find("pictureBoxGraph", true)[0];
            TextBox textBoxTitle = (TextBox)report.Items.Find("textBoxTitle", true)[0];

            pictureBoxGraph.Value = ImageValue;
            textBoxTitle.Value = TitleValue;
        }
    }
}