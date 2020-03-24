using ERP.Contracts.Domain.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class FolderModel : IFolder, INotifyPropertyChanged
    {
        private string _name;
        private string _relativePath;
        private bool _isRoot;
        private bool _isJob;
        private bool _isWork;
        private Dictionary<string, FileEntryModel> _files;
        private Dictionary<string, FolderModel> _subFolders;

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

        public string RelativePath
        {
            get { return _relativePath; }
            set
            {
                if (_relativePath != value)
                {
                    _relativePath = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsRoot
        {
            get { return _isRoot; }
            set
            {
                if (_isRoot != value)
                {
                    _isRoot = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsJob
        {
            get { return _isJob; }
            set
            {
                if (_isJob != value)
                {
                    _isJob = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsWork
        {
            get { return _isWork; }
            set
            {
                if (_isWork != value)
                {
                    _isWork = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Dictionary<string, FileEntryModel> Files
        {
            get { return _files; }
            set
            {
                if (_files != value)
                {
                    _files = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Dictionary<string, FolderModel> SubFolders
        {
            get { return _subFolders; }
            set
            {
                if (_subFolders != value)
                {
                    _subFolders = value;
                    RaisePropertyChanged();
                }
            }
        }

        public FolderModel()
        {
            _files = new Dictionary<string, FileEntryModel>();
            _subFolders = new Dictionary<string, FolderModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
