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
using SharedLibrary;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowNotifications.xaml
    /// </summary>
    public partial class WindowNotifications : Window
    {
        IndustrialMonitoringEntities _entities=new IndustrialMonitoringEntities();

        public WindowNotifications()
        {
            InitializeComponent();
        }

        public IndustrialMonitoringEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        private void WindowNotifications_OnLoaded(object sender, RoutedEventArgs e)
        {
            var items = Entities.Items;
            List<ItemTabViewModel> source=new List<ItemTabViewModel>();

            foreach (Item item in items)
            {
                var tab = Entities.TabsItems.FirstOrDefault(x => x.ItemId == item.ItemId);
                string name = string.Format("{0} - {1}", tab.Tab.TabName, item.ItemName);
                ItemTabViewModel itemTabViewModel=new ItemTabViewModel(item.ItemId,tab.TabId,name);

                source.Add(itemTabViewModel);
            }

            ComboBoxItems.ItemsSource = source.OrderBy(x=>x.Name);
        }

        private void ComboBoxItems_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (ItemTabViewModel)ComboBoxItems.SelectedItem;
            
            if (item != null)
            {
                GenerateListBox();

                ButtonAdd.IsEnabled = true;
                ButtonEdit.IsEnabled = true;
                ButtonDelete.IsEnabled = true;
            }
        }

        public void GenerateListBox()
        {
            var item = (ItemTabViewModel)ComboBoxItems.SelectedItem;
            var notifications = Entities.NotificationItems.Where(x => x.ItemId == item.ItemId).ToList();

            ListBoxNotifications.ItemsSource = notifications;
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (ItemTabViewModel)ComboBoxItems.SelectedItem;

            WindowDefineNotification windowDefineNotification=new WindowDefineNotification();
            NotificationViewModel model = new NotificationViewModel();
            windowDefineNotification.Notification = model;
            windowDefineNotification.Reference=this;
            windowDefineNotification.ItemId = item.ItemId;
            windowDefineNotification.Show();
        }

        public void AddNotification(int itemId,NotificationViewModel item)
        {
            NotificationItem notificationItem=new NotificationItem();
            notificationItem.ItemId = itemId;
            notificationItem.NotificationType = (int)item.Type;
            notificationItem.High = item.High;
            notificationItem.Low = item.Low;
            notificationItem.NotificationMsg = item.Message;
            notificationItem.Priority = item.Priority;

            Entities.NotificationItems.Add(notificationItem);

            Entities.SaveChanges();


            GenerateListBox();
            MessageBox.Show("Item added");
        }

        public void EditNotification(int notificationId, int itemId, NotificationViewModel item)
        {
            var notificaton = Entities.NotificationItems.FirstOrDefault(x => x.NotificationId == notificationId);
            if (notificaton != null)
            {
                notificaton.ItemId = itemId;
                notificaton.NotificationType = (int) item.Type;
                notificaton.High = item.High;
                notificaton.Low = item.Low;
                notificaton.NotificationMsg = item.Message;
                notificaton.Priority = item.Priority;

                Entities.SaveChanges();

                GenerateListBox();
                MessageBox.Show("Item edited");
            }
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItemCombo = (ItemTabViewModel)ComboBoxItems.SelectedItem;
            var selectedItem = (NotificationItem)ListBoxNotifications.SelectedItem;

            if (selectedItem != null)
            {
                NotificationViewModel model = new NotificationViewModel();
                model.Type = (NotificationType)selectedItem.NotificationType;
                if (selectedItem.Low != null) model.Low = selectedItem.Low.Value;
                if (selectedItem.High != null) model.High = selectedItem.High.Value;
                model.Message = selectedItem.NotificationMsg;
                if (selectedItem.Priority != null) model.Priority = selectedItem.Priority.Value;

                WindowDefineNotification windowDefineNotification = new WindowDefineNotification();
                windowDefineNotification.Notification = model;
                windowDefineNotification.NotificationId = selectedItem.NotificationId;
                windowDefineNotification.ItemId = selectedItemCombo.ItemId;
                windowDefineNotification.Reference = this;
                windowDefineNotification.Show();
            }
        }

        private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var result= MessageBox.Show("Are you sure you want to delete this item?","Delete",MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var selectedItem = (NotificationItem)ListBoxNotifications.SelectedItem;

                if (selectedItem != null)
                {
                    var item1 =
                        Entities.NotificationItemsLogLatests.FirstOrDefault(
                            x => x.NotificationId == selectedItem.NotificationId);

                    if (item1 != null)
                    {
                        Entities.NotificationItemsLogLatests.Remove(item1);
                    }

                    var items2 =
                        Entities.NotificationItemsLogs.Where(x => x.NotificationId == selectedItem.NotificationId);

                    if (items2.Any())
                    {
                        Entities.NotificationItemsLogs.RemoveRange(items2);
                    }

                    var item= Entities.NotificationItems.FirstOrDefault(x => x.NotificationId == selectedItem.NotificationId);
                    Entities.NotificationItems.Remove(item);

                    Entities.SaveChanges();

                    GenerateListBox();
                    MessageBox.Show("Item removed");
                }
            }
        }
    }
}
