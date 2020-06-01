using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class ElementModel : INotifyPropertyChanged, IElement
    {
        private int _id;
        private int _plantOrderId;
        private string _position;
        private string _description;
        private double _count;
        private string _unit;
        private string _surface;
        private ElementType _elementType;
        private double _amount;
        private ObservableCollection<ElementModel> _children;
        private string _name;
        private string _length;
        private string _colourInside;
        private string _colourOutside;
        private string _profileNumber;
        private string _contraction;
        private string _filename;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int PlantOrderId
        {
            get { return _plantOrderId; }
            set
            {
                if (_plantOrderId != value)
                {
                    _plantOrderId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged();
                }
            }
        }
        public double Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Surface
        {
            get { return _surface; }
            set
            {
                if (_surface != value)
                {
                    _surface = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<ElementModel> Children
        {
            get { return _children; }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ElementType ElementType
        {
            get { return _elementType; }
            set
            {
                if (_elementType != value)
                {
                    _elementType = value;
                    RaisePropertyChanged();
                }
            }
        }
        public double Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Length
        {
            get { return _length; }
            set
            {
                if (_length != value)
                {
                    _length = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ColourInside
        {
            get { return _colourInside; }
            set
            {
                if (_colourInside != value)
                {
                    _colourInside = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ColourOutside
        {
            get { return _colourOutside; }
            set
            {
                if (_colourOutside != value)
                {
                    _colourOutside = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ProfileNumber
        {
            get { return _profileNumber; }
            set
            {
                if (_profileNumber != value)
                {
                    _profileNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Contraction
        {
            get { return _contraction; }
            set
            {
                if (_contraction != value)
                {
                    _contraction = value;
                    RaisePropertyChanged("Contraction");
                }
            }
        }
        public string Filename
        {
            get { return _filename; }
            set
            {
                if (_filename != value)
                {
                    _filename = value;
                    RaisePropertyChanged("Filename");
                }
            }
        }

        public ElementModel() => _children = new ObservableCollection<ElementModel>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
