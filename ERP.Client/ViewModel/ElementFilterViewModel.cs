using ERP.Client.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.ViewModel
{
    public class ElementFilterViewModel : INotifyPropertyChanged
    {
        private ElementFilterModel _selectedFilter;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ElementFilterModel> Filters { get; private set; }

        public ElementFilterViewModel() => Filters = new ObservableCollection<ElementFilterModel>();

        public void Load(List<ElementFilterModel> filters)
        {
            Filters.Clear();
            foreach (var filter in filters)
            {
                Filters.Add(filter);
            }
        }

        public void Add(ElementFilterModel filter) => Filters.Add(filter);

        public ElementFilterModel SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (_selectedFilter != value)
                {
                    _selectedFilter = value;
                    RaisePropertyChanged("SelectedFilter");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
