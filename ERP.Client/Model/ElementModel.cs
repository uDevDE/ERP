using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ERP.Client.Model
{
    public class ElementModel : INotifyPropertyChanged, IElement, IComparable
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
                    RaisePropertyChanged("Id");
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
                    RaisePropertyChanged("PlantOrderId");
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
                    RaisePropertyChanged("Position");
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
                    RaisePropertyChanged("Description");
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
                    RaisePropertyChanged("Count");
                    RaisePropertyChanged("Percent");
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
                    RaisePropertyChanged("Unit");
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
                    RaisePropertyChanged("Surface");
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
                    RaisePropertyChanged("Children");
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
                    RaisePropertyChanged("ElementType");
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
                    RaisePropertyChanged("Amount");
                    RaisePropertyChanged("Percent");
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
                    RaisePropertyChanged("Length");
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
                    RaisePropertyChanged("Name");
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
                    RaisePropertyChanged("ColourInside");
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
                    RaisePropertyChanged("ColourOutside");
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
                    RaisePropertyChanged("ProfileNumber");
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
        public string Percent
        {
            get
            {
                int amount = Convert.ToInt32(_amount);
                int count = Convert.ToInt32(_count);

                return string.Format("( {0:d} %)", ((amount * 100) / count));
            }
        }

        public ElementModel() => _children = new ObservableCollection<ElementModel>();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public int CompareTo(object obj)
        {
            var element = obj as ElementModel;
            int result = _position.CompareTo(element.Position);

            if (result == 0)
                result = _description.CompareTo(element.Description);

            if (result == 0)
                result = _length.CompareTo(element.Length);

            if (result == 0)
                result = _contraction.CompareTo(element.Contraction);

            return result;
        }
    }
}
