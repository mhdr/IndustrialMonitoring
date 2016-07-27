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
using System.Windows.Shapes;
using MonitoringAdmin.Lib;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowAddBACnet.xaml
    /// </summary>
    public partial class WindowAddBACnet : Window
    {
        public WindowAddBACnet()
        {
            InitializeComponent();
        }

        private void WindowAddBACnet_OnLoaded(object sender, RoutedEventArgs e)
        {
            BACnetItem item=new BACnetItem();
            DataFormBACnet.CurrentItem = item;
        }

        private void DataFormBACnet_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            BACnetItem item = (BACnetItem)DataFormBACnet.CurrentItem;

            Item newItem = new Item();
            newItem.ItemName = item.ItemName;
            newItem.ItemType = item.ItemType;
            newItem.SaveInItemsLogTimeInterval = item.SaveInItemsLogTimeInterval;
            newItem.SaveInItemsLogLastesTimeInterval = item.SaveInItemsLogLastesTimeInterval;
            newItem.ShowInUITimeInterval = item.ShowInUiTimeInterval;
            newItem.ScanCycle = item.ScanCycle;
            newItem.SaveInItemsLogWhen = item.SaveInItemsLogWhen;
            newItem.SaveInItemsLogLastWhen = item.SaveInItemsLogLastWhen;
            newItem.DefinationType = item.DefinationType;
            newItem.Unit = item.Unit;
            newItem.Order = item.Order;
            newItem.BACnetIP = item.BaCnetIp;
            newItem.BACnetPort = item.BaCnetPort;
            newItem.BACnetControllerInstance = item.BaCnetControllerInstance;
            newItem.BACnetItemInstance = item.BaCnetItemInstance;
            newItem.BACnetItemType = item.BaCnetItemType;
            newItem.MinRange = item.MinRange;
            newItem.MaxRange = item.MaxRange;
            newItem.NormalizeWhenOutOfRange = item.NormalizeWhenOutOfRange;
            newItem.ThreadGroupId = item.ThreadGroupId;
            newItem.NumberOfDataForBoxplot = item.NumberOfDataForBoxplot;
            newItem.InOut = item.InOut;

            IndustrialMonitoringEntities entities = new IndustrialMonitoringEntities();
            entities.Items.Add(newItem);
            entities.SaveChanges();

            MessageBox.Show("New item saved successfully");
            try
            {
               
            }
            catch (Exception ex)
            {
                throw ex;
                MessageBox.Show(ex.Message);
            }

        }

        private void ButtonClear_OnClick(object sender, RoutedEventArgs e)
        {
            BACnetItem item = new BACnetItem();
            DataFormBACnet.CurrentItem = item;
        }
    }
}
