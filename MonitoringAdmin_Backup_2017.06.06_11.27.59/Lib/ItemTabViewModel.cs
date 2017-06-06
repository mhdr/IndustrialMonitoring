namespace MonitoringAdmin.Lib
{
    public class ItemTabViewModel
    {
        public int ItemId { get; set; }

        public int TabId { get; set; }

        public string Name { get; set; }

        public ItemTabViewModel(int itemId, int tabId, string name)
        {
            this.ItemId = itemId;
            this.TabId = tabId;
            this.Name = name;
        }
    }
}
