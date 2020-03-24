using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.ComponentModel;

namespace ERP.Client.Model
{
    public class FileEntryInfoModel : IFileEntryInfo, INotifyPropertyChanged
    {
        private int _projectNumber;
        private string _direction;
        private string _projectIdentifier;
        private string _section;
        private string _contraction;
        private string _description;
        private FileEntryExtensionType _extension;

        public int ProjectNumber
        {
            get { return _projectNumber; }
            set
            {
                if (_projectNumber != value)
                {
                    _projectNumber = value;
                    RaisePropertyChanged("ProjectNumber");
                }
            }
        }
        public string Direction
        {
            get { return _direction; }
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    RaisePropertyChanged("Direction");
                }
            }
        }
        public string ProjectIdentifier
        {
            get { return _projectIdentifier; }
            set
            {
                if (_projectIdentifier != value)
                {
                    _projectIdentifier = value;
                    RaisePropertyChanged("ProjectIdentifier");
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
                    RaisePropertyChanged("Section");
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
        public FileEntryExtensionType Extension
        {
            get { return _extension; }
            set
            {
                if (_extension != value)
                {
                    _extension = value;
                    RaisePropertyChanged("Extension");
                    RaisePropertyChanged("ExtensionAsStr");
                }
            }
        }
        public string ExtensionAsStr => _extension.ToString();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
