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

        private List<Item2> _itemsList=new List<Item2>();
        private List<Tab2> _tabsItems = new List<Tab2>();
        private List<User3> _userItems=new List<User3>();
        private Dictionary<string, bool> _tabItemsExpandStatus; 

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

        private List<Item2> ItemsList
        {
            get { return _itemsList; }
            set { _itemsList = value; }
        }

        private List<Tab2> TabsItems
        {
            get { return _tabsItems; }
            set { _tabsItems = value; }
        }

        private List<User3> UserItems
        {
            get { return _userItems; }
            set { _userItems = value; }
        }

        public Dictionary<string, bool> TabItemsExpandStatus
        {
            get { return _tabItemsExpandStatus; }
            set { _tabItemsExpandStatus = value; }
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
            UserItems = UserServiceClient.GetUsers3();
            Dispatcher.BeginInvoke(new Action(() => BindTreeViewUsersUI()));
        }

        private void BindTreeViewUsersUI()
        {
            TreeViewUsers.ItemsSource = UserItems;
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

        private void AddItemsToTab(string selectedTabName, List<string> selectedItems)
        {
            Thread thread=new Thread(()=>AddItemsToTabAsync(selectedTabName,selectedItems));
            thread.Start();
        }

        private void AddItemsToTabAsync(string selectedTabName, List<string> selectedItems)
        {
            ProcessDataServiceClient.AddItemsToTab(selectedTabName, selectedItems);
            Dispatcher.BeginInvoke(new Action(BindTreeViewTabs));
        }

        private void StatusBarBottom_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearStatusBar();
        }

        private void ShowMsgOnStatusBar(string msg)
        {
            ClearStatusBar();

            StatusBarBottom.Items.Add(msg);
        }

        private void ClearStatusBar()
        {
            StatusBarBottom.Items.Clear();
        }


        private void MenuItemAddItemsToTab_OnClick(object sender, RadRoutedEventArgs e)
        {
            
            List<string> items=new List<string>();
            foreach (Item2 selectedItem in ListBoxItems.SelectedItems)
            {
                items.Add(selectedItem.ItemName);
            }

            Tab2 selectedTab = (Tab2) TreeViewTabs.SelectedItem;

            AddItemsToTab(selectedTab.TabName,items);
        }

        private void MenuItemAddItemsToUser_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ContextMenuTreeViewTabs_OnOpening(object sender, RadRoutedEventArgs e)
        {
            if (TreeViewTabs.SelectedItem == null)
            {
                e.Handled = true;
            }

            if (TreeViewTabs.SelectedItem is Tab2)
            {
                if (ListBoxItems.SelectedItems.Count==0)
                {
                    MenuItemAddItemsToTab.IsEnabled = false;
                }
                else
                {
                    MenuItemAddItemsToTab.IsEnabled = true;    
                }
                
                MenuItemDeleteItemFromTab.IsEnabled = false;
            }
            else if (TreeViewTabs.SelectedItem is ProcessDataServiceReference.Item3)
            {
                MenuItemAddItemsToTab.IsEnabled = false;
                MenuItemDeleteItemFromTab.IsEnabled = true;
            }
        }

        private void MenuItemDeleteItemFromTab_OnClick(object sender, RadRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TreeViewTabs_OnExpanded(object sender, RadRoutedEventArgs e)
        {
            var t = e.OriginalSource;
            throw new NotImplementedException();
        }
    }
}
