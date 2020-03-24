using ERP.Contracts.Domain.Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ERP.Client.Model
{
    public class FileEntryModel : IFileEntry, INotifyPropertyChanged
    {
        private string _name;
        private string _relativePath;
        private string _filePath;
        private FileEntryInfoModel _fileInfo;

        public string Name
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(_name); }
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
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    RaisePropertyChanged();
                }
            }
        }
        public FileEntryInfoModel FileInfo
        {
            get { return _fileInfo; }
            set
            {
                if (_fileInfo != value)
                {
                    _fileInfo = value;
                    RaisePropertyChanged();
                }
            }
        }

        //public FileEntryModel() => _fileInfo = new FileEntryInfoModel();

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
