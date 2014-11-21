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
using System.Windows.Media.Animation;
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
        private bool isAnimationActive = false;
        private Storyboard storyboard;

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

        public void ShowAlarmAnimation()
        {
            if (storyboard == null)
            {
                storyboard = (Storyboard)FindResource("StoryboardAnim");
            }

            if (isAnimationActive == false)
            {
                storyboard.Begin();
                isAnimationActive = true;
            }
        }

        public void HideAlarmAnimation()
        {
            if (storyboard == null)
            {
                storyboard = (Storyboard)FindResource("StoryboardAnim");
            }

            storyboard.Stop();
            isAnimationActive = false;
        }

        private void TabHeaderUserControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (storyboard == null)
            {
                storyboard = (Storyboard)FindResource("StoryboardAnim");
            }
        }
    }
}
