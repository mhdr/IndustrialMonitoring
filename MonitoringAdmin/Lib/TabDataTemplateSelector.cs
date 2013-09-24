using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MonitoringAdmin.ProcessDataServiceReference;

namespace MonitoringAdmin.Lib
{
    public class TabDataTemplateSelector : DataTemplateSelector
    {
        private DataTemplate tabTemplate;
        private HierarchicalDataTemplate userTemplate;

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Tab1)
            {
                
            }
            else if (item is Items2)
            {
                
            }

            return base.SelectTemplate(item, container);
        }
    }
}
