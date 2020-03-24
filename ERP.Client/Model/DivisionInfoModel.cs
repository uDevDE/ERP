using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.ComponentModel;

namespace ERP.Client.Model
{
    public class DivisionInfoModel : IDivisionInfo, INotifyPropertyChanged
    {
        private int _divisionInfoId;
        private string _name;
        private string _description;
        private DivisionType _divisionType;
        private string _machinePath;

        public DivisionInfoModel() { }

        public DivisionInfoModel(DivisionInfoModel division)
        {
            _divisionInfoId = division.DivisionInfoId;
            _name = division.Name;
            _description = division.Description;
            _divisionType = division.DivisionType;
            _machinePath = division.MachinePath;
        }

        public int DivisionInfoId
        {
            get { return _divisionInfoId; }
            set
            {
                if (_divisionInfoId != value)
                {
                    _divisionInfoId = value;
                    RaisePropertyChanged("DivisionInfoId");
                }
            }
        }

        public DivisionType DivisionType
        {
            get { return _divisionType; }
            set
            {
                if (_divisionType != value)
                {
                    _divisionType = value;
                    RaisePropertyChanged("DivisionType");
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

        public string MachinePath
        {
            get { return _machinePath; }
            set
            {
                if (_machinePath != value)
                {
                    _machinePath = value;
                    RaisePropertyChanged("MachinePath");
                }
            }
        }

        public static bool operator ==(DivisionInfoModel src, DivisionInfoModel dest)
        {
            if (ReferenceEquals(src, dest))
                return true;

            if ((object)src == null || (object)dest == null)
                return false;

            return  src.Description == dest.Description && 
                    src.DivisionInfoId == dest.DivisionInfoId && 
                    src.DivisionType == dest.DivisionType && 
                    src.MachinePath == dest.MachinePath && 
                    src.Name == dest.Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(DivisionInfoModel src, DivisionInfoModel dest) => !(src == dest);

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
