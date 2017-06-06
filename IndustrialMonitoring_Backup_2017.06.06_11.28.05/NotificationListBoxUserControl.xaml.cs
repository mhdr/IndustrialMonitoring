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
    /// Interaction logic for NotificationListBoxUserControl.xaml
    /// </summary>
    public partial class NotificationListBoxUserControl : UserControl
    {
        private bool hasFault=false;

        public NotificationListBoxUserControl()
        {
            InitializeComponent();
        }

        public void SetItemName(string itemName)
        {
            TextBlockItemName.Text = itemName;
        }

        public void SetTime(DateTime time)
        {
            TextBlockNotificationTime.Text = time.ToString();
        }

        public void SetDesription(string description)
        {
            TextBlockDescription.Text = description;
        }

        public void SetCategory(string category)
        {
            TextBlockCategory.Text = category;
        }

        public string GetCategory()
        {
            return TextBlockCategory.Text;
        }

        public string GetItemName()
        {
            return TextBlockItemName.Text;
        }

        public DateTime GetTime()
        {
            return Convert.ToDateTime(TextBlockNotificationTime.Text);
        }

        public string GetDesription()
        {
            return TextBlockDescription.Text;
        }

        public void SetHasFault(bool _hasFault)
        {
            if (_hasFault)
            {
                hasFault = true;
                GridMain.Background = Brushes.LightPink;
            }
            else
            {
                hasFault = false;
                GridMain.Background = Brushes.PaleGreen;
            }
        }

        public bool GetHasFault()
        {
            return hasFault;
        }
    }
}
