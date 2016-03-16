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
    /// Interaction logic for WindowPermission.xaml
    /// </summary>
    public partial class WindowPermission : Window
    {
        IndustrialMonitoringEntities _entities=new IndustrialMonitoringEntities();
        List<ItemTabViewModel> _itemNames=new List<ItemTabViewModel>();
        private User selectedUser = null;

        public WindowPermission()
        {
            InitializeComponent();
        }

        public IndustrialMonitoringEntities Entities
        {
            get { return _entities; }
            set { _entities = value; }
        }

        public List<ItemTabViewModel> ItemNames
        {
            get { return _itemNames; }
            set { _itemNames = value; }
        }

        private void WindowPermission_OnLoaded(object sender, RoutedEventArgs e)
        {
            var users = Entities.Users.ToList();
            ComboBoxUsers.ItemsSource = users.OrderBy(x=>x.LastName);
        }

        private void ComboBoxUsers_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser =(User) ComboBoxUsers.SelectedItem;

            if (selectedUser != null)
            {
                WrapPanelPermission.Children.Clear();

                var items = Entities.Items.OrderBy(x=>x.Order);
                ItemNames.Clear();

                foreach (Item item in items)
                {
                    var tabs = Entities.TabsItems.Where(x => x.ItemId == item.ItemId);

                    foreach (TabsItem tab in tabs)
                    {
                        string name = string.Format("{0} - {1}", tab.Tab.TabName, item.ItemName);
                        ItemTabViewModel itemTabViewModel=new ItemTabViewModel(item.ItemId,tab.Tab.TabId,name);
                        ItemNames.Add(itemTabViewModel);
                    }
                }


                ItemNames = new List<ItemTabViewModel>(ItemNames.OrderBy(x => x.Name));
                GenerateCheckBoxes();
            }
        }

        private void GenerateCheckBoxes()
        {
            var permissions = Entities.UsersItemsPermissions.Where(x => x.UserId == selectedUser.UserId);

            foreach (ItemTabViewModel item in ItemNames)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Name = string.Format("CheckBox_{0}_{1}", item.TabId, item.ItemId);
                checkBox.Content = item.Name;
                checkBox.Margin = new Thickness(30, 5, 30, 5);

                if (permissions.Any(x => x.ItemId == item.ItemId))
                {
                    checkBox.IsChecked = true;
                }
                else
                {
                    checkBox.IsChecked = false;
                }

                WrapPanelPermission.Children.Add(checkBox);
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                return;
            }

            var checkboxes = WrapPanelPermission.Children;
            List<int> ids=new List<int>();

            foreach (UIElement checkbox in checkboxes)
            {
                var chk = (CheckBox) checkbox;

                if (chk.IsChecked.Value)
                {
                    var parts = chk.Name.Split(new string[] { "_" }, StringSplitOptions.None);
                    int id = int.Parse(parts[2]);

                    if (id > 0)
                    {
                        if (!ids.Contains(id))
                        {
                            ids.Add(id);
                        }
                    }
                }
            }

            var items = Entities.Items.ToList();

            foreach (Item item in items)
            {
                if (!ids.Contains(item.ItemId))
                {
                    var permission = Entities.UsersItemsPermissions.FirstOrDefault(x => x.ItemId == item.ItemId 
                    && x.UserId==selectedUser.UserId);

                    if (permission != null)
                    {
                        Entities.UsersItemsPermissions.Remove(permission);
                    }
                }
            }

            Entities.SaveChanges();

            foreach (int id in ids)
            {
                if (!Entities.UsersItemsPermissions.Any(x => x.UserId == selectedUser.UserId && x.ItemId == id))
                {
                    Entities.UsersItemsPermissions.Add(new UsersItemsPermission()
                    {
                        ItemId = id,
                        UserId = selectedUser.UserId
                    });
                }
            }

            Entities.SaveChanges();

            MessageBox.Show("Permissions Saved");
        }

        private void ButtonSelectAll_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                return;
            }

            var checkboxes = WrapPanelPermission.Children;
            List<int> ids = new List<int>();

            foreach (UIElement checkbox in checkboxes)
            {
                var chk = (CheckBox)checkbox;

                chk.IsChecked = true;
            }
        }

        private void ButtonDeselectAll_OnClick(object sender, RoutedEventArgs e)
        {
            if (selectedUser == null)
            {
                return;
            }

            var checkboxes = WrapPanelPermission.Children;
            List<int> ids = new List<int>();

            foreach (UIElement checkbox in checkboxes)
            {
                var chk = (CheckBox)checkbox;

                chk.IsChecked = false;
            }
        }
    }
}
