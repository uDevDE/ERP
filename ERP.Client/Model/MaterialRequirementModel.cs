using ERP.Contracts.Domain.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class MaterialRequirementModel : INotifyPropertyChanged, IMaterialRequirement
    {
        private int _id;
        private int _materialNumber;
        private string _articleNumber;
        private string _articleDescription;
        private decimal _count;
        private string _unit;
        private float _length;
        private string _surfaceInside;
        private string _surfaceOutside;
        private string _descriptionStock;
        private string _position;

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
        public int MaterialNumber
        {
            get { return _materialNumber; }
            set
            {
                if (_materialNumber != value)
                {
                    _materialNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ArticleNumber
        {
            get { return _articleNumber; }
            set
            {
                if (_articleNumber != value)
                {
                    _articleNumber = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ArticleDescription
        {
            get { return _articleDescription; }
            set
            {
                if (_articleDescription != value)
                {
                    _articleDescription = value;
                    RaisePropertyChanged();
                }
            }
        }
        public decimal Count
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
        public float Length
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
        public string SurfaceInside
        {
            get { return _surfaceInside; }
            set
            {
                if (_surfaceInside != value)
                {
                    _surfaceInside = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string SurfaceOutside
        {
            get { return _surfaceOutside; }
            set
            {
                if (_surfaceOutside != value)
                {
                    _surfaceOutside = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string DescriptionStock
        {
            get { return _descriptionStock; }
            set
            {
                if (_descriptionStock != value)
                {
                    _descriptionStock = value;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
