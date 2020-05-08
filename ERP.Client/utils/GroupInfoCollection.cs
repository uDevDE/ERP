using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ERP.Client.utils
{
    public class GroupInfoCollection<T> : ObservableCollection<T>
    {
        public object Key { get; set; }

        public new IEnumerator<T> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
