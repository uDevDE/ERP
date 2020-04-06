using ERP.Contracts.Domain.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class PlantOrderModel : INotifyPropertyChanged, IPlantOrder
    {
        private int _id;
        private string _number;
        private int _parentId;
        private string _name;
        private string _processTemplate;
        private bool _isFinished;
        private string _description;
        private string _contraction;
        private string _section;
        private string _materialRequirement;
        private int _losId;
        private string _processId;

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
        public string Number
        {
            get { return _number; }
            set
            {
                if (_number != value)
                {
                    _number = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int ParentId
        {
            get { return _parentId; }
            set
            {
                if (_parentId != value)
                {
                    _parentId = value;
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
        public string ProcessTemplate
        {
            get { return _processTemplate; }
            set
            {
                if (_processTemplate != value)
                {
                    _processTemplate = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsFinished
        {
            get { return _isFinished; }
            set
            {
                if (_isFinished != value)
                {
                    _isFinished = value;
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
        public string Contraction
        {
            get { return _contraction; }
            set
            {
                if (_contraction != value)
                {
                    _contraction = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Section
        {
            get { return _section; }
            set
            {
                if (_section != value)
                {
                    _section = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string MaterialRequirement
        {
            get { return _materialRequirement; }
            set
            {
                if (_materialRequirement != value)
                {
                    _materialRequirement = value;
                    RaisePropertyChanged();
                }
            }
        }
        public int LosId
        {
            get { return _losId; }
            set
            {
                if (_losId != value)
                {
                    _losId = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ProcessId
        {
            get { return _processId; }
            set
            {
                if (_processId != value)
                {
                    _processId = value;
                    RaisePropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
