using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using MonitoringAdmin.DataCollectorServiceReference;
using MonitoringAdmin.ProcessDataServiceReference;
using MonitoringAdmin.UsersServiceReference;
using Telerik.Windows;
using Telerik.Windows.Controls;

namespace MonitoringAdmin
{
    /// <summary>
    /// Interaction logic for WindowItemsManagement.xaml
    /// </summary>
    public partial class WindowItemsManagement : Window
    {
        private ProcessDataServiceClient _processDataServiceClient=new ProcessDataServiceClient();
        private UserServiceClient _userServiceClient=new UserServiceClient();

        private List<Items2> ItemsList=new List<Items2>();
        private List<TabItems2> TabsItems = new List<TabItems2>(); 

        public WindowItemsManagement()
        {
            InitializeComponent();
        }

        public ProcessDataServiceClient ProcessDataServiceClient
        {
            get { return _processDataServiceClient; }
            set { _processDataServiceClient = value; }
        }

        public UserServiceClient UserServiceClient
        {
            get { return _userServiceClient; }
            set { _userServiceClient = value; }
        }

        private void BindListBoxItems()
        {
            BusyIndicatorItems.IsBusy = true;
            Thread thread=new Thread(()=>BindListBoxItemsAsync());
            thread.Start();
        }

        private void BindListBoxItemsAsync()
        {
            ItemsList = ProcessDataServiceClient.GetItems2();
            Dispatcher.BeginInvoke(new Action(() => BindListBoxItemsUI()));
        }

        private void BindListBoxItemsUI()
        {
            ListBoxItems.ItemsSource = ItemsList;
            BusyIndicatorItems.IsBusy = false;
        }

        private void BindTreeViewUsers()
        {
            BusyIndicatorUsers.IsBusy = true;
            Thread thread = new Thread(() => BindTreeViewUsersAsync());
            thread.Start();
        }

        private void BindTreeViewUsersAsync()
        {
            List<Users2> usersList = UserServiceClient.GetUsers2();
            Dispatcher.BeginInvoke(new Action(() => BindTreeViewUsersUI(usersList)));
        }

        private void BindTreeViewUsersUI(List<Users2> usersList)
        {
            foreach (Users2 user in usersList)
            {
                RadTreeViewItem item = new RadTreeViewItem();
                item.Header = user.UserName;
                TreeViewUsers.Items.Add(item);
            }
            BusyIndicatorUsers.IsBusy = false;
        }

        private void BindTreeViewTabs()
        {
            BusyIndicatorTabs.IsBusy = true;
            Thread thread = new Thread(() => BindTreeViewTabsAsync());
            thread.Start();
        }

        private void BindTreeViewTabsAsync()
        {
            TabsItems = ProcessDataServiceClient.GetTabItems2();
            Dispatcher.BeginInvoke(new Action(() => BindTreeViewTabsUI()));
        }

        private void BindTreeViewTabsUI()
        {
            TreeViewTabs.ItemsSource = TabsItems;
            BusyIndicatorTabs.IsBusy = false;
        }

        private void WindowItemsManagement_OnLoaded(object sender, RoutedEventArgs e)
        {
            BindListBoxItems();
            BindTreeViewUsers();
            BindTreeViewTabs();
        }

        private void MenuItemAddItemsToTab_OnClick(object sender, RadRoutedEventArgs e)
        {
            List<string> selectedItems=new List<string>();

            foreach (var selectedItem in ListBoxItems.SelectedItems)
            {
                selectedItems.Add(((Items2) selectedItem).ItemName);
            }

            string selectedTab = TreeViewTabs.SelectedItem.ToString();


        }
    }
}
