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

namespace IndustrialMonitoring
{
    /// <summary>
    /// Interaction logic for TabHeaderUserControl.xaml
    /// </summary>
    public partial class TabHeaderUserControl : UserControl
    {
        public TabHeaderUserControl()
        {
            InitializeComponent();
        }

        public void SetHeader(string header)
        {
            TextBlockMain.Text = header;
        }

        public string GetHeader()
        {
            return TextBlockMain.Text;
        }

        public void ShowAlarmIcon()
        {
            ImageMain.Visibility=Visibility.Visible;
        }

        public void HideAlarmIcon()
        {
            ImageMain.Visibility=Visibility.Collapsed;
        }
    }
}
