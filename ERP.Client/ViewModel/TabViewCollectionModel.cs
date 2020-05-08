using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Collections.ObjectModel;

namespace ERP.Client.ViewModel
{
    public class TabViewCollectionModel
    {
        public ObservableCollection<TabViewItem> Tabs { get; private set; }

        public TabViewCollectionModel() => Tabs = new ObservableCollection<TabViewItem>();
    }
}
