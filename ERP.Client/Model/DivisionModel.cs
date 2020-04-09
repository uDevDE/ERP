using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class DivisionModel : IDivision, INotifyPropertyChanged
    {
        private int _divisionId;
        private string _name;
        private string _description;
        private DivisionInfoModel _divisionType;
        private int _divisionInfoId;
        private ObservableCollection<ProcessTemplateModel> _processTemplates;

        public DivisionModel() => _divisionType = new DivisionInfoModel();
        public DivisionModel(DivisionModel division)
        {
            _divisionId = division.DivisionId;
            _name = division.Name;
            _description = division.Description;
            _divisionType = division.DivisionType;
            _divisionInfoId = division._divisionInfoId;
        }

        public int DivisionId
        {
            get { return _divisionId; }
            set
            {
                if (_divisionId != value)
                {
                    _divisionId = value;
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
        public DivisionInfoModel DivisionType
        {
            get { return _divisionType; }
            set
            {
                if (_divisionType != value)
                {
                    _divisionType = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int DivisionInfoId
        {
            get { return _divisionInfoId; }
            set
            {
                if (_divisionInfoId != value)
                {
                    _divisionInfoId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public ObservableCollection<ProcessTemplateModel> ProcessTemplates
        {
            get { return _processTemplates; }
            set
            {
                if (_processTemplates != value)
                {
                    _processTemplates = value;
                    RaisePropertyChanged();
                }
            }
        }
        
        public static bool operator ==(DivisionModel src, DivisionModel dest)
        {
            if (ReferenceEquals(src, dest))
                return true;

            if ((object)src == null || (object)dest == null)
                return false;

            return  src.Description == dest.Description && 
                    src.DivisionId == dest.DivisionId && 
                    src.DivisionInfoId == dest.DivisionInfoId && 
                    src.DivisionType == dest.DivisionType && 
                    src.Name == dest.Name;
        }

        public static bool operator !=(DivisionModel src, DivisionModel dest) => !(src == dest);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
