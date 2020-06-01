using ERP.Client.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ERP.Client.ViewModel
{
    public class ElementViewModel : INotifyPropertyChanged
    {
        private ElementModel _selectedElement;

        public ObservableCollection<ElementModel> Elements { get; private set; }

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
                if (_selectedElement != value)
                {
                    _selectedElement = value;
                    RaisePropertyChanged("SelectedElement");
                }
            }
        }

        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
