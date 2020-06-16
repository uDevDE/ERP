using ERP.Client.Model;
using ERP.Client.utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Data;

namespace ERP.Client.ViewModel
{
    public class ElementViewModel : INotifyPropertyChanged
    {
        private ElementModel _selectedElement;

        public ObservableCollection<ElementModel> Elements { get; private set; }
        public CollectionViewSource GroupedItems { get; private set; }

        public ElementViewModel() => Elements = new ObservableCollection<ElementModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Load(List<ElementModel> elements)
        {
            Elements.Clear();

            /*Elements.Add(new ElementModel()
            {
                Amount = 2,
                Count = 42,
                Position = "382180 RAL 9010",
                Description = "Irgendeine Beschreibung",
                Surface = "RAL 9010 Schwarz"
            });*/

            foreach (var element in elements)
            {
                //var rnd = new System.Random();
                //element.Amount = System.Math.Round(rnd.NextDouble() * (element.Count - 1) + 1);
                Elements.Add(element);
            }
        }

        public void Add(ElementModel element) => Elements.Add(element);

        public ElementModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if (_selectedElement != value && value != null)
                {
                    _selectedElement = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ElementModel Find(string profileNumber, string length, string surface)
        {
            foreach (var element in Elements)
            {
                if (element.Position == profileNumber && element.Length == length && element.Surface == surface)
                {
                    return element;
                }
            }

            return null;
        }

        public CollectionViewSource GroupData(string propertyName)
        {
            ObservableCollection<GroupInfoCollection<ElementModel>> groups = new ObservableCollection<GroupInfoCollection<ElementModel>>();
            var query = from item in Elements
                        orderby item
                        group item by item.GetType().GetProperty(propertyName).GetValue(item, null) into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoCollection<ElementModel> info = new GroupInfoCollection<ElementModel>
                {
                    Key = g.GroupName
                };

                foreach (var item in g.Items)
                {
                    info.Add(item);
                }

                groups.Add(info);
            }


            GroupedItems = new CollectionViewSource
            {
                IsSourceGrouped = true,
                Source = groups
            };

            return GroupedItems;
        }

        public void Remove(ElementModel element) => Elements.Remove(element);

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
